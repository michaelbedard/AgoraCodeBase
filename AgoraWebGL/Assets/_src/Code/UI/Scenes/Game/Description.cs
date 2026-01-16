using _src.Code.Core.Actors;
using _src.Code.Core.Interfaces.UI;
using UnityEngine.UIElements;

namespace _src.Code.UI.Scenes.Game
{
    public class Description : CustomVisualElement, IVisualElement
    {
        public new class UxmlFactory : UxmlFactory<Description, UxmlTraits> { }
        public string Path => "Assets/_src/UI/Popups/DescriptionPopup.uxml";

        public string Title;
        public string Content;
        
        protected override void InitializeCore()
        {
            // Title = Get<Title>();
            // Content = Get<Label>("Description");
            //
            // Title.Label.text = "WarningPopup";
            // Content.text = "Message";
        }
    }
}