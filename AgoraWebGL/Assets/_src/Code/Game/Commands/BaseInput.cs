using System;
using _src.Code.Core.Interfaces.Services;
using Agora.Core.Dtos.Game.Commands;
using Zenject;

namespace _src.Code.Game.Commands
{
    public abstract class BaseInput
    {
        public SignalBus SignalBus;
        public IAnimationQueueService AnimationQueueService;
        public IGameModuleService GameModuleService;
        
        public int InputId;

        public abstract void Ask(CommandDto input);
        public abstract void Cancel();
    }
    
    public abstract class BaseInput<TInput> : BaseInput
    {
        protected abstract void AskCore(TInput input);

        public override void Ask(CommandDto input)
        {
            if (input is TInput validInput)
            {
                AskCore(validInput);
                return;
            }

            throw new Exception("Invalid command dto");
        }
    }
}