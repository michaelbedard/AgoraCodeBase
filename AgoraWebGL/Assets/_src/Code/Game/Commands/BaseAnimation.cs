using _src.Code.Core.Actors;
using _src.Code.Core.Interfaces.Services;
using _src.Code.Core.Signals.Game;
using Agora.Core.Dtos.Game.Commands;
using Zenject;

namespace _src.Code.Game.Commands
{
    public abstract class BaseAnimation<TAnimation> where TAnimation : CommandDto
    {
        protected readonly IAnimationQueueService AnimationQueueService;
        protected readonly IGameModuleService GameModuleService;
        protected readonly IAudioService AudioService;

        [Inject]
        protected BaseAnimation(SignalBus signalBus, IAnimationQueueService animationQueueService, IGameModuleService gameModuleService)
        {
            AnimationQueueService = animationQueueService;
            GameModuleService = gameModuleService;
            AudioService = ServiceLocator.GetService<IAudioService>();
            
            signalBus.Subscribe<GameAnimationSignal>(s => Animate(s.Animation));
        }
        
        protected abstract void AnimateCore(TAnimation animation);

        private void Animate(CommandDto action)
        {
            if (action is not TAnimation validAction) 
                return;

            AnimateCore(validAction);
        }
    }
}