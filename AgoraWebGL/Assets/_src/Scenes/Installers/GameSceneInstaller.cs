using _src.Code.App.Logic.Game;
using _src.Code.App.Managers;
using _src.Code.App.Services;
using _src.Code.Core.Actors;
using _src.Code.Core.Interfaces.Handlers;
using _src.Code.Core.Interfaces.Services;
using _src.Code.Core.Signals.Game;
using UnityEngine;
using Zenject;

namespace _src.Scenes.Installers
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
        [SerializeField] private Transform particleSystemRoot;
        
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
                .To<ICameraPlaneService>()
                .AsSingle()
                .WithArguments(cameraPlaneTransform)
                .NonLazy();
            
            Container.Bind<IParticleSystemService>()
                .To<ParticleSystemService>()
                .AsSingle()
                .WithArguments(particleSystemRoot)
                .NonLazy();
            
            Container.BindInterfacesAndSelfTo<CameraManager>().AsSingle().NonLazy();
            
            // -----------------------------------------------------------
            // Logic
            // -----------------------------------------------------------
            
            // Handler
            Container.Bind<IGameLogic>().To<GameLogic>().AsSingle().NonLazy();
            
            // Managers
            
            // Services
            
            // -----------------------------------------------------------
            // Signals
            // -----------------------------------------------------------
            
            Container.DeclareSignal<EndGameSignal>();
        }
    }
}