using System;
using _src.Code.Core.Interfaces.Logic;
using _src.Code.Core.Interfaces.Proxies;
using _src.Code.Core.Interfaces.Services;
using UnityEngine.UIElements;
using Zenject;

namespace _src.Code.App.Logic
{
    public partial class AppLogic : IAppLogic, IInitializable, IDisposable
    {
        private readonly ISceneService _sceneService;
        private readonly IVisualElementService _visualElementService;
        private readonly IAudioService _audioService;
        private readonly IAuthHttpProxy _authHttpProxy;
        private readonly IUtilityHttpProxy _utilityHttpProxy;
        private readonly IHubProxy _hubProxy;
        private readonly IClientDataService _clientDataService;
        
        // --- Shared State ---
        private bool _isAppReady;

        [Inject]
        public AppLogic(
            ISceneService sceneService, 
            IVisualElementService visualElementService,
            IAudioService audioService,
            IAuthHttpProxy authHttpProxy,
            IUtilityHttpProxy utilityHttpProxy,
            IHubProxy hubProxy,
            IClientDataService clientDataService)
        {
            _sceneService = sceneService;
            _visualElementService = visualElementService;
            _audioService = audioService;
            _authHttpProxy = authHttpProxy;
            _utilityHttpProxy = utilityHttpProxy;
            _hubProxy = hubProxy;
            _clientDataService = clientDataService;
        }
    }
}