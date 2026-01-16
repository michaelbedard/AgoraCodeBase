using _src.Code.Core.Actors;
using _src.Code.Core.Interfaces.UI;
using _src.Code.UI.Common;
using UnityEngine.UIElements;

namespace _src.Code.UI.Shared
{
    public class YesNoPopup : CustomVisualElement, IVisualElement
    {
        public new class UxmlFactory : UxmlFactory<YesNoPopup, UxmlTraits> { }

        public Title Title;
        public Label Message;
        public SecondaryButton YesButton;
        public SecondaryButton NoButton;
        
        protected override void InitializeCore()
        {
            Title = Get<Title>();
            Message = Get<Label>("Message");
            YesButton = Get<SecondaryButton>("YesButton");
            NoButton = Get<SecondaryButton>("NoButton");
            
            Title.Label.text = "YesNoPopup";
            Message.text = "Message";
            YesButton.Text = "No";
            NoButton.Text = "Yes";
            
            YesButton.Clicked += Hide;
            NoButton.Clicked += Hide;
        }
    }
}