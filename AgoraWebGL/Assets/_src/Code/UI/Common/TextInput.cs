using System;
using _src.Code.Core;
using _src.Code.Core.Actors;
using _src.Code.Core.Interfaces.UI;
using _src.Code.Core.Utility;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

namespace _src.Code.UI.Common
{
    [RequiredSprite("TextInput_Normal")]
    [RequiredSprite("TextInput_Focused")]
    public class TextInput : CustomVisualElement, IVisualElement
    {
        public new class UxmlFactory : UxmlFactory<TextInput, UxmlTraits> { }
        public TextField TextField => _textField;
        
        public string Label {
            get => _textField.label;
            set => _textField.label = value;
        }
        
        public string Value {
            get => _textField.value;
            set => _textField.value = value;
        }
        
        public bool IsPassword
        {
            get => _textField.isPasswordField;
            set => _textField.isPasswordField = value;
        }

        private TextField _textField;
        private VisualElement _textInput;

        private VisualElement _focusedBackgroundLayer;
        private bool _isFocused = false;
        
        protected override async void InitializeCore()
        {
            var focusedSprite = VisualElementService.GetSprite<TextInput>("TextInput_Normal");
            var normalSprite = VisualElementService.GetSprite<TextInput>("TextInput_Focused");
            
            _textField = Get<TextField>();
            _textInput = Get<VisualElement>("unity-text-input");
            
            // Create overlay for focused background (initially hidden)b
            _focusedBackgroundLayer = new VisualElement
            {
                style =
                {
                    position = Position.Absolute,
                    top = 0,
                    left = 0,
                    right = 0,
                    bottom = 0,
                    opacity = 0f, // Initially invisible
                    display = DisplayStyle.Flex,
                }
            };
            
            // Add it on top of the button
            _textInput.Insert(0, _focusedBackgroundLayer);
            
            // Preload focused background without displaying it
            try
            {
                // await LoadAndSetBackground(_focusedImage, isFocusedLayer: true);
                // await LoadAndSetBackground(_normalImage, isFocusedLayer: false); // after
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
            
            // callbacks
            _textField.RegisterCallback<MouseEnterEvent>(async evt =>
            {
                Cursor.SetCursor(Globals.Instance.CursorTextureInput, Vector2.zero, CursorMode.Auto);
                
                FadeInFocusedBackground();
                AnimateScale(new Vector3(1.02f, 1.02f, 1f));
            });
            
            _textField.RegisterCallback<MouseLeaveEvent>(async evt => 
            {
                if (!_isFocused)
                {
                    FadeOutFocusedBackground();
                    AnimateScale(Vector3.one);
                }
                
                Cursor.SetCursor(Globals.Instance.CursorTextureDefault, Vector2.zero, CursorMode.Auto);
            });
            
            _textField.RegisterCallback<FocusEvent>(async evt =>
            {
                FadeInFocusedBackground();
                AnimateScale(new Vector3(1.02f, 1.02f, 1f));
                
                _isFocused = true;
            });

            _textField.RegisterCallback<BlurEvent>(async evt =>
            {
                FadeOutFocusedBackground();
                AnimateScale(Vector3.one);
                
                _isFocused = false;
            });
        }
        
        /// <summary>
        /// Loads and assigns background images.
        /// </summary>
        private async System.Threading.Tasks.Task LoadAndSetBackground(string backgroundAddress, bool isFocusedLayer)
        {
            try
            {
                var sprite = await Addressables.LoadAssetAsync<Sprite>(backgroundAddress).Task;
                var bgStyle = new StyleBackground(sprite);

                if (isFocusedLayer)
                    _focusedBackgroundLayer.style.backgroundImage = bgStyle;
                else
                    _textInput.style.backgroundImage = bgStyle; // Normal background
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
        
        /// <summary>
        /// Smoothly fades in the focused background.
        /// </summary>
        private void FadeInFocusedBackground()
        {
            DOTween.To(() => _focusedBackgroundLayer.resolvedStyle.opacity,
                x => _focusedBackgroundLayer.style.opacity = x,
                1f, // Fully visible
                0.2f).SetEase(Ease.OutQuad);
        }

        /// <summary>
        /// Smoothly fades out the focused background.
        /// </summary>
        private void FadeOutFocusedBackground()
        {
            DOTween.To(() => _focusedBackgroundLayer.resolvedStyle.opacity,
                x => _focusedBackgroundLayer.style.opacity = x,
                0f, // Fully transparent
                0.2f).SetEase(Ease.OutQuad);
        }

        /// <summary>
        /// Animates the scaling effect on hover.
        /// </summary>
        private void AnimateScale(Vector3 targetScale)
        {
            DOTween.To(() => _textInput.style.scale.value.value,
                x => _textInput.style.scale = new Scale(x),
                targetScale,
                0.2f).SetEase(Ease.OutQuad);
        }
    }
}