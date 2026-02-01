using _src.Code.App.Managers;
using _src.Code.Core.Interfaces.Logic;
using _src.Code.Core.Interfaces.Managers;
using _src.Code.Core.Interfaces.Proxies;
using _src.Code.Core.Interfaces.Services;
using Zenject;

namespace _src.Code.App.Logic.Game
{
    public partial class GameLogic : IGameLogic
    {
        private readonly SignalBus _signalBus;
        private readonly ISceneService _sceneService;
        private readonly IVisualElementService _visualElementService;
        private readonly IClientDataService _clientDataService;
        private readonly IAudioService _audioService;
        private readonly IBoardPlaneService _boardPlaneService;
        private readonly IGameModuleService _gameModuleService;
        private readonly IAnimationQueueService _animationQueueService;
        private readonly IDescriptionManager _descriptionManager; 
        private readonly CameraManager _cameraManager; 
        private readonly IGameHubProxy _gameHubProxy; 

        [Inject]
        public GameLogic(
            SignalBus signalBus,
            ISceneService sceneService,
            IVisualElementService visualElementService,
            IClientDataService clientDataService,
            IAudioService audioService,
            IBoardPlaneService boardPlaneService,
            IGameModuleService gameModuleService,
            IAnimationQueueService animationQueueService,
            IDescriptionManager descriptionManager,
            CameraManager cameraManager,
            IHubController hubController,
            IGameHubProxy gameHubProxy)
        {
            _signalBus = signalBus;
            _sceneService = sceneService;
            _visualElementService = visualElementService;
            _clientDataService = clientDataService;
            _audioService = audioService;
            _boardPlaneService = boardPlaneService;
            _gameModuleService = gameModuleService;
            _animationQueueService = animationQueueService;
            _descriptionManager = descriptionManager;
            _cameraManager = cameraManager;
            _gameHubProxy = gameHubProxy;
            
            // for now...
            hubController.RegisterGameLogic(this);
        }
    }
}