namespace Agora.Core.Dtos.Game.Commands.Actions;

public class PlayCardInsideZoneActionDto : CommandDto
{
    public string CardId { get; set; }
    public string ZoneId { get; set; }
    
    public bool CanDropAnywhere { get; set; }
}

public class PlayCardInsideZoneAnimationDto : PlayCardInsideZoneActionDto
{
    public string PlayerId { get; set; }
}