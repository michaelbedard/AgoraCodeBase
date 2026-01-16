using System.Collections.Generic;
using _src.Code.Core.Actors;
using _src.Code.Core.Interfaces.UI;
using UnityEngine.UIElements;

namespace _src.Code.UI.Common
{
    public class RadioButtons : CustomVisualElement, IVisualElement
    {
        public new class UxmlFactory : UxmlFactory<RadioButtons, UxmlTraits>
        {
        }

        public string Path => "Assets/_src/UI/Common/RadioButtons.uxml";

        public string Label {
            get => _radioButtonGroup.label;
            set => _radioButtonGroup.label = value;
        }
        
        public IEnumerable<string> Choices {
            get => _radioButtonGroup.choices;
            set => _radioButtonGroup.choices = value;
        }
        
        public int Value {
            get => _radioButtonGroup.value;
            set => _radioButtonGroup.value = value;
        }

        private RadioButtonGroup _radioButtonGroup;

        protected override void InitializeCore()
        {
            _radioButtonGroup = Get<RadioButtonGroup>();
        }
        
        // Add a method to register value change callbacks
        public void RegisterValueChangedCallback(EventCallback<ChangeEvent<int>> callback)
        {
            _radioButtonGroup.RegisterValueChangedCallback(callback);
        }
    }
}