using System;
using System.Linq;
using System.Threading.Tasks;
using _src.Code.Core.Interfaces.GameModules;
using _src.Code.Core.Interfaces.GameModules.Other;
using _src.Code.Core.Interfaces.Managers;
using _src.Code.UI.Common;
using _src.Code.UI.Scenes.Game;
using Agora.Core.Actors;
using Agora.Core.Dtos;
using Agora.Core.Dtos.Game.GameModules;
using Agora.Core.Enums;
using Agora.Core.Payloads.Hubs;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UIElements;
using UnityEngine.XR;
using Zenject;

namespace _src.Code.App.Logic.Game
{
    public partial class GameLogic
    {
        [Inject] private IHandManager _handManager;
        
        public async Task<Result> LoadGame(LoadGamePayload payload)
        {
            _hasStated = false;
            
            Console.WriteLine("LoadGame!!!");
            
            // LOGIC
            
            var loadData = payload;
            _clientDataService.LoadGameData(payload);
            
            // block start btn
            var gameScreen = await _visualElementService.GetOrCreate<GameScreen>();
            // startGameScreen.LoadingWidget.Label.text = "Loading Game...";
            // startGameScreen.LoadingWidget.parent.style.display = DisplayStyle.Flex;
            // startGameScreen.ReadyButton.Button.parent.style.display = DisplayStyle.None;
            
            // setup music and environment
            // var playMusicTask = _audioService.PlayBackgroundMusicAsync(loadData.MusicAddress);
            // var instantiateEnvironmentTask = Addressables.InstantiateAsync(loadData.EnvironmentAddress).Task;
            // await Task.WhenAll(playMusicTask, instantiateEnvironmentTask);
            
            // Setup rules
            // await startGameScreen.SetSimplifiedRule(loadData.SimplifiedRulesAddress);
            
            // var rulesWindow = await _visualElementService.GetOrCreate<RulesWindow>();
            // await rulesWindow.SetRules(loadData.Title, loadData.RulesAddress);
            
            // get client profile and seat
            var clientProfile = loadData.Players.Find(p => p.Id == _clientDataService.Id);
            var clientSeat = loadData.PlayerIdToSeat[clientProfile.Id];
            
            // setup game modules
            foreach (var gameModuleLoadData in loadData.GameModules)
            {
                // instantiate
                var gm = await _gameModuleService.InstantiateGameModuleAsync(gameModuleLoadData, gameModuleLoadData.Type);
                
                // position
                var transform = gm.Transform;
                if (gameModuleLoadData.Position != null)
                {
                    var position = new Vector2(gameModuleLoadData.Position.X, gameModuleLoadData.Position.Y);

                    if (!string.IsNullOrEmpty(gameModuleLoadData.OwnerId))
                    {
                        var ownerSeat = loadData.PlayerIdToSeat[gameModuleLoadData.OwnerId];
                        var seatDistance = ownerSeat - clientSeat;
                        
                        _boardPlaneService.AddPlayerElement(transform, position, loadData.Players.Count, seatDistance, offset: 0, radiusOffset: 0);
                    }
                    else
                    {
                        _boardPlaneService.AddCommonElement(transform, position, offset: 0);
                    }
                }
                else
                {
                    // default position, somewhere far away
                    transform.position = new Vector3(0, 0, 40f);
                }
            }
            
            // setup hand for each player
            foreach (var player in loadData.Players)
            {
                var playerSeat = loadData.PlayerIdToSeat[player.Id];
                var seatDistance = playerSeat - clientSeat;
                
                var hand = _handManager.CreateHand(player.Id);
                hand.PlayerId = player.Id;
                _clientDataService.PlayerIdToHand[player.Id] = hand;
                
                // different position settings depending on nb of players and seat
                PositionHand(hand, seatDistance, loadData.Players.Count);
            }
            
            // set camera position
            var initialCameraPosition = new Vector3(0, 20, -12);
            
            _cameraManager.Initialize(initialCameraPosition);
            
            // clear animation queue
            _animationQueueService.Clear();
            
            // set description ready
            await _descriptionManager.Start();

            return Result.Success();
        }

        private void PositionHand(IHand hand, int seatDistance, int numberOfPlayers)
        {
            if (seatDistance == 0)
            {
                // down, this is client
                hand.Position(270f, 270f, -0.1f, 4f, 3f);
                hand.SetIsFaceUp(false);
            }
            else
            {
                switch (numberOfPlayers)
                {
                    case 1:
                    {
                        // down, this is client
                        hand.Position(270f, 270f, -0.1f, 4f, 3f);
                        break;
                    }
                    case 2:
                    {
                        // up
                        hand.Position(90f, 90f, -0.1f, 3f, 2f);
                        break;
                    }
                    case 3:
                    {
                        if (seatDistance == -1 || seatDistance == 2)
                        {
                            // up-right
                            hand.Position(30f, 45f, 0.13f, 3f, 2f);
                        }
                        else
                        {
                            // up-left
                            hand.Position(150f, 135f, 0.13f, 3f, 2f);
                        }

                        break;
                    }
                    case 4:
                    {
                        if (Mathf.Abs(seatDistance) == 2)
                        {
                            // up
                            hand.Position(90f, 90f, -0.1f, 3f, 2f);
                        }
                        else if (seatDistance == 1)
                        {
                            // right
                            hand.Position(0f, 0f, -0.1f, 3f, 2f);
                        }
                        else
                        {
                            // left
                            hand.Position(180f, 180f, -0.1f, 3f, 2f);
                        }

                        break;
                    }
                    default:
                    {
                        throw new NotImplementedException();
                    }
                }

                hand.SetIsFaceUp(true);
            }
        }
    }
}