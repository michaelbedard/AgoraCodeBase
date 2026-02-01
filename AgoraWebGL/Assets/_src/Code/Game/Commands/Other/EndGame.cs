using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _src.Code.Core.Actors;
using _src.Code.Core.Interfaces.Managers;
using _src.Code.Core.Interfaces.Services;
using Agora.Core.Dtos.Game.Commands.Other;
using UnityEngine;
using Zenject;

namespace _src.Code.Game.Commands.Other
{
    public class EndGame : BaseAnimation<EndCommandDto>
    {
        private readonly IVisualElementService _visualElementService;
        private readonly ICharacterService _characterService;
        private readonly ICameraPlaneService _cameraPlaneService;
        
        public EndGame(
            SignalBus signalBus, 
            IAnimationQueueService animationQueueService, 
            IGameModuleService gameModuleService, 
            IVisualElementService visualElementService,
            ICharacterService characterService,
            ICameraPlaneService cameraPlaneService) 
            : base(signalBus, animationQueueService, gameModuleService)
        {
            _visualElementService = visualElementService;
            _characterService = characterService;
            _cameraPlaneService = cameraPlaneService;
        }

        protected override async void AnimateCore(EndCommandDto endCommand)
        {
            try
            {
                AnimationQueueService.Push(async () =>
                {
                    // var gameScreen = await _visualElementService.GetOrCreate<GameScreen>();
                    // var endGameScreen = await _visualElementService.GetOrCreate<EndGameScreen>();
                    //
                    // // set message
                    // endGameScreen.Window.SetMessage(endCommand.Message);
                    //
                    // // set label
                    // switch (endCommand)
                    // {
                    //     case SingleWinnerEndCommandDto payload:
                    //         endGameScreen.Label.Label.text = $"Player {payload.WinnerUsername} has won!";
                    //         break;
                    //     case CompetitiveEndCommandDto payload:
                    //         endGameScreen.Label.Label.text = $"Player {payload.OrderedPlayerUsernames[0]} has won!";
                    //         break;
                    //     case CooperativeEndCommandDto payload:
                    //         endGameScreen.Label.Label.text = $"Group XXX has won!!!";
                    //         break;
                    //     default:
                    //         throw new Exception("Unhandled signal type");
                    // }
                    //
                    // // position characters
                    // switch (endCommand)
                    // {
                    //     case SingleWinnerEndCommandDto payload:
                    //         var orderedPlayerUsernames = new List<string>() { payload.WinnerUsername };
                    //         orderedPlayerUsernames = orderedPlayerUsernames.Concat(payload.LosersIUsername).ToList();
                    //
                    //         await PositionPlayersByRankAsync(orderedPlayerUsernames);
                    //         endGameScreen.Window.SetWinners(orderedPlayerUsernames);
                    //
                    //         break;
                    //     case CompetitiveEndCommandDto payload:
                    //         await PositionPlayersByRankAsync(payload.OrderedPlayerUsernames.ToList());
                    //         endGameScreen.Window.SetWinners(payload.OrderedPlayerUsernames.ToList());
                    //         break;
                    //     case CooperativeEndCommandDto payload:
                    //         await PositionPlayersAsGroupAsync(payload);
                    //         break;
                    //     default:
                    //         throw new Exception("Unhandled signal type");
                    // }
                    //
                    // // reset allowed actions 
                    // foreach (var gameModule in GameModuleService.GetAllGameModules())
                    // {
                    //     gameModule.ClickActions = new List<ClickAction>();
                    //     gameModule.DragActions = new List<DragAction>();
                    // }
                    //
                    // // remove description
                    // ServiceLocator.GetService<IDescriptionManager>().Stop();
                    //
                    // // show screen
                    // gameScreen.ShowEndGameScreen();
                    
                    // NOTE : WE DONT CALL NEXT ANIMATION
                    
                });
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        private async Task PositionPlayersByRankAsync(List<string> orderedPlayerUsernames)
        {
        //     // position each player
        //     foreach (var playerUsername in orderedPlayerUsernames)
        //     {
        //         var rank = orderedPlayerUsernames.IndexOf(playerUsername);
        //         var targetPlayerId = _gameDataService.PlayerUsernameToGameModuleId[playerUsername];
        //         var targetPlayer = (IPlayer)GameModuleService.GetGameModuleById(targetPlayerId);
        //         var character = await _characterService.GetCharacterById(targetPlayer.CharacterId, false);
        //
        //         // position character
        //         var position = rank switch
        //         {
        //             0 => new Vector2(-1, -1),
        //             1 => new Vector2(-0.4f, -0.7f),
        //             2 => new Vector2(-0.2f, -0.6f),
        //             3 => new Vector2(-0.3f, -0.6f),
        //             _ => throw new Exception("Unhandled signal type")
        //         };
        //         
        //         var offset = rank switch
        //         {
        //             0 => 0f,
        //             1 => -2.5f,
        //             2 => -3f,
        //             3 => -3f,
        //             _ => throw new Exception("Unhandled signal type")
        //         };
        //         
        //         var tangentialRotation = rank switch
        //         {
        //             0 => -40f,
        //             _ => 20f
        //         };
        //
        //         _cameraPlaneService.PositionElement(character.transform, position, offset: offset, tangentialRotation: tangentialRotation);
        //         character.SetActive(true);
        //         
        //         if (rank == 0)
        //         {
        //             character.GetComponent<AnimatorController>().DoWinningAnimation();
        //         }
        //     }
        }

        private async Task PositionPlayersAsGroupAsync(CooperativeEndCommandDto payload)
        {
        }
    }
}