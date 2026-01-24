using Agora.Core.Dtos.Game.GameModules;
using Agora.Engine.Entities.@base;

namespace Agora.Engine.Entities;

public class Zone : ZoneDto, IGameModule
{
    public new List<CardDto> Cards { get; set; } = new();
    
    public GameModuleDto ToDto()
    {
        return new ZoneDto()
        {
            Id = Id,
            Name = Name,
            Type = Type,
            Width = Width,
            Height = Height,
            StackingMethod = StackingMethod,
            Cards = Cards,
        };
    }
}