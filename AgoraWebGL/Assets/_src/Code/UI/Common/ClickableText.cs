using System;
using System.Threading.Tasks;
using _src.Code.Core;
using _src.Code.Core.Actors;
using _src.Code.Core.Interfaces.UI;
using UnityEngine;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

namespace _src.Code.UI.Common
{
    public class ClickableText : CustomVisualElement, IVisualElement
    {
        public new class UxmlFactory : UxmlFactory<ClickableText, UxmlTraits> { }
        public string Path => "Assets/_src/UI/Common/ClickableText.uxml";
        
        public Button Button { get; set; }

        private Color _initialColor;
        
        protected override void InitializeCore()
        {
            Button = this.Q<Button>();
            
            // _initialColor = Button.style.color.value;
            _initialColor = Color.white;
            
            Button.RegisterCallback<MouseEnterEvent>(async evt => 
            {
                await SetTextColor(Color.gray);
                Cursor.SetCursor(Globals.Instance.CursorTextureClickable, Vector2.zero, CursorMode.Auto);
            });
            
            Button.RegisterCallback<MouseLeaveEvent>(async evt => 
            {
                await SetTextColor(_initialColor);
                Cursor.SetCursor(Globals.Instance.CursorTextureDefault, Vector2.zero, CursorMode.Auto);
            });
        }
        
        private async Task SetTextColor(Color targetColor)
        {
            try
            {
                // Gradually change the text color over time (optional animation)
                float duration = 0.2f;
                float elapsedTime = 0f;
                Color currentColor = Button.resolvedStyle.color;

                while (elapsedTime < duration)
                {
                    elapsedTime += Time.deltaTime;
                    Button.style.color = Color.Lerp(currentColor, targetColor, elapsedTime / duration);
                    await Task.Yield();
                }

                // Ensure the final color is set
                Button.style.color = targetColor;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }
}