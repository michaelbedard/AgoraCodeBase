using Agora.Core.Enums;

namespace Agora.Core.Dtos.Game.GameModules;

public class DiceDto : GameModuleDto
{
    public override GameModuleType Type => GameModuleType.Dice;
    
    public int NumberOfSides { get; set; }
}