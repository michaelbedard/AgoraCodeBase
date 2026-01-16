using System.Linq;
using _src.Code.Core.Actors;
using _src.Code.Core.Interfaces.UI;
using _src.Code.UI.Common;
using UnityEngine.UIElements;

namespace _src.Code.UI.Scenes.Game.GameInputs
{
    public class SelectAmountPanel : CustomVisualElement, IVisualElement
    {
        public new class UxmlFactory : UxmlFactory<SelectAmountPanel, UxmlTraits> { }
        public string Path => "Assets/_src/UI/Panels/Game/SelectAmountPanel.uxml";

        public SubTitle Title;
        public TextInput TextInput;
        public SecondaryButton SecondaryButton;
        
        protected override void InitializeCore()
        {
            Title = Get<SubTitle>();
            TextInput = Get<TextInput>();
            SecondaryButton = Get<SecondaryButton>();

            TextInput.Label = string.Empty;
            SecondaryButton.Text = "Ok";
            
            TextInput.TextField.RegisterValueChangedCallback(evt =>
            {
                TextInput.TextField.value = new string(evt.newValue.Where(char.IsDigit).ToArray());
            });
        }
    }
}