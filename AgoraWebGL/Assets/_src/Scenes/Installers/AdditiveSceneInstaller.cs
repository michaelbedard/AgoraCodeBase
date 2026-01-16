using Zenject;

namespace _src.Scenes.Installers
{
    public class AdditiveSceneInstaller : MonoInstaller<AdditiveSceneInstaller>
    {
        public override void InstallBindings()
        {
            // NOTE : appLogic looks for AdditiveScreen inside the UiDocument, which requires it to be inside a screen, not project
            // Container.BindInterfacesAndSelfTo<AppLogic>().AsSingle().NonLazy();
        }
    }
}