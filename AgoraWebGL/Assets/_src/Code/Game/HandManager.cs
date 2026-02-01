using _src.Code.Core.Interfaces.GameModules.Other;
using _src.Code.Core.Interfaces.Managers;
using _src.Code.Game._other.Hand;
using UnityEngine;
using Zenject;

// Your namespace

namespace _src.Code.Game
{
    public class HandManager : MonoBehaviour, IHandManager
    {
        [SerializeField] private Hand _handPrefab;
        [Inject] private DiContainer _container;
    
        [Inject]
        public void Construct(DiContainer container, [Inject(Id = "HandPrefab")] GameObject handPrefab)
        {
            _container = container;
            _handPrefab = handPrefab.GetComponent<Hand>();
        }

        public IHand CreateHand(string playerId)
        {
            // Instantiate the prefab using Zenject so dependencies are injected
            var hand = _container.InstantiatePrefabForComponent<IHand>(_handPrefab, transform);
        
            hand.PlayerId = playerId;
            hand.SetGameObjectName($"Hand_{playerId}");
        
            return hand;
        }
    }
}