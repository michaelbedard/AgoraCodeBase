using System;
using System.Threading.Tasks;
using _src.Code.Core.Interfaces.Handlers;
using _src.Code.Core.Interfaces.Proxies;
using _src.Code.Core.Interfaces.Services;
using _src.Code.Core.Signals.Other;
using UnityEngine;
using Zenject;

namespace _src.Code
{
    public class Startup : IInitializable
    {
        private readonly SignalBus _signalBus;
        private readonly ISceneService _sceneService;
        private readonly IClientDataService _clientDataService;
        private readonly IAudioService _audioService;
        private readonly IGameHttpProxy _gameHttpProxy;
        private readonly IUtilityHttpProxy _utilityHttpProxy;
        private readonly IVisualElementService _visualElementService;
        private readonly IAppLogic _appLogic;

        [Inject]
        public Startup(
            SignalBus signalBus,
            ISceneService sceneService,
            IClientDataService clientDataService,
            IConnectionService connectionService,
            IAudioService audioService,
            IGameHttpProxy gameHttpProxy,
            IUtilityHttpProxy utilityHttpProxy,
            IVisualElementService visualElementService,
            IAppLogic appLogic)
        {
            _signalBus = signalBus;
            _sceneService = sceneService;
            _clientDataService = clientDataService;
            _audioService = audioService;
            _gameHttpProxy = gameHttpProxy;
            _utilityHttpProxy = utilityHttpProxy;
            _visualElementService = visualElementService;
            _appLogic = appLogic;
        }

        public void Initialize()
        {
            _ = RunAsync();
        }

        public async Task RunAsync()
        {
            try
            {
                Debug.Log("Loading Scene...");
                await _sceneService.LoadAdditiveScene();
                await SetupAudio();
                
                Debug.Log("Login in...");
                await Login();

                Debug.Log("Startup Complete");
                _signalBus.Fire<StartupCompleteSignal>();
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
        
        private async Task SetupAudio()
        {
            await _audioService.PlayBackgroundMusicAsync("Assets/_src/Audio/Viking_Tavern.mp3");

            _clientDataService.MusicVolume = 50;
            _clientDataService.SoundEffectVolume = 50;
            
            _audioService.SetBackgroundMusicVolume(50);
            _audioService.SetSoundEffectsVolume(50);
        }

        private async Task Login()
        {
            #if UNITY_EDITOR
            
            await _appLogic.Login("001", "mock_jade2");
            
            #endif
        }
    }
}