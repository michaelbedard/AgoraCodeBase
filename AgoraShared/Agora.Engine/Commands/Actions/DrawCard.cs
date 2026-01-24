using Agora.Core.Dtos.Game.Commands;
using Agora.Core.Dtos.Game.Commands.Actions;
using Agora.Engine.Commands._base;
using Agora.Engine.Entities;

namespace Agora.Engine.Commands.Actions;

public class DrawCard : BaseGameAction
{
    public Deck Deck;
    
    public DrawCard(Player player, Deck deck) : base(player)
    {
        Deck = deck;
    }
    
    public override GameActionResult Execute()
    {
        var result = new GameActionResult();
        if (!Deck.Cards.Any()) return result;
        
        var card = Deck.Cards[0];
        Deck.Cards.RemoveAt(0);
        Player.Hand.Add(card);

        result.SendToAll(new DrawCardAnimationDto()
        {
            PlayerId = Player.Id,
            DeckId = Deck.Id,
            CardId = card.Id
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