using _src.Code.Core.Actors;
using UnityEngine;
using Zenject;

namespace _src.Code.__test__
{
    public class BaseTest : MonoBehaviour
    {
        protected SignalBus SignalBus;

        private void Awake()
        {
            #if !UNITY_EDITOR
                Destroy(this);
                return;
            #endif

            SignalBus = ServiceLocator.GetService<SignalBus>();
        }
    }

}