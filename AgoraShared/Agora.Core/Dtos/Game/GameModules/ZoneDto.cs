using Agora.Core.Actors;
using Agora.Core.Enums;

namespace Agora.Core.Dtos.Game.GameModules;

public class ZoneDto : GameModuleDto
{
    public override GameModuleType Type => GameModuleType.Zone;
        
    public float Width { get; set; }
    public float Height { get; set; }
    public ZoneStackingMethod StackingMethod { get; set; }
    public List<CardDto> Cards { get; set; } = new();
}