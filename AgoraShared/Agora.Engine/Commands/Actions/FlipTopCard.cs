using Agora.Core.Dtos.Game.Commands;
using Agora.Core.Dtos.Game.Commands.Actions;
using Agora.Engine.Commands._base;
using Agora.Engine.Entities;

namespace Agora.Engine.Commands.Actions;

public class FlipTopCard : BaseGameAction
{
    public Deck Deck;
    public Zone Zone;
    
    public FlipTopCard(Player player, Deck deck, Zone zone) : base(player)
    {
        Deck = deck;
        Zone = zone;
    }
    
    public override GameActionResult Execute()
    {
        var result = new GameActionResult();
        if (!Deck.Cards.Any()) return result;
        
        var flippedCard = Deck.Cards.First();
        
        Deck.Cards.Remove(flippedCard);
        Zone.Cards.Add(flippedCard);

        result.SendToAll(new FlipTopCardAnimationDto()
        {
            DeckId = Deck.Id,
            ZoneId = Zone.Id,
            CardId = flippedCard.Id
        });

        return result;
    }

    protected override CommandDto CreateDto()
    {
        return new FlipTopCardActionDto()
        {
            DeckId = Deck.Id,
            ZoneId = Zone.Id,
        };
    }
}