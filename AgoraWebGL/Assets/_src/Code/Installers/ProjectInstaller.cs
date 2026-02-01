using _src.Code.App.Logic;
using _src.Code.App.Managers;
using _src.Code.App.Services;
using _src.Code.Core.Actors;
using _src.Code.Core.Interfaces.Proxies;
using _src.Code.Core.Interfaces.Services;
using _src.Code.Core.Signals.Inputs;
using _src.Code.Core.Signals.Other;
using _src.Code.Network.HttpProxies;
using _src.Code.Network.HubController;
using _src.Code.Network.HubProxy;
using _src.Settings;
using Agora.Core.Actors;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

namespace _src.Code.Installers
{
    public class ProjectInstaller : MonoInstaller<ProjectInstaller>
    {
        [Header("Scene References")]
        [SerializeField] private AudioSource backgroundMusicSource;
        [SerializeField] private AudioSource soundEffectSource;
        
        [Header("Configs")]
        [SerializeField] private UIConfig uiConfig;
        
        public override void InstallBindings()
        {
            // Register SignalBus
            SignalBusInstaller.Install(Container);
            ServiceLocator.Initialize(Container);
            
            // startup
            Container.BindInterfacesAndSelfTo<Startup>().AsTransient();
            
            // UI
            Container.Bind<UIConfig>().FromInstance(uiConfig).AsSingle();
            Container.Bind<UIDocument>().FromComponentInHierarchy().AsSingle();
            
            // -----------------------------------------------------------
            // Network
            // -----------------------------------------------------------
            
            // controllers
            Container.Bind<IHubController>().To<HubController>().AsSingle().NonLazy();
            
            // hub proxy
            Container.BindInterfacesAndSelfTo<HubProxy>().AsSingle();
            
            // https proxies
            Container.Bind<IAuthHttpProxy>().To<AuthHttpProxy>().AsTransient();
            Container.Bind<ILobbyHttpProxy>().To<LobbyHttpProxy>().AsTransient();
            Container.Bind<IGameHttpProxy>().To<GameHttpProxy>().AsTransient();
            Container.Bind<IUtilityHttpProxy>().To<UtilityHttpProxy>().AsTransient();
            
            // -----------------------------------------------------------
            // Logic
            // -----------------------------------------------------------
            
            // Handler
            Container.BindInterfacesAndSelfTo<AppLogic>().AsSingle().NonLazy();
            
            // Managers
            Container.BindInterfacesAndSelfTo<InputManager>().AsSingle().NonLazy();
            
            // Services
            Container.Bind<IAnimationQueueService>().To<AnimationQueueService>().AsSingle();
            Container.Bind<ICharacterService>().To<CharacterService>().AsSingle();
            Container.Bind<IClientDataService>().To<ClientDataService>().AsSingle();
            Container.Bind<IConnectionService>().To<ConnectionService>().AsSingle();
            Container.Bind<ILobbyService>().To<LobbyService>().AsSingle();
            Container.Bind<ISceneService>().To<SceneService>().AsSingle();
            Container.Bind<IVisualElementService>().To<VisualElementService>().AsSingle();
            
            Container.Bind<IAudioService>()
                .To<AudioService>()
                .AsSingle()
                .WithArguments(backgroundMusicSource, soundEffectSource)
                .NonLazy();
            
            // -----------------------------------------------------------
            // Signals
            // -----------------------------------------------------------
            
            Container.DeclareSignal<StartupCompleteSignal>();
            Container.DeclareSignal<SceneReadySignal>();
            
            Container.DeclareSignal<ClickSignal>().OptionalSubscriber();
            Container.DeclareSignal<DragEndSignal>().OptionalSubscriber();
            Container.DeclareSignal<DragStartSignal>().OptionalSubscriber();
            Container.DeclareSignal<DragUpdateSignal>().OptionalSubscriber();
            Container.DeclareSignal<HoldEndSignal>().OptionalSubscriber();
            Container.DeclareSignal<HoldStartSignal>().OptionalSubscriber();
            Container.DeclareSignal<HoverEndSignal>().OptionalSubscriber();
            Container.DeclareSignal<HoverStartSignal>().OptionalSubscriber();
            Container.DeclareSignal<ScrollSignal>().OptionalSubscriber();

            Container.DeclareSignal<ResponseSignal<Result>>();
        }
    }
}