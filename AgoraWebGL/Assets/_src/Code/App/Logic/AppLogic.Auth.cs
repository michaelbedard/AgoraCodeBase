using System.Threading.Tasks;
using _src.Code.UI.Shared;
using Agora.Core.Payloads.Http.Auth;

namespace _src.Code.App.Logic
{
    public partial class AppLogic
    {
        public async Task Login(string currentChannelId, string authCode)
        {
            _clientDataService.ChannelId = currentChannelId;
            
            var result = await _authHttpProxy.Login(new LoginPayload()
            {
                OAuthCode = authCode,
                LobbyId = currentChannelId,
            });
            
            if (result.IsSuccess)
            {
                var user = result.Data;
                _clientDataService.Id = user.Id;
                _clientDataService.Username = user.Username;
                _clientDataService.AvatarId = user.Avatar;
                _clientDataService.Pronouns = user.Pronouns;
                
                // Connect to hub
                // var hubConnectionSuccess = await _hubProxy.ConnectAsync();
                // if (!hubConnectionSuccess)
                // {
                //     var warning = await _visualElementService.Create<WarningPopup>();
                //     warning.Title.Label.text = "Hub Connection failed!";
                //     warning.Button.Label.text = "Ok";
                //     warning.Show();
                //     return;
                // }
                
                var popup = await _visualElementService.GetOrCreate<WarningPopup>();
                popup.Title.Label.text = "Login success!";
                popup.Message.text = result.Data.Username + "\n" + _clientDataService.Id + "\n" + _clientDataService.ChannelId;
                popup.Button.Label.text = "Ok";
                popup.Show();
            }
            else
            {
                var popup = await _visualElementService.GetOrCreate<WarningPopup>();
                popup.Title.Label.text = "Login failed!";
                popup.Message.text = result.Message;
                popup.Button.Label.text = "Ok";
                popup.Show();
            }
        }
        
        public async Task Logout()
        {
            await _authHttpProxy.Logout();
        }
    }
}