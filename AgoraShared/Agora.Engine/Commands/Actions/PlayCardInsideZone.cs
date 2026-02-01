using Agora.Core.Dtos.Game.Commands;
using Agora.Core.Dtos.Game.Commands.Actions;
using Agora.Engine.Commands._base;
using Agora.Engine.Entities;

namespace Agora.Engine.Commands.Actions;

public class PlayCardInsideZone : BaseGameAction
{
    public Card Card;
    public Zone Zone;

    public PlayCardInsideZone(Player player, Card card, Zone zone) : base(player)
    {
        Card = card;
        Zone = zone;
    }
    
    public override GameActionResult Execute()
    {
        var result = new GameActionResult();
        
        Player.Hand.Remove(Card);
        Zone.Cards.Add(Card);

        result.SendToAll(new PlayCardInsideZoneAnimationDto()
        {
            PlayerId = Player.Id,
            CardId = Card.Id,
            ZoneId = Zone.Id
        });

        return result;
    }

    protected override CommandDto CreateDto()
    {
        return new PlayCardInsideZoneActionDto()
        {
            CardId = Card.Id,
            ZoneId = Zone.Id
        };
    }
}