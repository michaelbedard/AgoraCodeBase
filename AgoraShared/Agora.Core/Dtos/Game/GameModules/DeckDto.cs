namespace Agora.Core.Dtos.Game.GameModules;

public class DeckDto : GameModuleDto
{
    public string Color { get; set; }
    public string TopImage { get; set; }
    public List<CardDto> Cards { get; set; } = new();
}