using UnityEngine;

namespace _src.Code.Core.Utility
{
    public class FaceCamera : MonoBehaviour
    {
        private Transform cameraTransform;

        void Start()
        {
            // Cache the main camera's transform
            cameraTransform = Camera.main.transform;
        }

        void LateUpdate()
        {
            // Make the UI face the camera
            transform.LookAt(transform.position + cameraTransform.rotation * Vector3.forward,
                cameraTransform.rotation * Vector3.up);
        }
    }
}