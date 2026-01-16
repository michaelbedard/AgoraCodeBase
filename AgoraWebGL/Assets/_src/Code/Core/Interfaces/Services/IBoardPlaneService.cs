using _src.Code.Core.Interfaces.Services.Other;
using UnityEngine;

namespace _src.Code.Core.Interfaces.Services
{
    public interface IBoardPlaneService : IPlaneService
    {
        void AddCommonElement(Transform element, Vector2 position, float normalRotation = 0, float tangentialRotation = 0, float offset = 0);

        void AddPlayerElement(Transform element, Vector2 position, int numberOfPlayers, int seat,
            float normalRotation = 0, float offset = 0, float radiusOffset = 0f);

        void PositionElementAtCursor(Transform element, float normalRotation = 0, float tangentialRotation = 0f,
            float offset = 0f);

        Vector2? GetPositionAtCursor();
        Vector3? GetPositionAtCursorWorldSpace();

        
    }
}