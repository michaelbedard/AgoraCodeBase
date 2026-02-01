using System.Linq;
using System.Threading.Tasks;
using _src.Code.Core.Actors;
using _src.Code.Core.Interfaces.Logic;
using _src.Code.UI.Scenes.Entry;
using Agora.Core.Actors;
using Agora.Core.Dtos;
using Agora.Core.Enums;
using Agora.Core.Payloads.Hubs;
using UnityEngine.UIElements;

namespace _src.Code.App.Logic.Entry
{
    public partial class EntryLogic
    {
        public async Task<Result> PlayerJoinLobby(UserDto player)
        {
            // do not add ourselves
            if (_clientDataService.Id == player.Id) return null;
            
            var lobbyView = await _uiService.GetOrCreate<LobbyView>();
            await lobbyView.AddPlayer(player);
            
            _clientDataService.UsersInLobby.Add(player);
            
            return Result.Success();
        }
        
        public async Task<Result> PlayerLeaveLobby(string userId)
         {
             var lobbyView = await _uiService.GetOrCreate<LobbyView>();
             lobbyView.RemovePlayer(userId);

             _clientDataService.UsersInLobby.RemoveAll(u => u.Id == userId);
             
             return Result.Success();
         }
        
        public async Task<Result> GameSelected(GameKey gameKey)
        {
            var lobbyView = await _uiService.GetOrCreate<LobbyView>();
            lobbyView.SetGame(gameKey);
            
            return Result.Success();
        }
        
        public async Task<Result> LaunchGame(LoadGamePayload payload)
        {
            // 1. Load data
            _clientDataService.LoadGameData(payload);

            // 2. Change Scene
            await _sceneService.LoadGameScene(async () =>
            {
                // Setup UI
                // var startGameScreen = await _uiService.GetOrCreate<StartGameScreen>();
                // startGameScreen.LoadingWidget.Label.text = "Loading Game...";
                // startGameScreen.LoadingWidget.parent.style.display = DisplayStyle.Flex;
                // startGameScreen.ReadyButton.Button.parent.style.display = DisplayStyle.None;
                
                var result = await ServiceLocator.GetService<IGameLogic>().LoadGame(payload);
                if (result.IsSuccess)
                {
                    await _gameHubProxy.ReadyToStart();
                }

            }, true);

            return Result.Success();
        }
    }
}