using Agora.Core.Enums;

namespace Agora.Core.Dtos.Game.GameModules;

public class DeckDto : GameModuleDto
{
    public override GameModuleType Type => GameModuleType.Deck;
    
    public string Color { get; set; }
    public string TopImage { get; set; }
    public List<CardDto> Cards { get; set; } = new();
    
}