using _src.Code.Core.Interfaces.Services;
using _src.Code.Core.Utility.Plane;
using UnityEngine;
using Zenject;

namespace _src.Code.App.Services
{
    public class BoardPlaneService : PlanePositionController, IBoardPlaneService
    {
        [Inject]
        public BoardPlaneService(Transform boardPlaneTransform) : base(boardPlaneTransform)
        {
        }
        
        public void AddCommonElement(Transform element, Vector2 position, float normalRotation = 0,
            float tangentialRotation = 0, float offset = 0f)
        {
            PositionElement(element, position, normalRotation, tangentialRotation, offset, smoothDamp: false);
            element.SetParent(PlaneTransform.transform);
        }

        public void AddPlayerElement(Transform element, Vector2 position, int numberOfPlayers, int seat, float normalRotation = 0,
            float offset = 0, float radiusOffset = 0f)
        {
            // Calculate the angle for the player's position
            var angle = -seat * Mathf.PI * 2 / numberOfPlayers;

            // Rotate the position vector to the correct angle
            var newPosition = RotateVertex(position, angle, 0);

            // Apply the radius offset by adjusting the vector's magnitude
            if (radiusOffset != 0f)
            {
                var direction = newPosition.normalized; // Get the direction
                newPosition += direction * radiusOffset; // Add the radius offset
            }

            // Calculate the rotation angle for the player
            normalRotation = seat * Mathf.PI * 2 / numberOfPlayers * Mathf.Rad2Deg + 180f;

            // Position the element with the calculated parameters
            PositionElement(element, newPosition, normalRotation, 0f, offset, smoothDamp: false);

            // Set the element's parent
            var targetParent = PlaneTransform.transform;
            element.SetParent(targetParent);
        }
        
        public new Vector2? GetPositionAtCursor()
        {
            return base.GetPositionAtCursor();
        }

        public new Vector3? GetPositionAtCursorWorldSpace()
        {
            return base.GetPositionAtCursorWorldSpace();
        }
        
        public void PositionElementAtCursor(Transform element, float normalRotation = 0, float tangentialRotation = 0f, float offset = 0f)
        {
            PositionElementAtCursor(element, normalRotation, tangentialRotation, offset, perpendicularToPlane: true, smoothDamp: false);
            element.SetParent(PlaneTransform.transform);
        }

        public Vector2 GetTransformPlaneCoordinate(Vector3 worldPosition)
        {
            return GetTransformPlaneCoordinates(worldPosition);
        }

        public Vector3 TransformPoint(Vector2 planeCoordinates)
        {
            return PlaneTransform.TransformPoint(new Vector3(planeCoordinates.x, 0, planeCoordinates.y));
        }
        
        
        // PRIVATE
        
        // Rotate a vertex by a given angle
        private Vector2 RotateVertex(Vector2 vertex, float angle, float offset)
        {
            float cos = Mathf.Cos(angle + offset);
            float sin = Mathf.Sin(angle + offset);
            return new Vector2(
                vertex.x * cos - vertex.y * sin,
                vertex.x * sin + vertex.y * cos
            );
        }

        
    }
}