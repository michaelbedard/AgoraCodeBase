using DG.Tweening;
using UnityEngine;

namespace _src.Code.Core.Interfaces.GameModules.Other
{
    public interface IHand
    {
        // Properties
        string PlayerId { get; set; }
        string Description { get; set; }
        ICard CardBeingDrag { get; set; }

        // Methods
        void SetGameObjectName(string gameObjectName);
        void AddCard(ICard card, int index);
        int RemoveCard(ICard card);
        bool Contains(ICard card);
        Sequence UpdateCardPositions(float transitionTime = 0.5f);
        void Position(float firstAngle, float secondAngle, float elevation, float radius, float angleIncrement);
        void SetIsFaceUp(bool isFaceUp);
        void ShowMoreCardsToTheRight();
        void ShowMoreCardsToTheLeft();
        Transform GetTransformInFront(ICard card, float distance, bool faceUp, bool straight);
        Vector2 GetCenterOfRotation();
    }
}