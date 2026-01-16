using System.Threading.Tasks;

namespace _src.Code.App.Logic.Game
{
    public partial class GameLogic
    {
        public async Task PlayAnimation(string username)
        {
            if (!_gameDataService.PlayerUsernameToGameModuleId.ContainsKey(username))
            {
                return;
            }

            var gameModuleId = _gameDataService.PlayerUsernameToGameModuleId[username];
            var gameModule = _gameModuleService.GetGameModuleById(gameModuleId);

            if (gameModuleId == _gameDataService.PlayerId)
            {
                // It's the local player, so do not trigger (assume handled locally)
                return;
            }

            // if (gameModule is IPlayer player)
            // {
            //     player.DoCharacterAnimation(false);
            // }
            
            await Task.CompletedTask;
        }
    }
}