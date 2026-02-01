using System.Collections.Generic;
using System.Linq;
using _src.Code.Core.Actors;
using _src.Code.Core.Interfaces.GameModules;
using _src.Code.Core.Interfaces.GameModules.Other;
using _src.Code.Core.Interfaces.Services;
using _src.Code.Core.Signals.Inputs;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace _src.Code.Game._other.Hand
{
    public partial class Hand : MonoBehaviour, IHand
    {
        // private fields
        private IClientDataService _clientDataService;
        private ICameraService _cameraService;
        private ICameraPlaneService _cameraPlaneService;
        private HandSlotPositions _handSlotPositions;
        private List<ICard> _cardsInHand;

        private Sequence _currentSequence;
        public ICard CardBeingDrag { get; set; }

        // const
        private const float DefaultTransitionTime = 0.5f;

        // properties
        public string PlayerId { get; set; }
        public string Description { get; set; }

        /// <summary>
        /// Awake
        /// </summary>
        public void Awake()
        {
            _clientDataService = ServiceLocator.GetService<IClientDataService>();
            _cameraService = ServiceLocator.GetService<ICameraService>();
            _cameraPlaneService = ServiceLocator.GetService<ICameraPlaneService>();

            var signalBus = ServiceLocator.GetService<SignalBus>();
            signalBus.Subscribe<DragStartSignal>(HandleDragStart);
            signalBus.Subscribe<DragUpdateSignal>(HandleDragUpdate);
            
            _handSlotPositions = GetComponent<HandSlotPositions>();
            _cardsInHand = new List<ICard>();
        }

        public void SetGameObjectName(string gameObjectName)
        {
            gameObject.name = gameObjectName;
        }

        /// <summary>
        /// Add a card to hand
        /// </summary>
        public void AddCard(ICard card, int index)
        {
            if (_cardsInHand.Contains(card))
            {
                Debug.LogWarning("Card is being add to hand, but is already there");

                if (card == CardBeingDrag)
                {
                    var indexOfCardBeingHovered = GetIndexOfCardBeingHovered();
                    if (indexOfCardBeingHovered > -1)
                    {
                        Debug.LogWarning("from " + index + " to " + GetIndexOfCardBeingHovered());
                        index = indexOfCardBeingHovered;
                    }
                    
                    CardBeingDrag = null;
                }
                
                // Ensure the card is moved to the correct position
                _cardsInHand.Remove(card);
                _cardsInHand.Insert(index, card);

                return;
            }
            
            card.Hand = this;
            _cardsInHand.Insert(index, card);
            _handSlotPositions.AddSlot();
        }

        /// <summary>
        /// Remove a card from hand
        /// </summary>
        public int RemoveCard(ICard card)
        {
            var index = _cardsInHand.IndexOf(card);
            _cardsInHand.Remove(card);
            card.Hand = null;
            
            _handSlotPositions.RemoveSlot();
            return index;
        }

        public bool Contains(ICard card)
        {
            return _cardsInHand.Contains(card);
        }

        /// <summary>
        /// UpdateCardPositions
        /// </summary>
        public Sequence UpdateCardPositions(float transitionTime = DefaultTransitionTime)
        {
            // _currentSequence?.Kill();
            
            // check for index to skip
            var indexOfCardBeingHovered = -1;
            if (CardBeingDrag != null)
            {
                indexOfCardBeingHovered = GetIndexOfCardBeingHovered();
            }
            
            // default indexToSkip to -1
            var indexToSkip = -1;
            
            // update slots and indexToSkip based on dragging and hovering
            if (CardBeingDrag != null)
            {
                // dragging
                if (indexOfCardBeingHovered > -1)
                {
                    // dragging + hovering
                    _handSlotPositions.UpdateSlotPositions();
                    indexToSkip = indexOfCardBeingHovered;
                }
                else
                {
                    // dragging + not hovering
                    _handSlotPositions.UpdateSlotPositions(true);
                }
            }
            else
            {
                // not dragging
                _handSlotPositions.UpdateSlotPositions();
            }
            
            var indexInHand = 0;
            _currentSequence = DOTween.Sequence();
            foreach (var card in _cardsInHand)
            {
                // increase indexInHand by 1 if should skip
                if (_cardsInHand.IndexOf(card) == indexToSkip)
                {
                    indexInHand += 1;
                }
                
                // continue if card being drag
                if (CardBeingDrag != null && CardBeingDrag == card)
                {
                    continue;
                }

                var cardTransform = card.Transform;
                var targetTransform = _handSlotPositions.SlotTransforms[indexInHand];
                
                card.SetLayer("CardsInHand_" + indexInHand);
                _cameraPlaneService.AddChild(cardTransform);
                
                _currentSequence.Join(cardTransform.DOLocalMove(targetTransform.localPosition, transitionTime));
                _currentSequence.Join(cardTransform.DORotateQuaternion(Quaternion.Euler(targetTransform.eulerAngles), transitionTime));
                _currentSequence.Join(cardTransform.DOScale(targetTransform.localScale, transitionTime));
                
                _currentSequence.OnUpdate(() =>
                {
                    if (card.InputObject.IsBeingDrag)
                    {
                        // Kill the sequence if the card is being dragged
                        _currentSequence.Kill();
                        Debug.Log("Animation canceled because the card is being dragged.");
                    }
                });
                
                indexInHand++;
            }

            return _currentSequence; // Return the sequence so the caller can control it
        }

        private int GetIndexOfCardBeingHovered()
        {
            var objectAtMousePositionList = _cameraService.GetObjectsAtMousePosition();

            if (CardBeingDrag != null)
            {
                // remove current card being drag collider from list
                if (objectAtMousePositionList.Contains(CardBeingDrag.InputObject))
                {
                    objectAtMousePositionList.Remove(CardBeingDrag.InputObject);
                }
            }

            var colliderOfCardInHandBeingHovered = objectAtMousePositionList
                .FirstOrDefault(c => _cardsInHand.Exists(i => i.InputObject == c));
            
            if (colliderOfCardInHandBeingHovered != null)
            {
                return _cardsInHand.Select(c => c.InputObject).ToList().IndexOf(colliderOfCardInHandBeingHovered);
            }

            return -1;
        }
    }
}