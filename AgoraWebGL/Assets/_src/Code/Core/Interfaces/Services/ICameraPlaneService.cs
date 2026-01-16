using _src.Code.Core.Interfaces.Services.Other;
using UnityEngine;

namespace _src.Code.Core.Interfaces.Services
{
    public interface ICameraPlaneService : IPlaneService
    {
        public float GetDistanceFromClippingPlane();
        public void PositionElementAtCursor(Transform element, float normalRotation = 0, float tangentialRotation = 0f, float offset = 0f);
        public void PositionElement(Transform element, Vector2 position, float normalRotation = 0, float tangentialRotation = 0f, float offset = 0f);
        public void AddChild(Transform child);
    }
}