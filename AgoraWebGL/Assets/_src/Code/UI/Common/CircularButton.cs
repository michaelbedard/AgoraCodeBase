using System;
using System.Threading.Tasks;
using _src.Code.Core.Actors;
using DG.Tweening;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UIElements;

namespace _src.Code.UI.Common
{
    public class CircularButton : CustomButton
    {
        public new class UxmlFactory : UxmlFactory<CircularButton, UxmlTraits> { }
        

        private string _iconAddress;
        public string IconAddress
        {
            set
            {
                _iconAddress = value;
                _ = SetIconAsync(value); // Fire-and-forget the async operation
            }
        }

        private VisualElement _iconContainer;
        
        protected override void InitializeCore()
        {
            base.InitializeCore();
            
            _iconContainer = Get("Icon");
            
            // hide label
            Label.style.opacity = 0;
            
            // Mouse enter: show focused background
            Button.RegisterCallback<MouseEnterEvent>(async evt =>
            {
                FadeInText();
            });
            
            // Mouse leave: show normal background
            Button.RegisterCallback<MouseLeaveEvent>(async evt =>
            {
                FadeOutText();
            });
        }
        
        /// <summary>
        /// SetIconAsync
        /// </summary>
        private async Task SetIconAsync(string address)
        {
            try
            {
                var sprite = await Addressables.LoadAssetAsync<Sprite>(address).Task;
                _iconContainer.style.backgroundImage = new StyleBackground(sprite);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to load icon from address '{address}': {ex.Message}");
            }
        }

        /// <summary>
        /// ShowAdditionalText
        /// </summary>
        private void FadeInText()
        {
            DOTween.To(() => Label.resolvedStyle.opacity,
                x => Label.style.opacity = x,
                1f, // Fully visible
                0.2f).SetEase(Ease.OutQuad);
            
            DOTween.To(() => _iconContainer.resolvedStyle.opacity,
                x => _iconContainer.style.opacity = x,
                0f, // Fully transparent
                0.2f).SetEase(Ease.OutQuad);
        }
        
        /// <summary>
        /// HideAdditionalText
        /// </summary>
        private void FadeOutText()
        {
            DOTween.To(() => Label.resolvedStyle.opacity,
                x => Label.style.opacity = x,
                0f, // Fully transparent
                0.2f).SetEase(Ease.OutQuad);
            
            DOTween.To(() => _iconContainer.resolvedStyle.opacity,
                x => _iconContainer.style.opacity = x,
                1f, // Fully visible
                0.2f).SetEase(Ease.OutQuad);
        }
    }
}