using System.Threading.Tasks;

namespace _src.Code.App.Logic.Game
{
    public partial class GameLogic
    {
        public async Task PlayAnimation(string username)
        {
            // if (!_clientDataService.PlayerUsernameToGameModuleId.ContainsKey(username))
            // {
            //     return;
            // }
            //
            // var gameModuleId = _clientDataService.PlayerUsernameToGameModuleId[username];
            // var gameModule = _gameModuleService.GetGameModuleById(gameModuleId);

            // if (gameModuleId == _clientDataService.PlayerId)
            // {
            //     // It's the local player, so do not trigger (assume handled locally)
            //     return;
            // }

            // if (gameModule is IPlayer player)
            // {
            //     player.DoCharacterAnimation(false);
            // }
            
            await Task.CompletedTask;
        }
    }
}