using Agora.Core.Enums;

namespace Agora.Core.Dtos.Game.GameModules;

public class TokenDto : GameModuleDto
{
    public override GameModuleType Type => GameModuleType.Token;
    
    public string StartZoneId { get; set; }
}