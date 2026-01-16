namespace Agora.Core.Dtos.Game.Commands.Other;

public class ShowMessageCommandDto : CommandDto
{
    public string Message { get; set; }
    public int? DurationMillis { get; set; }
    public bool BlockSequence { get; set; }
}