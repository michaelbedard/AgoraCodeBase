using Agora.Core.Actors;
using Agora.Core.Enums;

namespace Agora.Core.Dtos.Game.GameModules;

public abstract class GameModuleDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public Position? Position { get; set; }
    public string? OwnerId { get; set; }
    
    public abstract GameModuleType Type { get; }
}