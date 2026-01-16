using Zenject;

namespace _src.Code.Core.Utility
{
    public class SceneReadyBroadcaster : IInitializable
    {
        private readonly SignalBus _signalBus;

        public SceneReadyBroadcaster(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        public void Initialize()
        {
        }
    }
}