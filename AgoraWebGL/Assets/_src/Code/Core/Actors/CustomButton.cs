using System;
using _src.Code.Core.Interfaces.UI;
using _src.Code.Core.Utility;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

namespace _src.Code.Core.Actors
{
    // These attributes ensure 'Normal_Background' and 'Focused_Background' slots 
    // appear in UIConfig for any class that inherits from CustomButton.
    [RequiredSprite(KeyNormal)]
    [RequiredSprite(KeyFocused)]
    public abstract class CustomButton : CustomVisualElement, IVisualElement
    {
        // Define keys as constants to avoid typos
        protected const string KeyNormal = "Normal_Background";
        protected const string KeyFocused = "Focused_Background";

        private Button _button;
        private Label _label;
        public Button Button
        {
            get
            {
                if (_button == null) _button = this.Q<Button>();
                return _button;
            }
        }
        
        public Label Label 
        {
            get
            {
                if (_label == null) _label = this.Q<Label>();
                return _label;
            }
        }

        public string Text
        {
            get => Label.text;
            set => Label.text = value; 
        }

        public event Action Clicked;
        private VisualElement _focusedBackgroundLayer;

        protected override void InitializeCore() // Removed 'async' (No longer needed)
        {
            Button.clicked += () => Clicked?.Invoke();

            // Create overlay for focused background (initially hidden)
            _focusedBackgroundLayer = new VisualElement
            {
                style =
                {
                    position = Position.Absolute,
                    top = 0, left = 0, right = 0, bottom = 0,
                    opacity = 0f, 
                    display = DisplayStyle.Flex,
                }
            };

            // Add it on top of the button content
            Button.Insert(0, _focusedBackgroundLayer);

            // ---------------------------------------------------------
            // NEW SYNCHRONOUS LOADING
            // ---------------------------------------------------------
            // We use 'this.GetType().Name' so that if this instance is a 'PrimaryButton',
            // it asks UIConfig for 'PrimaryButton' sprites.
            // (Assumes your Service/Config supports string lookup, see note below)
            var focusedSprite = VisualElementService.GetSprite(GetType().Name, KeyFocused);
            var normalSprite = VisualElementService.GetSprite(GetType().Name, KeyNormal);

            if (focusedSprite != null)
                _focusedBackgroundLayer.style.backgroundImage = new StyleBackground(focusedSprite);
            
            if (normalSprite != null)
                Button.style.backgroundImage = new StyleBackground(normalSprite);

            // ---------------------------------------------------------

            SetupCallbacks();
        }

        private void SetupCallbacks()
        {
            // Mouse enter: show focused background
            Button.RegisterCallback<MouseEnterEvent>(evt =>
            {
                Cursor.SetCursor(Globals.Instance.CursorTextureClickable, Vector2.zero, CursorMode.Auto);
                FadeInFocusedBackground();
                AnimateScale(new Vector3(1.02f, 1.02f, 1f));
            });

            // Mouse leave: show normal background
            Button.RegisterCallback<MouseLeaveEvent>(evt =>
            {
                Cursor.SetCursor(Globals.Instance.CursorTextureDefault, Vector2.zero, CursorMode.Auto);
                FadeOutFocusedBackground();
                AnimateScale(Vector3.one);
            });
        }

        private void FadeInFocusedBackground()
        {
            DOTween.To(() => _focusedBackgroundLayer.resolvedStyle.opacity,
                x => _focusedBackgroundLayer.style.opacity = x,
                1f, 0.2f).SetEase(Ease.OutQuad);
        }

        private void FadeOutFocusedBackground()
        {
            DOTween.To(() => _focusedBackgroundLayer.resolvedStyle.opacity,
                x => _focusedBackgroundLayer.style.opacity = x,
                0f, 0.2f).SetEase(Ease.OutQuad);
        }

        private void AnimateScale(Vector3 targetScale)
        {
            DOTween.To(() => style.scale.value.value,
                x => style.scale = new Scale(x),
                targetScale, 0.2f).SetEase(Ease.OutQuad);
        }
    }
}