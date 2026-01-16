using UnityEngine;

namespace _src.Code.Core.Utility
{
    public class CameraMover : MonoBehaviour
    {
        [Header("Movement Settings")]
        [Tooltip("The speed at which the camera moves.")]
        public float speed = 2f;

        [Tooltip("The distance the camera moves back and forth.")]
        public float distance = 10f;

        [Tooltip("The axis of movement (e.g., X for horizontal, Z for forward/backward).")]
        public Vector3 movementAxis = Vector3.right; // Default to moving along X-axis
        
        private Vector3 startPosition;
        private bool movingForward = true;

        private void Start()
        {
            // Store the starting position
            startPosition = transform.position;
        }

        private void Update()
        {
            // Determine the direction of movement
            float step = speed * Time.deltaTime;
            if (movingForward)
            {
                transform.position += movementAxis * step;
                if (Vector3.Distance(startPosition, transform.position) >= distance)
                {
                    movingForward = false;
                }
            }
            else
            {
                transform.position -= movementAxis * step;
                if (Vector3.Distance(startPosition, transform.position) <= 0.01f)
                {
                    movingForward = true;
                }
            }
        }
    }

}