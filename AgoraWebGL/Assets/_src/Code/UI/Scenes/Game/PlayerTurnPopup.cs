using _src.Code.Core.Actors;
using _src.Code.Core.Interfaces.UI;
using DG.Tweening;
using UnityEngine.UIElements;

namespace _src.Code.UI.Scenes.Game
{
    public class PlayerTurnPopup: CustomVisualElement, IVisualElement
    {
        public new class UxmlFactory : UxmlFactory<PlayerTurnPopup, UxmlTraits> { }
        public string Path => "Assets/_src/UI/Popups/PlayerTurnPopup.uxml";

        public string Message {
            get => _message.text;
            set => _message.text = value;
        }

        private VisualElement _content;
        private VisualElement _background;
        private Label _message;
        
        private const float RotationSpeed = 8f;
        
        protected override void InitializeCore()
        {
            _message = Get<Label>("Message");
            _background = Get("Background");

            _message.text = "It's Your Turn!";
            
            RotateBackground();
        }
        
        private void RotateBackground()
        {
            // Animate the rotation indefinitely
            DOTween.To(
                    () => _background.style.rotate.value.angle.value, // Getter for current angle
                    angle => _background.style.rotate = new Rotate(new Angle(angle)), // Setter for angle
                    360f, // Target angle
                    RotationSpeed // Duration of one full rotation
                )
                .SetEase(Ease.Linear) // Smooth linear rotation
                .SetLoops(-1, LoopType.Restart) // Infinite loop
                .Play();
        }
    }
}