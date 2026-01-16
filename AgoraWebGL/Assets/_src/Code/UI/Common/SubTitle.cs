using _src.Code.Core.Actors;
using _src.Code.Core.Interfaces.UI;
using UnityEngine.UIElements;

namespace _src.Code.UI.Common
{
    public class SubTitle : CustomVisualElement, IVisualElement
    {
        public new class UxmlFactory : UxmlFactory<SubTitle, UxmlTraits> { }
        public string Path => "Assets/_src/UI/Common/SubTitle.uxml";

        public Label Label { get; set; }

        protected override void InitializeCore()
        {
            Label = this.Q<Label>();
        }
    }
}