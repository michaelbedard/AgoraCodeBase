using _src.Code.App.Logic.Game;
using _src.Code.App.Managers;
using _src.Code.App.Services;
using _src.Code.Core.Actors;
using _src.Code.Core.Interfaces.Factories;
using _src.Code.Core.Interfaces.GameModules;
using _src.Code.Core.Interfaces.Logic;
using _src.Code.Core.Interfaces.Services;
using _src.Code.Core.Signals.Game;
using _src.Code.Game;
using _src.Code.Game.Commands;
using _src.Code.Game.Commands.GameActions;
using _src.Code.Game.Commands.Other;
using _src.Code.Game.Factories;
using _src.Code.Game.Modules.Card;
using _src.Code.Game.Modules.Deck;
using _src.Code.Game.Modules.Zone;
using Agora.Core.Dtos.Game.GameModules;
using UnityEngine;
using Zenject;

namespace _src.Code.Installers
{
    public class GameSceneInstaller: MonoInstaller<GameSceneInstaller>
    {
        [Header("Camera Settings")]
        [SerializeField] private CameraManager cameraManager;
        [SerializeField] public Transform cameraRigTransform;
        [SerializeField] public Camera mainCamera;

        [Header("Board Plane Service Settings")] 
        [SerializeField] private Transform boardPlaneTransform; 
        
        [Header("Camera Plane Service Settings")] 
        [SerializeField] private Transform cameraPlaneTransform; 

        [Header("Particle System Settings")]
        [SerializeField] private new ParticleSystem particleSystem;
        
        [Header("Manager Settings")]
        [SerializeField] private GameObject handPrefab;
        [SerializeField] private Transform characterTransform;
        
        [Header("Game Modules")]
        [SerializeField] private GameObject cardPrefab;
        [SerializeField] private GameObject deckPrefab;
        [SerializeField] private GameObject zonePrefab;
        
        public override void InstallBindings()
        {
            // Init
            ServiceLocator.Initialize(Container);
            
            // -----------------------------------------------------------
            // Services
            // -----------------------------------------------------------
            
            Container.Bind<ICameraService>()
                .To<CameraService>()
                .AsSingle()
                .WithArguments(mainCamera, cameraRigTransform)
                .NonLazy();
            
            Container.Bind<IBoardPlaneService>()
                .To<BoardPlaneService>()
                .AsSingle()
                .WithArguments(boardPlaneTransform)
                .NonLazy();
            
            Container.Bind<ICameraPlaneService>()
                .To<CameraPlaneService>()
                .AsSingle()
                .WithArguments(cameraPlaneTransform)
                .NonLazy();
            
            Container.Bind<IParticleSystemService>()
                .To<ParticleSystemService>()
                .AsSingle()
                .WithArguments(particleSystem)
                .NonLazy();
            
            Container.BindInterfacesAndSelfTo<CameraManager>()
                .FromInstance(cameraManager)
                .AsSingle()
                .NonLazy();
            
            // -----------------------------------------------------------
            // Logic
            // -----------------------------------------------------------
            
            // Handler
            Container.Bind<IGameLogic>().To<GameLogic>().AsSingle().NonLazy();
            
            // Managers
            Container.Bind<GameObject>().WithId("HandPrefab").FromInstance(handPrefab);
            Container.BindInterfacesAndSelfTo<HandManager>()
                .FromNewComponentOnNewGameObject()
                .WithGameObjectName("HandManager") 
                .AsSingle()
                .NonLazy();
            
            Container.BindInterfacesAndSelfTo<TurnManager>()
                .FromNewComponentOnNewGameObject()
                .AsSingle()
                .WithArguments(characterTransform) 
                .NonLazy();
            
            Container.BindInterfacesAndSelfTo<DescriptionManager>().AsSingle().NonLazy();
            
            Container.Bind<InputListener>().AsSingle().NonLazy();
            
            // Services
            Container.Bind<IGameModuleService>().To<GameModuleService>().AsSingle();
            
            // Factories
            Container.Bind<ICard>().To<Card>().AsTransient();
            Container.Bind<ICardFactory>().To<CardFactory>().AsTransient();
            Container.BindFactory<CardDto, ICard, PlaceholderCardFactory>().FromFactory<CardFactory>();
            Container.BindInstance(cardPrefab).WhenInjectedInto<CardFactory>();
            
            Container.Bind<IDeck>().To<Deck>().AsTransient();
            Container.Bind<IDeckFactory>().To<DeckFactory>().AsTransient();
            Container.BindFactory<DeckDto, IDeck, PlaceholderDeckFactory>().FromFactory<DeckFactory>();
            Container.BindInstance(deckPrefab).WhenInjectedInto<DeckFactory>();
            
            Container.Bind<IZone>().To<Zone>().AsTransient();
            Container.Bind<IZoneFactory>().To<ZoneFactory>().AsTransient();
            Container.BindFactory<ZoneDto, IZone, PlaceholderZoneFactory>().FromFactory<ZoneFactory>();
            Container.BindInstance(zonePrefab).WhenInjectedInto<ZoneFactory>();
            
            // -----------------------------------------------------------
            // Signals
            // -----------------------------------------------------------
            
            // commands
            Container.Bind<DrawCard>().AsSingle().NonLazy();
            Container.Bind<FlipTopCard>().AsSingle().NonLazy();
            Container.Bind<PlayCard>().AsSingle().NonLazy();
            Container.Bind<PlayCardInsideZone>().AsSingle().NonLazy();
            Container.Bind<TransferToken>().AsSingle().NonLazy();
            
            // other
            Container.DeclareSignal<GameActionSignal>();
            Container.DeclareSignal<GameAnimationSignal>();
            Container.DeclareSignal<GameInputSignal>();
            
            Container.Bind<PlayerTurn>().AsSingle().NonLazy();
            Container.DeclareSignal<EndGameSignal>();
        }
    }
}