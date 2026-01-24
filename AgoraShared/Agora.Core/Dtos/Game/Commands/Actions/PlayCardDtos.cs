namespace Agora.Core.Dtos.Game.Commands.Actions;

public class PlayCardActionDto : CommandDto
{
    public string CardId { get; set; }
    public string? ZoneId { get; set; }
}

public class PlayCardAnimationDto : PlayCardActionDto
{
}