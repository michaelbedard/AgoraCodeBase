using _src.Code.Core.Actors;
using _src.Code.Core.Interfaces.Services;
using _src.Code.Core.Interfaces.UI;
using _src.Code.UI.Common;
using UnityEngine.UIElements;

namespace _src.Code.UI.Scenes.Additive.SettingsWindow
{
    public class ProfilePanel : CustomVisualElement, IVisualElement
    {
         public new class UxmlFactory : UxmlFactory<ProfilePanel, UxmlTraits> { }

        private TextInput _userIdTextField;
        private TextInput _usernameTextField;
        
        protected override void InitializeCore()
        {
            _userIdTextField = Get<TextInput>("UserId");
            _usernameTextField = Get<TextInput>("Username");

            _userIdTextField.Label = "UserId";
            _usernameTextField.Label = "Username";
            
            // set title
            Get<SubTitle>("ProfileSubTitle").Label.text = "Profile";

            // for now
            var clientDataService = ServiceLocator.GetService<IClientDataService>();
            _userIdTextField.Value = clientDataService.Id;
            _usernameTextField.Value = clientDataService.Username;
        }

        public void SetProfileInformation(string userId, string username)
        {
            _userIdTextField.Value = userId;
            _usernameTextField.Value = username;
        }
    }
}