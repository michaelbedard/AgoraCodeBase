using _src.Code.Core.Interfaces.GameModules;
using _src.Code.Core.Interfaces.Services;
using Agora.Core.Dtos.Game.Commands.Other;
using Zenject;

namespace _src.Code.Game.Commands.Other
{
    public class PlayerTurn : BaseAnimation<PlayerTurnCommandDto>
    {
        private readonly IGameDataService _gameDataService;
        private readonly IVisualElementService _visualElementService;
        
        public PlayerTurn(
            SignalBus signalBus,
            IAnimationQueueService animationQueueService,
            IGameModuleService gameModuleService,
            IGameDataService gameDataService,
            IVisualElementService visualElementService) 
            : base(signalBus, animationQueueService, gameModuleService)
        {
            _gameDataService = gameDataService;
            _visualElementService = visualElementService;
        }

        protected override void AnimateCore(PlayerTurnCommandDto animation)
        {
            AnimationQueueService.Push(async () =>
            {
                // // show player turns
                // foreach (var playerModule in GameModuleService.GetAllGameModules<IPlayer>())
                // {
                //     playerModule.IsPlayerTurn = animation.PlayersTakingTurn.Contains(playerModule.Id);
                //     playerModule.UpdateGlowColor();
                // }
                //
                // // continue
                // AnimationQueueService.Next();
            });
        }
    }
}