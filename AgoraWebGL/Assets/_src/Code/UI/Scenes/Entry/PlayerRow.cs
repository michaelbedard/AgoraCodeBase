using _src.Code.Core.Actors;
using UnityEngine.UIElements;

namespace _src.Code.UI.Scenes.Entry
{
    public class PlayerRow : CustomVisualElement
    {
        public new class UxmlFactory : UxmlFactory<PlayerRow, UxmlTraits> { }

        // fields
        private Label _label;
        
        /// <summary>
        /// Initialize
        /// </summary>
        protected override void InitializeCore()
        {
            _label = Get<Label>();
        }

        public void SetUsername(string username)
        {
            _label.text = username;
        }
    }
}