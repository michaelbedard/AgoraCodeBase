using Agora.Core.Dtos.Game.Commands;
using Agora.Core.Dtos.Game.Commands.Actions;
using Agora.Core.Extensions;
using Agora.Engine.Commands._base;
using Agora.Engine.Entities;

namespace Agora.Engine.Commands.Actions;

public class ShuffleDeck : BaseGameAction
{
    public Deck Deck;
    
    public ShuffleDeck(Player player, Deck deck) : base(player)
    {
        Deck = deck;
    }
    
    public override GameActionResult Execute()
    {
        var result = new GameActionResult();
        if (!Deck.Cards.Any()) return result;

        Deck.Cards.Shuffle();

        result.SendToAll(new ShuffleDeckAnimationDto()
        {
            // no player
            DeckId = Deck.Id,
        });

        return result;
    }

    protected override CommandDto CreateDto()
    {
        return new DrawCardActionDto()
        {
            DeckId = Deck.Id
        };
    }
}