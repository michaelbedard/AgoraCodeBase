using _src.Code.Core.Actors;
using _src.Code.Core.Interfaces.UI;
using _src.Code.UI.Common;
using UnityEngine.UIElements;

namespace _src.Code.UI.Scenes.Additive.SettingsWindow
{
    public class RulesPanel : CustomVisualElement, IVisualElement
    {
        public new class UxmlFactory : UxmlFactory<RulesPanel, UxmlTraits> { }

        private Label _label;
        
        protected override void InitializeCore()
        {
            _label = Get<Label>();
            
            // set sub-titles
            Get<SubTitle>("RulesSubTitle").Label.text = "Rules";
        }

        public void SetRules(string rules)
        {
            _label.text = rules;
        }
    }
}