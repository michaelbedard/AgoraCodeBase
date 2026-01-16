using _src.Code.App.Logic.Entry;
using _src.Code.Core.Actors;
using _src.Code.Core.Signals.Other;
using Zenject;

namespace _src.Scenes.Installers
{
    public class EntrySceneInstaller : MonoInstaller<EntrySceneInstaller>
    {
        public override void InstallBindings()
        {
            // Init
            ServiceLocator.Initialize(Container);
            
            // -----------------------------------------------------------
            // Logic
            // -----------------------------------------------------------
            
            Container.BindInterfacesAndSelfTo<EntryLogic>().AsSingle().NonLazy();
            
            // -----------------------------------------------------------
            // Signals
            // -----------------------------------------------------------

            Container.DeclareSignal<EstablishingServerCommunicationSignal>();
        }
    }
}