namespace Agora.Core.Dtos.Game.Commands.Inputs;

public class SelectAmountDto : CommandDto
{
    public string Label { get; set; }
    public int Min { get; set; }
    public int Max { get; set; }
}