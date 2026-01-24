using System.Collections.Generic;
using System.Threading.Tasks;
using _src.Code.Core.Actors;
using _src.Code.Core.Signals.Game;
using Agora.Core.Actors;
using Agora.Core.Payloads.Hubs;

namespace _src.Code.App.Logic.Game
{
    public partial class GameLogic
    {
        public async Task<Result> UpdateGame(UpdateGamePayload payload)
        {
            // send animations
            foreach (var animation in payload.Animations)
            {
                _signalBus.Fire(new GameAnimationSignal(animation));
            }
            
            // send input
            if (payload.Input != null)
            {
                _signalBus.Fire(new GameInputSignal(payload.Input));
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
            
            // update glow
            foreach (var gameModule in _gameModuleService.GetAllGameModules())
            {
                gameModule.UpdateGlowColor();
            }
            
            // send descriptions
            foreach (var description in payload.Descriptions)
            {
                var gm = _gameModuleService.GetGameModuleById(description.Key);
                gm.Description = description.Value;
            }
            
            return Result.Success();
        }
    }
}