namespace Agora.Core.Dtos.Game.Commands.Actions;

public class ActivateZoneActionDto: CommandDto
{
    public string ZoneId { get; set; }
}

public class ActivateZoneAnimationDto : ActivateZoneActionDto
{
    public bool ShouldAnimate { get; set; }
}