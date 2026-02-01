using Agora.Core.Actors;
using Agora.Engine.Commands.Inputs;
using Agora.Engine.Entities;
using Agora.Engine.Entities.@base;

namespace Agora.Engine.Games.Uno;

public partial class Uno : BaseGame
{
    protected override string Title => "Uno";
    protected override string RulesAddress => "XXX";
    protected override string MusicAddress => "XXX";
    protected override string EnvironmentAddress => "XXX";
    
    // --- Game State ---
    private Deck _deck = new Deck();
    private Zone _discardPile = new Zone();
    private int _currentPlayerIndex = 0;
    private int _direction = 1;
    private string _activeColor = "";

    protected override List<IGameModule> Setup()
    {
        _deck = new Deck()
        {
            Name = "DrawingDeck", 
            Color = "#228B22", 
            TopImage = "Assets/_src/Games/Uno/back.png",
            Position = new Position(-2, 0),
            Cards = GenerateUnoDeck()
        };
        _discardPile = new Zone()
        {
            Name = "DiscardPile",
            Position = new Position(2, 0),
        };

        return new List<IGameModule>() { _deck, _discardPile };
    }

    protected override Dictionary<string, string> GetDescriptions(Player player)
    {
        var topCard = GetTopCard();
        string topCardName = topCard != null ? topCard.Name : "Empty";

        var descriptions = new Dictionary<string, string>()
        {
            { _deck.Id, $"Deck: {_deck.Cards.Count} cards" },
            { _discardPile.Id, $"Top Card: {topCardName} (Current Color: {_activeColor})" },
        };

        foreach (var p in Players)
        {
            if (p == player) continue;
            descriptions[p.Id] = $"{p.Username}: {p.Hand.Count} cards";
        }

        return descriptions;
    }

    // --- Deck Generation ---
    private List<Card> GenerateUnoDeck()
    {
        var cards = new List<Card>();
        string[] colors = { "red", "blue", "green", "yellow" };

        foreach (var color in colors)
        {
            // 0 Card (1 per color)
            cards.Add(new UnoCard(0, color));

            // 1-9 Cards (2 per color)
            for (int i = 1; i <= 9; i++)
            {
                cards.Add(new UnoCard(i, color));
                cards.Add(new UnoCard(i, color));
            }

            // Action Cards (2 per color)
            for (int i = 0; i < 2; i++)
            {
                cards.Add(new SkipCard(color));
                cards.Add(new ReverseCard(color));
                cards.Add(new PlusTwoCard(color));
            }
        }

        // Wild Cards (4 each)
        for (int i = 0; i < 4; i++)
        {
            cards.Add(new WildCard());
            cards.Add(new PlusFourCard());
        }

        return cards;
    }

    // --- Card Definitions ---

    public class UnoCard : Card.SpecialCard<Uno>
    {
        public int Value { get; set; } // 0-9 for numbers, 10=Skip, 11=Reverse, 12=+2, 13=Wild, 14=+4
        public string Color { get; set; } // red, blue, green, yellow, black

        public UnoCard(int value, string color)
        {
            Value = value;
            Color = color;
            Name = $"{value}_{color}";
            FrontImage = $"Assets/_src/Games/Uno/{Color}/{Name}.png";
            BackImage = "Assets/_src/Games/Uno/back.png";
        }
    }
    
    public abstract class SpecialUnoCard(int value, string color) : UnoCard(value, color)
    {
    }

    public class SkipCard(string color) : SpecialUnoCard(10, color)
    {
        public override Task ApplyEffectAsync(Uno game)
        {
            Console.WriteLine($"Turn Skipped!");
            game.AdvanceTurn(2);
            return Task.CompletedTask;
        }
    }

    public class ReverseCard(string color) : SpecialUnoCard(11, color)
    {
        public override Task ApplyEffectAsync(Uno game)
        {
            Console.WriteLine($"Direction Reversed!");
            if (game.Players.Count == 2)
            {
                game.AdvanceTurn(2);
            }
            else
            {
                game.ReverseTurnOrder();
            }
            return Task.CompletedTask;
        }
    }

    public class PlusTwoCard(string color) : SpecialUnoCard(12, color)
    {
        public override Task ApplyEffectAsync(Uno game)
        {
            Console.WriteLine($"+2 Applied!");
            game.DrawCards(2);
            return Task.CompletedTask;
        }
    }

    public class WildCard : SpecialUnoCard
    {
        public WildCard() : base(13, "black") 
        { 
            Name = "wild_black";
            FrontImage = "Assets/_src/Games/Uno/black/wild.png";
        }

        public override async Task ApplyEffectAsync(Uno game)
        {
            var player = game.Players[game._currentPlayerIndex];
            
            Console.WriteLine("PLAYED WILD");

            var result = await game.Ask(new CardChoice(player, [
                new Card() { Name = "Red", FrontImage = "Assets/_src/Games/Uno/empty_red.png" },
                new Card() { Name = "Blue", FrontImage = "Assets/_src/Games/Uno/empty_blue.png" },
                new Card() { Name = "Green", FrontImage = "Assets/_src/Games/Uno/empty_green.png" },
                new Card() { Name = "Yellow", FrontImage = "Assets/_src/Games/Uno/empty_yellow.png" }
            ]));
            
            game._activeColor = result.Name.ToLower();
            game.AdvanceTurn();
        }
    }

    public class PlusFourCard : SpecialUnoCard
    {
        public PlusFourCard() : base(14, "black") 
        { 
            Name = "+4_black"; 
            FrontImage = "Assets/_src/Games/Uno/black/plus4.png";
        }

        public override async Task ApplyEffectAsync(Uno game)
        {
            var player = game.Players[game._currentPlayerIndex];
            
            var result = await game.Ask(new CardChoice(player, [
                new Card() { Name = "Red", FrontImage = "Assets/_src/Games/Uno/empty_red.png" },
                new Card() { Name = "Blue", FrontImage = "Assets/_src/Games/Uno/empty_blue.png" },
                new Card() { Name = "Green", FrontImage = "Assets/_src/Games/Uno/empty_green.png" },
                new Card() { Name = "Yellow", FrontImage = "Assets/_src/Games/Uno/empty_yellow.png" }
            ]));
            
            game._activeColor = result.Name.ToLower();
            
            Console.WriteLine($"+4 Played! Color changed to {game._activeColor}");
            game.DrawCards(4);
        }
    }
}