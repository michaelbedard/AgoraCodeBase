using System.Threading.Tasks;
using _src.Code.Core.Actors;
using _src.Code.Core.Interfaces.UI;
using _src.Code.UI.Common;
using _src.Code.UI.Common.IconButtons;
using UnityEngine.UIElements;

namespace _src.Code.UI.Scenes.Additive.SettingsWindow
{
    public class SettingsWindow : CustomVisualElement, IVisualElement
    {
        public new class UxmlFactory : UxmlFactory<SettingsWindow, UxmlTraits> { }

        private Title _title;
        private CloseIcon _closeBtn;
        private VisualElement _panelContainer;
        private VisualElement _currentPanel;
        
        protected override void InitializeCore()
        {
            _title = Get<Title>();
            _panelContainer = Get<VisualElement>("PanelContainer");
            _closeBtn = Get<CloseIcon>();
            
            _title.Label.text = "Settings";
            
            // set each buttons
            var profileBtn = Get<SecondaryButton>("ProfileBtn");
            profileBtn.Text = "Profile";
            profileBtn.Clicked += ChangePanel<ProfilePanel>;
            
            var audioBtn = Get<SecondaryButton>("AudioBtn");
            audioBtn.Text = "Audio";
            audioBtn.Clicked += ChangePanel<AudioPanel>;
            
            var rulesBtn = Get<SecondaryButton>("RulesBtn");
            rulesBtn.Text = "Rules";
            rulesBtn.Clicked += ChangePanel<RulesPanel>;

            _closeBtn.Clicked += Hide;

            // default
            if (_currentPanel == null)
            {
                ChangePanel<ProfilePanel>();
            }
        }
        
        private async void ChangePanel<T>() where T : CustomVisualElement, IVisualElement, new()
        {
            var newPanel = await VisualElementService.GetOrCreate<T>();
            
            if (_currentPanel == newPanel) return;
            
            if (_currentPanel != null && _currentPanel.parent == _panelContainer)
            {
                _panelContainer.Remove(_currentPanel);
            }

            _currentPanel = newPanel;
            _panelContainer.Add(_currentPanel);
        }
    }
}