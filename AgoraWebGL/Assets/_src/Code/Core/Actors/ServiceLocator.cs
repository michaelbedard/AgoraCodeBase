using UnityEngine;
using Zenject;

namespace _src.Code.Core.Actors
{
    public static class ServiceLocator
    {
        private static DiContainer _container;

        public static void Initialize(DiContainer container)
        {
            _container = container;
        }

        public static T GetService<T>()
        {
            if (!Application.isPlaying || _container == null)
            {
                Debug.LogWarning("Service container is unavailable.");
                return default;
            }
            
            return _container.Resolve<T>();
        }
    }
}