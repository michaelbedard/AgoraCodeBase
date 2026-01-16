using _src.Code.Core.Interfaces.GameModules;
using UnityEngine;

namespace _src.Code.Game.Modules._other.Hand
{
    public partial class Hand
    {
        public void Position(float firstAngle, float secondAngle, float elevation, float radius, float angleIncrement)
        {
            _handSlotPositions.firstAngle = firstAngle;
            _handSlotPositions.secondAngle = secondAngle;
            _handSlotPositions.elevation = elevation;
            _handSlotPositions.radius = radius;
            _handSlotPositions.angleIncrement = angleIncrement;
        }

        public void SetIsFaceUp(bool isFaceUp)
        {
            _handSlotPositions.isFaceUp = isFaceUp;
            UpdateCardPositions();
        }
        
        public void ShowMoreCardsToTheRight()
        {
            _handSlotPositions.MoveSlotsToTheRight();
            UpdateCardPositions();
        }
        
        public void ShowMoreCardsToTheLeft()
        {
            _handSlotPositions.MoveSlotsToTheLeft();
            UpdateCardPositions();
        }
        
        public Transform GetTransformInFront(ICard card, float distance, bool faceUp, bool straight)
        {
            var cardIndex = _cardsInHand.IndexOf(card);
            return _handSlotPositions.GetTransformInFront(cardIndex, distance, faceUp, straight);
        }
        
        public Vector2 GetCenterOfRotation()
        {
            return _handSlotPositions.GetCenterOfRotation();
        }
    }
}