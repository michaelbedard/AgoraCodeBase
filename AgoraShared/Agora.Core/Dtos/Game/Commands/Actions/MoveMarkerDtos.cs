namespace Agora.Core.Dtos.Game.Commands.Actions;

public class MoveMarkerActionDto: CommandDto
{
    public string MarkerId { get; set; }
    public string ZoneId { get; set; }
}

public class MoveMarkerAnimationDto : MoveMarkerActionDto
{
    public string[] Path { get; set; }
}