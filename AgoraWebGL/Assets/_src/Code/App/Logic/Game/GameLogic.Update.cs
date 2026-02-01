using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _src.Code.Core.Actors;
using _src.Code.Core.Interfaces.Managers;
using _src.Code.Core.Signals.Game;
using _src.Code.UI.Scenes.Game;
using Agora.Core.Actors;
using Agora.Core.Payloads.Hubs;
using Zenject;

namespace _src.Code.App.Logic.Game
{
    public partial class GameLogic
    {
        [Inject] private ITurnManager _turnManager;
        
        private bool _hasStated;
        
        public async Task<Result> UpdateGame(UpdateGamePayload payload)
        {
            if (!_hasStated)
            {
                // hide loading view
                var gameScreen = await _visualElementService.GetOrCreate<GameScreen>();
                gameScreen.HideLoadingView();

                _hasStated = true;
            }
            
            // send animations
            foreach (var animation in payload.Animations)
            {
                _signalBus.Fire(new GameAnimationSignal(animation));
            }
            
            // reset allowed actions 
            foreach (var gameModule in _gameModuleService.GetAllGameModules())
            {
                gameModule.ClickActions = new List<ClickAction>();
                gameModule.DragActions = new List<DragAction>();
            }
            
            // send actions
            foreach (var action in payload.Actions)
            {
                _signalBus.Fire(new GameActionSignal(action));
            }
            
            // send input
            if (payload.Input != null)
            {
                _signalBus.Fire(new GameInputSignal(payload.Input));
            }
            
            // update glow
            foreach (var gameModule in _gameModuleService.GetAllGameModules())
            {
                gameModule.UpdateGlowColor();
            }
            
            // send descriptions
            foreach (var description in payload.Descriptions)
            {
                if (_clientDataService.PlayerIdToHand.Keys.Contains(description.Key))
                {
                    // its a player.  Set description to the hand
                    _clientDataService.PlayerIdToHand[description.Key].Description = description.Value;
                    
                }
                else
                {
                    var gm = _gameModuleService.GetGameModuleById(description.Key);
                    gm.Description = description.Value;
                }
            }
            
            // set player turn visual
            await _turnManager.UpdateVisual(payload.PlayersTakingTurn);
            
            return Result.Success();
        }
    }
}