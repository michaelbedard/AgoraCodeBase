using _src.Code.Core.Actors;
using _src.Code.Core.Interfaces.UI;
using UnityEngine;
using UnityEngine.UIElements;

namespace _src.Code.UI.Common
{
    public class LoadingWidget : CustomVisualElement, IVisualElement
    {
        public new class UxmlFactory : UxmlFactory<LoadingWidget, UxmlTraits> { }
        
        private Label _label;
        private VisualElement _spinningImage;
        
        private const float RotationSpeed = 200f;

        public Label Label
        {
            get
            {
                if (_label == null) _label = this.Q<Label>();
                return _label;
            }
        }

        public VisualElement SpinningImage
        {
            get
            {
                if (_spinningImage == null) _spinningImage = this.Q("SpinningImage"); 
                return _spinningImage;
            }
        }

        protected override void InitializeCore()
        {
            Label.text = "Loading ...";
            
            // schedule.Execute(SpinImage).Every(16);
        }
        
        private void SpinImage()
        {
            // Get the current rotation value and increase it by the speed
            var currentRotation = _spinningImage.resolvedStyle.rotate.angle.value;
            var newRotation = currentRotation + RotationSpeed * Time.deltaTime;

            // Apply the new rotation
            _spinningImage.style.rotate = new Rotate(newRotation);
        }
    }
}