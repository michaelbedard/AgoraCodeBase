using _src.Code.Core.Actors;
using UnityEngine.UIElements;

namespace _src.Code.UI.Common
{
    public class Title : CustomVisualElement
    {
        public new class UxmlFactory : UxmlFactory<Title, UxmlTraits> { }

        public Label Label { get; private set; }

        protected override void InitializeCore()
        {
            Label = this.Q<Label>();
        }

        public void SetText(string text)
        {
            if (Label != null)
            {
                Label.text = text;
            }
        }
    }
}