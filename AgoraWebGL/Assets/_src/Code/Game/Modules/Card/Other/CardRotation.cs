using UnityEngine;

namespace _src.Code.Game.Modules.Card.Other
{
    [ExecuteInEditMode]
    public class CardRotation : MonoBehaviour
    {
        public GameObject cardFront;
        public GameObject cardBack;
        [HideInInspector]
        public bool showingBack = false;
    
        private void Awake()
        {
            showingBack = IsFacingBack();
            UpdateCardSide();
        }
    
        void Update()
        {
            var isFacingBack = IsFacingBack();
            
            if (isFacingBack != showingBack)
            {
                // Additional if check to avoid always setting active/inactive
                showingBack = isFacingBack;
                UpdateCardSide();
            }
        }
    
        bool IsFacingBack()
        {
            var cameraPosition = Camera.main.transform.position;
            var cardToCamera = cameraPosition - transform.position;
            var cardForward = transform.forward;
    
            // Calculate dot product to determine if the camera is facing the front or back of the card
            var dotProduct = Vector3.Dot(cardForward, cardToCamera);
    
            return dotProduct > 0; // Adjust the condition based on your card's forward direction
        }
    
        void UpdateCardSide()
        {
            if (showingBack)
            {
                // Show the back side
                cardFront.SetActive(false);
                cardBack.SetActive(true);
            }
            else
            {
                // Show the front side
                cardFront.SetActive(true);
                cardBack.SetActive(false);
            }
        }
    }
}