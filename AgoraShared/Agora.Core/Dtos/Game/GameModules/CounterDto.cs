using Agora.Core.Enums;

namespace Agora.Core.Dtos.Game.GameModules;

public class CounterDto : GameModuleDto
{
    public override GameModuleType Type => GameModuleType.Counter;
}