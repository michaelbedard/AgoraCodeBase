using System;
using _src.Code.UI.Common;
using _src.Code.UI.Scenes.Entry;
using _src.Code.UI.Shared;
using Agora.Core.Dtos.Lobby;
using UnityEngine;

namespace _src.Code.App.Logic.Entry
{
    public partial class EntryLogic
    {
        private EntryScreen _entryScreen;
        
        public async void Initialize()
        {
            // 1. Load the Screen
            _entryScreen = await _uiService.GetOrCreate<EntryScreen>();
            
            // 2. Subscribe to Events
            _entryScreen.PlayWithFriendsClicked += HandlePlayWithFriends;
            _entryScreen.PlayOnlineClicked += HandlePlayOnline;
            _entryScreen.ShopClicked += HandleShopClicked;
        }

        public void Dispose()
        {
            if (_entryScreen != null)
            {
                _entryScreen.PlayWithFriendsClicked -= HandlePlayWithFriends;
                _entryScreen.PlayOnlineClicked -= HandlePlayOnline;
                _entryScreen.ShopClicked -= HandleShopClicked;
            }
        }
        
        private async void HandlePlayWithFriends()
        {
            // 1. Show Loading
            var loadingWidget = await _uiService.GetOrCreate<LoadingWidget>();
            _entryScreen.HideMainView();
            _entryScreen.ShowLoadingWidget(loadingWidget);
            
            // wait 1 sec
            await System.Threading.Tasks.Task.Delay(1000);
            
            // 2. Network Request
            var result = await _lobbyHttpProxy.JoinLobby(_clientDataService.ChannelId);
            
            if (result.IsSuccess)
            {
                _clientDataService.LobbyId = result.Data.Id;
                
                // 3. Success: Show Lobby View
                var lobbyView = await _uiService.GetOrCreate<LobbyView>();
                lobbyView.SetTitle(result.Data.Id);
                await lobbyView.SetPlayers(result.Data.Players);

                lobbyView.StartGameClicked += async () =>
                {
                    throw new NotImplementedException();
                };
                
                lobbyView.CancelClicked += async () =>
                {
                    var cancelResult = await _lobbyHttpProxy.LeaveLobby(_clientDataService.LobbyId);
                    if (cancelResult.IsSuccess)
                    {
                        _entryScreen.HideLobbyView();
                    }
                    else
                    {
                        var warningPopup = await _uiService.GetOrCreate<WarningPopup>();
                        warningPopup.Title.Label.text = "Unable to leave lobby";
                        warningPopup.Message.text = result.Message;
                
                        Debug.Log("Failed to leave lobby");
                    }
                };
                
                Debug.Log($"Joined lobby: {result.Data.Id}");
                
                _entryScreen.HideLoadingWidget();
                _entryScreen.ShowLobbyView(lobbyView);
            }
            else
            {
                // 4. Failure: Show Warning
                var warningPopup = await _uiService.GetOrCreate<WarningPopup>();
                warningPopup.Title.Label.text = "Unable to join lobby";
                warningPopup.Message.text = result.Message;
                
                Debug.Log("Failed to join lobby");
                
                _entryScreen.HideLoadingWidget();
                _entryScreen.ShowMainView();
            }
        }

        private async void HandlePlayOnline()
        {
            Debug.Log("Play Online Clicked");
            
            var popup = await _uiService.GetOrCreate<WarningPopup>();
            popup.Title.Label.text = "Coming Soon";
            popup.Message.text = "This functionality has not been implemented yet.";
            popup.Show();
        }

        private async void HandleShopClicked()
        {
            Debug.Log("Shop Clicked");

            var popup = await _uiService.GetOrCreate<WarningPopup>();
            popup.Title.Label.text = "Coming Soon";
            popup.Message.text = "This functionality has not been implemented yet.";
            popup.Show();
        }
    }
}