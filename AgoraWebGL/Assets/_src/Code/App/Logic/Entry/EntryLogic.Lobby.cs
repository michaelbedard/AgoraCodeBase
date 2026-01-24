using System.Threading.Tasks;
using _src.Code.UI.Scenes.Entry;
using Agora.Core.Actors;
using Agora.Core.Dtos;
using Agora.Core.Enums;

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
            
            return Result.Success();
        }
        
        public async Task<Result> PlayerLeaveLobby(string userId)
         {
             var lobbyView = await _uiService.GetOrCreate<LobbyView>();
             lobbyView.RemovePlayer(userId);
             
             return Result.Success();
         }
        
        public async Task<Result> GameSelected(GameKey gameKey)
        {
            var lobbyView = await _uiService.GetOrCreate<LobbyView>();
            lobbyView.SetGame(gameKey);
            
            return Result.Success();
        }
    }
}