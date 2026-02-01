using Agora.Core.Dtos.Game.GameModules;
using Agora.Core.Enums;

namespace Agora.Engine.Entities.@base;

public interface IGameModule
{
    public string Id { get; set; }
    public string Name { get; set; }
    public GameModuleType Type { get; }
    
    public GameModuleDto ToDto();
}