using Agora.Core.Enums;

namespace Agora.Core.Dtos.Game.GameModules;

public class CardDto : GameModuleDto
{
    public override GameModuleType Type => GameModuleType.Card;
    
    public string FrontImage { get; set; }
    public string BackImage { get; set; }
}