using UnityEngine;
using UnityEngine.InputSystem;

namespace _src.Code.Core.Utility.Plane
{
    public class PlanePositionController
    {
        // fields
        protected readonly Transform PlaneTransform;
        private Vector3 _velocity = Vector3.zero;
        private Vector3 _scaleVelocity = Vector3.zero;
        private readonly Camera _mainCamera;

        // const
        private const float MouseDragSpeed = 0.1f;

        protected PlanePositionController(Transform planeTransform)
        {
            PlaneTransform = planeTransform;
            _mainCamera = Camera.main;
        }
        
        protected void PositionElement(Transform elementTransform, Vector2 positionOnPlane, float normalRotation = 0,
            float tangentialRotation = 0f, float offset = 0, bool offsetShouldBeInlineWithCamera = false,
            bool perpendicularToPlane = true, bool smoothDamp = true)
        {
            // Variables
            var planeCenter = PlaneTransform.position;
            var planeRotation = PlaneTransform.rotation;

            // Define world position
            Vector3 worldPosition;
            if (offsetShouldBeInlineWithCamera && _mainCamera != null)
            {
                // Camera to target point on the initial plane
                Vector3 cameraPosition = _mainCamera.transform.position;
                Vector3 targetOnPlane = planeCenter + planeRotation * new Vector3(positionOnPlane.x, 0, positionOnPlane.y);
                Vector3 cameraToTargetDirection = (targetOnPlane - cameraPosition).normalized;

                // Define the parallel plane at distance 'offset'
                Vector3 planeNormal = planeRotation * Vector3.up; // Plane's normal
                Vector3 parallelPlaneCenter = planeCenter + planeNormal * offset;

                // Calculate intersection of the camera's line of sight with the parallel plane
                float denominator = Vector3.Dot(cameraToTargetDirection, planeNormal);
                if (Mathf.Abs(denominator) > Mathf.Epsilon)
                {
                    float t = Vector3.Dot(parallelPlaneCenter - cameraPosition, planeNormal) / denominator;
                    worldPosition = cameraPosition + t * cameraToTargetDirection;
                }
                else
                {
                    // Fallback for parallel case
                    worldPosition = targetOnPlane + planeNormal * offset;
                }
            }
            else
            {
                // Directly apply offset perpendicular to the plane
                Vector3 localPosition = new Vector3(positionOnPlane.x, offset, positionOnPlane.y);
                worldPosition = planeCenter + planeRotation * localPosition;
            }

            // Apply transformations
            ApplyTransformations(elementTransform, worldPosition, normalRotation, tangentialRotation, 
                perpendicularToPlane, smoothDamp);
        }

        
        protected void PositionElementAtCursor(Transform elementTransform, float normalRotation = 0,
            float tangentialRotation = 0f, float offset = 0, bool perpendicularToPlane = true, bool smoothDamp = true)
        {
            var position = GetPositionAtCursor();

            if (position.HasValue)
            {
                PositionElement(
                    elementTransform,
                    position.Value,
                    normalRotation,
                    tangentialRotation,
                    offset,
                    false,
                    perpendicularToPlane,
                    smoothDamp
                );
            }
        }
        
        protected Vector2? GetPositionAtCursor()
        {
            var ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            var plane = new UnityEngine.Plane(PlaneTransform.up, PlaneTransform.position);

            // Calculate the point where the ray intersects with the plane
            if (plane.Raycast(ray, out var enter))
            {
                var hitPoint = ray.GetPoint(enter);

                var localHitPoint = PlaneTransform.InverseTransformPoint(hitPoint);
                return new Vector2(localHitPoint.x, localHitPoint.z);
            }

            return null;
        }
        
        protected Vector3? GetPositionAtCursorWorldSpace()
        {
            var ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            var plane = new UnityEngine.Plane(PlaneTransform.up, PlaneTransform.position);

            if (plane.Raycast(ray, out var enter))
            {
                return ray.GetPoint(enter);
            }

            return null;
        }
        
        private void ApplyTransformations(Transform elementTransform, Vector3 targetPosition, float normalRotation, 
            float tangentialRotation, bool perpendicularToPlane, bool smoothDamp)
        {
            var thirdAxisRotation = perpendicularToPlane ? 90f : 0f;
            
            if (smoothDamp)
            {
                // Smoothly move the element to the target position
                elementTransform.position = Vector3.SmoothDamp(elementTransform.position,
                    targetPosition, ref _velocity, MouseDragSpeed);

                // Set the initial orientation to face up along the plane normal
                Quaternion initialRotation = Quaternion.LookRotation(PlaneTransform.up);

                // Apply rotation around the plane normal by 'normalRotation' degrees
                Quaternion aroundNormal = Quaternion.AngleAxis(normalRotation, PlaneTransform.up);

                // Apply rotation around the plane's forward axis
                Vector3 tangentialAxis = PlaneTransform.forward;
                Quaternion aroundPerpendicular = Quaternion.AngleAxis(tangentialRotation, tangentialAxis);

                // Apply rotation around the third axis (e.g., the plane's right axis)
                Vector3 thirdAxis = PlaneTransform.right;
                Quaternion aroundThirdAxis = Quaternion.AngleAxis(thirdAxisRotation, thirdAxis);

                // Combine all rotations: normal, perpendicular, and third axis rotations
                Quaternion targetRotation = aroundNormal * aroundPerpendicular * aroundThirdAxis * initialRotation;

                // Smoothly rotate the element
                elementTransform.rotation = Quaternion.Slerp(elementTransform.rotation, targetRotation, MouseDragSpeed);
            }
            else
            {
                // Directly move the element to the target position
                elementTransform.position = targetPosition;

                // Set the initial orientation to face up along the plane normal
                Quaternion initialRotation = Quaternion.LookRotation(PlaneTransform.up);

                // Apply rotation around the plane normal by 'normalRotation' degrees
                Quaternion aroundNormal = Quaternion.AngleAxis(normalRotation, PlaneTransform.up);

                // Apply rotation around the plane's forward axis
                Vector3 tangentialAxis = PlaneTransform.forward;
                Quaternion aroundTangential = Quaternion.AngleAxis(tangentialRotation, tangentialAxis);
                
                // Apply rotation around the third axis (e.g., the plane's right axis)
                Vector3 thirdAxis = PlaneTransform.right;
                Quaternion aroundThirdAxis = Quaternion.AngleAxis(thirdAxisRotation, thirdAxis);

                // Combine rotations to ensure the perpendicular rotation follows the normal rotation
                elementTransform.rotation = aroundNormal * aroundTangential * aroundThirdAxis * initialRotation;
            }
        }
        
        protected Vector2 GetTransformPlaneCoordinates(Vector3 worldPosition)
        {
            var projectedPoint = Vector3.ProjectOnPlane(worldPosition - PlaneTransform.position, PlaneTransform.up) + PlaneTransform.position;
            var localPosition = Quaternion.Inverse(PlaneTransform.rotation) * (projectedPoint - PlaneTransform.position);

            // Return the 2D plane coordinates
            return new Vector2(localPosition.x, localPosition.z);
        }

    }
}