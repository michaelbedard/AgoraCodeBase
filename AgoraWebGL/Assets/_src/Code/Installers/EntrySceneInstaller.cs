using _src.Code.App.Logic.Entry;
using _src.Code.Core.Actors;
using _src.Code.Core.Signals.Other;
using UnityEngine;
using Zenject;

namespace _src.Code.Installers
{
    public class EntrySceneInstaller : MonoInstaller<EntrySceneInstaller>
    {
        [Header("Avatar Settings")]
        [SerializeField] private Transform avatarTransform;
        
        public override void InstallBindings()
        {
            // Init
            ServiceLocator.Initialize(Container);
            
            // -----------------------------------------------------------
            // Logic
            // -----------------------------------------------------------
            
            Container.BindInterfacesAndSelfTo<EntryLogic>().AsSingle().WithArguments(avatarTransform).NonLazy();
            
            // -----------------------------------------------------------
            // Signals
            // -----------------------------------------------------------

            Container.DeclareSignal<EstablishingServerCommunicationSignal>();
        }
    }
}