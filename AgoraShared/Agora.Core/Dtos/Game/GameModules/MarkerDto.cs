using Agora.Core.Enums;

namespace Agora.Core.Dtos.Game.GameModules;

public class MarkerDto : GameModuleDto
{
    public override GameModuleType Type => GameModuleType.Marker;
    
    public string StartZoneId { get; set; }
    public string? IdentifiableColor { get; set; }
}