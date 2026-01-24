using Agora.Core.Dtos.Game.Commands;
using Agora.Core.Dtos.Game.Commands.Actions;
using Agora.Engine.Commands._base;
using Agora.Engine.Entities;

namespace Agora.Engine.Commands.Actions;

public class PlayCard : BaseGameAction
{
    public Card Card;
    public Zone? Zone;

    public PlayCard(Player player, Card card, Zone? zone = null) : base(player)
    {
        Card = card;
        Zone = zone;
    }
    
    public override GameActionResult Execute()
    {
        var result = new GameActionResult();
        
        Player.Hand.Remove(Card);
        Zone?.Cards.Add(Card);

        result.SendToAll(new PlayCardAnimationDto()
        {
            PlayerId = Player.Id,
            CardId = Card.Id,
            ZoneId = Zone?.Id,
        });

        return result;
    }

    protected override CommandDto CreateDto()
    {
        return new PlayCardActionDto()
        {
            CardId = Card.Id,
            ZoneId = Zone?.Id,
        };
    }
}