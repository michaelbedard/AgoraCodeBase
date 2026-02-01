using Agora.Core.Dtos.Game.Commands;
using Agora.Core.Dtos.Game.Commands.Actions;
using Agora.Engine.Commands._base;
using Agora.Engine.Entities;

namespace Agora.Engine.Commands.Actions;

public class PlayCard : BaseGameAction
{
    public Card Card;

    public PlayCard(Player player, Card card) : base(player)
    {
        Card = card;
    }
    
    public override GameActionResult Execute()
    {
        var result = new GameActionResult();
        
        Player.Hand.Remove(Card);

        result.SendToAll(new PlayCardAnimationDto()
        {
            PlayerId = Player.Id,
            CardId = Card.Id,
        });

        return result;
    }

    protected override CommandDto CreateDto()
    {
        return new PlayCardActionDto()
        {
            PlayerId = Player.Id,
            CardId = Card.Id,
        };
    }
}