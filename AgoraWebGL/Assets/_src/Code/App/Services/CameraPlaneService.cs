using _src.Code.Core.Interfaces.Services;
using _src.Code.Core.Utility.Plane;
using UnityEngine;
using Zenject;

namespace _src.Code.App.Services
{
    public class CameraPlaneService : PlanePositionController, ICameraPlaneService
    {
        [Inject]
        public CameraPlaneService(Transform cameraPlaneTransform) : base(cameraPlaneTransform)
        {
        }

        public float GetDistanceFromClippingPlane()
        {
            return PlaneTransform.localPosition.z;
        }

        public void PositionElement(Transform element, Vector2 position, float normalRotation = 0, float tangentialRotation = 0f, float offset = 0f)
        {
            PositionElement(element, position, normalRotation, tangentialRotation, offset, offsetShouldBeInlineWithCamera: true,
                perpendicularToPlane: false, smoothDamp: false);
            element.SetParent(PlaneTransform.transform);
        }
        
        public void PositionElementAtCursor(Transform element, float normalRotation = 0, float tangentialRotation = 0f, float offset = 0f)
        {
            PositionElementAtCursor(element, normalRotation, tangentialRotation, offset, perpendicularToPlane: false);
            element.SetParent(PlaneTransform.transform);
        }

        public void AddChild(Transform child)
        {
            child.SetParent(PlaneTransform.transform);
        }
        
        public Vector2 GetTransformPlaneCoordinate(Vector3 worldPosition)
        {
            return GetTransformPlaneCoordinates(worldPosition);
        }
        
        public Vector3 TransformPoint(Vector2 planeCoordinates)
        {
            return PlaneTransform.TransformPoint(new Vector3(planeCoordinates.x, 0, planeCoordinates.y));
        }
    }
}