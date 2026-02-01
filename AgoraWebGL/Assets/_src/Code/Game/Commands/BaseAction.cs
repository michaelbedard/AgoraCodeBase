using _src.Code.Core.Actors;
using _src.Code.Core.Interfaces.Services;
using _src.Code.Core.Signals.Game;
using Agora.Core.Dtos.Game.Commands;
using Zenject;

namespace _src.Code.Game.Commands
{
    public abstract class BaseAction<TAction, TAnimation>  : BaseAnimation<TAnimation>
        where TAction : CommandDto
        where TAnimation : TAction
    {
        protected IClientDataService ClientDataService => ServiceLocator.GetService<IClientDataService>();
        
        [Inject]
        protected BaseAction(SignalBus signalBus, IAnimationQueueService animationQueueService, IGameModuleService gameModuleService) 
            : base(signalBus, animationQueueService, gameModuleService)
        {
            signalBus.Subscribe<GameActionSignal>(s => Allow(s.Action));
        }

        protected abstract void AllowCore(TAction action);

        private void Allow(CommandDto action)
        {
            if (action is not TAction validAction) 
                return;

            AllowCore(validAction);
        }
    }
}