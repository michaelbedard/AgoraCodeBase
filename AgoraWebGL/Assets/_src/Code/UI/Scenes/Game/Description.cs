using _src.Code.Core.Actors;
using _src.Code.Core.Interfaces.UI;
using _src.Code.UI.Common;
using UnityEngine.UIElements;

namespace _src.Code.UI.Scenes.Game
{
    public class Description : CustomVisualElement, IVisualElement
    {
        public new class UxmlFactory : UxmlFactory<Description, UxmlTraits> { }

        private Label _title;
        private Label _content;
        
        public VisualElement Container { get; private set; } 
        
        protected override void InitializeCore()
        {
            Container = Get("Container"); 
            
            _title = Get<Label>("Title");
            _content = Get<Label>("Content");
        }

        public void Setup(string title, string content)
        {
            _title.text = title;
            _content.text = content;
        }
    }
}