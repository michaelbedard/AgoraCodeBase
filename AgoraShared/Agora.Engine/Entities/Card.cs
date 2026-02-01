using Agora.Core.Dtos.Game.GameModules;
using Agora.Engine.Entities.@base;

namespace Agora.Engine.Entities;

public class Card: CardDto, IGameModule
{
    public GameModuleDto ToDto()
    {
        return new CardDto()
        {
            Id = Id,
            Name = Name,
            FrontImage = FrontImage,
            BackImage = BackImage,
        };
    }
    
    public class SpecialCard<T>: Card where T : BaseGame
    {
        public virtual Task ApplyEffectAsync(T game)
        {
            return Task.CompletedTask;
        }
    }
}