using _src.Code.App.Managers;
using _src.Code.Core.Interfaces.Handlers;
using _src.Code.Core.Interfaces.Managers;
using _src.Code.Core.Interfaces.Services;
using Zenject;

namespace _src.Code.App.Logic.Game
{
    public partial class GameLogic : IGameLogic
    {
        private readonly SignalBus _signalBus;
        private readonly IGameDataService _gameDataService;
        private readonly ISceneService _sceneService;
        private readonly IVisualElementService _visualElementService;
        private readonly IClientDataService _clientDataService;
        private readonly IAudioService _audioService;
        private readonly IBoardPlaneService _boardPlaneService;
        private readonly IGameModuleService _gameModuleService;
        private readonly IAnimationQueueService _animationQueueService;
        private readonly IDescriptionManager _descriptionManager; 
        private readonly CameraManager _cameraManager; 

        [Inject]
        public GameLogic(
            SignalBus signalBus,
            IGameDataService gameDataService,
            ISceneService sceneService,
            IVisualElementService visualElementService,
            IClientDataService clientDataService,
            IAudioService audioService,
            IBoardPlaneService boardPlaneService,
            IGameModuleService gameModuleService,
            IAnimationQueueService animationQueueService,
            IDescriptionManager descriptionManager,
            CameraManager cameraManager)
        {
            _signalBus = signalBus;
            _gameDataService = gameDataService;
            _sceneService = sceneService;
            _visualElementService = visualElementService;
            _clientDataService = clientDataService;
            _audioService = audioService;
            _boardPlaneService = boardPlaneService;
            _gameModuleService = gameModuleService;
            _animationQueueService = animationQueueService;
            _descriptionManager = descriptionManager;
            _cameraManager = cameraManager;
        }
    }
}