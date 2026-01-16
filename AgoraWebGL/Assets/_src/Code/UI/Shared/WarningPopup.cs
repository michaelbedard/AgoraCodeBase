using _src.Code.Core.Actors;
using _src.Code.Core.Interfaces.UI;
using _src.Code.UI.Common;
using UnityEngine.UIElements;

namespace _src.Code.UI.Shared
{
    public class WarningPopup : CustomVisualElement, IVisualElement
    {
        public new class UxmlFactory : UxmlFactory<WarningPopup, UxmlTraits> { }

        public Title Title;
        public Label Message;
        public SecondaryButton Button;
        
        protected override void InitializeCore()
        {
            Title = Get<Title>();
            Message = Get<Label>("Message");
            Button = Get<SecondaryButton>();
            
            // Title.Label.text = "WarningPopup";
            // Message.text = "Message";
            // Button.Text = "Ok";
            
            Button.Clicked += Hide;
        }
        
        
    }
}