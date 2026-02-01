using _src.Code.App.Managers;
using UnityEngine;
using Zenject;

namespace _src.Code.Installers
{
    public class AdditiveSceneInstaller : MonoInstaller<AdditiveSceneInstaller>
    {
        [SerializeField] private FadeManager fadeManager;
        
        public override void InstallBindings()
        {
            // NOTE : appLogic looks for AdditiveScreen inside the UiDocument, which requires it to be inside a screen, not project
            // Container.BindInterfacesAndSelfTo<AppLogic>().AsSingle().NonLazy();
            
            Container.Bind<FadeManager>().FromInstance(fadeManager).AsSingle();
        }
    }
}