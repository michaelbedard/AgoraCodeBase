using _src.Code.Core.Actors;
using _src.Code.UI.Common.IconButtons;
using UnityEngine;
using UnityEngine.UIElements;

namespace _src.Code.UI.Scenes.Additive
{
    public class AdditiveScreen : CustomVisualElement
    {
        // factory
        public new class UxmlFactory : UxmlFactory<AdditiveScreen, UxmlTraits> { }
        
        // fields
        private SettingsIcon _settingsBtn;
        private SettingsWindow.SettingsWindow _settingsWindow;
        
        /// <summary>
        /// InitializeCore
        /// </summary>
        protected override void InitializeCore()
        {
            _settingsBtn = Get<SettingsIcon>("SettingsIcon");
            _settingsBtn.Clicked += OnSettingsBtnClick;
        }
        
        /// <summary>
        /// OnSettingsBtnClick
        /// </summary>
        private async void OnSettingsBtnClick()
        {
            Debug.Log("OnSettingsBtnClick");
            if (!VisualElementService.DocumentContains(_settingsWindow))
            {
                _settingsWindow = await VisualElementService.GetOrCreate<SettingsWindow.SettingsWindow>();
                _settingsWindow.Show();
            }
        }
    }
}