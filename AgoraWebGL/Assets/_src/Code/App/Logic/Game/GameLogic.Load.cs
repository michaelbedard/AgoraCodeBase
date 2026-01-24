using System;
using System.Linq;
using System.Threading.Tasks;
using _src.Code.Core.Interfaces.GameModules;
using _src.Code.Core.Interfaces.GameModules.Other;
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

namespace _src.Code.App.Logic.Game
{
    public partial class GameLogic
    {
        public async Task<Result> LoadGame(LoadGamePayload payload)
        {
            // 1. Load data
            _gameDataService.LoadData(payload);

            // 2. Change Scene
            // (Assuming LoadGameScene takes a Func callback)
            await _sceneService.LoadGameScene(async () =>
            {
                // Logic inside the new scene
                _gameDataService.LoadData(payload);

                // // Setup UI
                // var startGameScreen = await _visualElementService.GetOrCreate<StartGameScreen>();
                // startGameScreen.LoadingWidget.Label.text = "Loading Game...";
                // startGameScreen.LoadingWidget.parent.style.display = DisplayStyle.Flex;
                // startGameScreen.ReadyButton.Button.parent.style.display = DisplayStyle.None;
                //
                // // Setup rules
                // await startGameScreen.SetSimplifiedRule(payload.SimplifiedRulesAddress);
                
            }, true);
            
            // LOGIC
            
            var loadData = payload;
            var numberOfPlayers = loadData.Players.Count();
            
            // block start btn
            // var startGameScreen = await _visualElementService.GetOrCreate<StartGameScreen>();
            // startGameScreen.LoadingWidget.Label.text = "Loading Game...";
            // startGameScreen.LoadingWidget.parent.style.display = DisplayStyle.Flex;
            // startGameScreen.ReadyButton.Button.parent.style.display = DisplayStyle.None;
            
            // load game info
            _gameDataService.LoadData(loadData);
            
            // set client player id
            _gameDataService.PlayerId = loadData.PlayerUsernameToGameModuleId[_clientDataService.Username];
            
            // setup music and environment
            var playMusicTask = _audioService.PlayBackgroundMusicAsync(loadData.MusicAddress);
            var instantiateEnvironmentTask = Addressables.InstantiateAsync(loadData.EnvironmentAddress).Task;
            await Task.WhenAll(playMusicTask, instantiateEnvironmentTask);
            
            // Setup rules
            // await startGameScreen.SetSimplifiedRule(loadData.SimplifiedRulesAddress);
            
            var rulesWindow = await _visualElementService.GetOrCreate<RulesWindow>();
            await rulesWindow.SetRules(loadData.Title, loadData.RulesAddress);
            
            // Setup board image
            // if (!string.IsNullOrWhiteSpace(loadData.BoardAddress))
            // {
            //     var boardImage = await Addressables.LoadAssetAsync<Sprite>(loadData.BoardAddress).Task;
            //     var boardImageObject = new GameObject("boardImage");
            //     boardImageObject.transform.localScale = new Vector3(2f, 2f, 2f);
            //     boardImageObject.AddComponent<SpriteRenderer>().sprite = boardImage;
            //     _boardPlaneService.AddCommonElement(boardImageObject.transform, Vector2.zero, offset: 0.1f);
            //     boardImageObject.transform.localRotation = Quaternion.Euler(90f, 0f, 0f); // after
            // }
            
            // get client profile and seat
            var clientProfile = loadData.Players.Find(p => p.Username == _clientDataService.Username);
            var clientSeat = loadData.PlayerUsernameToSeat[clientProfile.Username];
            
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
                    var offset = 0.05f; // just enough so that images dont overlap together

                    if (gameModuleLoadData.Type == GameModuleType.Deck)
                        offset += 0.5f;
                    if (gameModuleLoadData.Type == GameModuleType.Dice)
                        offset += 0.5f;
                    if (gameModuleLoadData.Type == GameModuleType.Marker)
                        offset += 0.35f;
                    
                    if (gameModuleLoadData.IsPlayerModule)
                    {
                        // var seatDistance = gameModuleLoadData.Seat - clientSeat;

                        var seatDistance = 1;

                        var radiusOffset = 0f;
                        // if (seatDistance != 0)
                        // {
                        //     if (numberOfPlayers == 3)
                        //     {
                        //         radiusOffset = loadData.DiagonalPlayerPositionOffset;
                        //     } 
                        //     if (numberOfPlayers == 4 && Mathf.Abs(seatDistance) != 2)
                        //     {
                        //         radiusOffset = loadData.HorizontalPlayerPositionOffset;
                        //     }
                        // }
                        
                        _boardPlaneService.AddPlayerElement(transform, position, loadData.Players.Count, seatDistance, offset:offset, radiusOffset:radiusOffset);
                    }
                    else
                    {
                        _boardPlaneService.AddCommonElement(transform, position, offset:offset);
                    }
                }
                else
                {
                    // default position, somewhere far away
                    transform.position = new Vector3(0, 0, 40f);
                }

                // if (gm is IPlayer p)
                // {
                //     playerGameModules.Add(p);
                // }
            }
            
            // setup hand for each player
            foreach (var playerProfile in loadData.Players)
            {
                var seatDistance = loadData.PlayerUsernameToSeat[playerProfile.Username] - clientSeat;
                var gameModuleId = loadData.PlayerUsernameToGameModuleId[playerProfile.Username];
                // var player = playerGameModules.Find(p => p.Id == gameModuleId);
                //
                // var hand = player.Hand;
                //
                // // different position settings depending on nb of players and seat
                // await PositionHand(playerProfile, hand, seatDistance, loadData.Players.Count);
                // hand.PlayerId = gameModuleId;
            }
            
            // set marker and tokens to starting zone
            foreach (var MarkerDto in loadData.GameModules.Where(l => l is MarkerDto).Cast<MarkerDto>())
            {
                var marker = _gameModuleService.GetGameModuleById<IMarker>(MarkerDto.Id);
                var startingZone = _gameModuleService.GetGameModuleById<IZone>(MarkerDto.StartZoneId);

                startingZone.AddMarker(marker);
                startingZone.UpdateObjectPositions(instantaneous: true);
            }
            
            foreach (var TokenDto in loadData.GameModules.Where(l => l is TokenDto).Cast<TokenDto>())
            {
                var token = _gameModuleService.GetGameModuleById<IToken>(TokenDto.Id);
                var startingZone = _gameModuleService.GetGameModuleById<IZone>(TokenDto.StartZoneId);

                startingZone.AddToken(token);
                startingZone.UpdateObjectPositions(instantaneous: true);
            }
            
            // set camera position
            var initialCameraPosition = new Vector3(0, 20, -12);
            
            _cameraManager.Initialize(initialCameraPosition);
            
            // clear animation queue
            _animationQueueService.Clear();
                
            // set ready button enable
            // startGameScreen.LoadingWidget.parent.style.display = DisplayStyle.None;
            // startGameScreen.ReadyButton.style.display = DisplayStyle.Flex;
            
            // set description ready
            await _descriptionManager.Start();

            return Result.Success();
        }

        private async Task PositionHand(UserDto playerProfile, IHand hand, int seatDistance, int numberOfPlayers)
        {
            var gameScreen = await _visualElementService.GetOrCreate<GameScreen>();
            
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
                        SetUpTag(gameScreen.TopTag, playerProfile);
                        break;
                    }
                    case 3:
                    {
                        if (seatDistance == -1 || seatDistance == 2)
                        {
                            // up-right
                            hand.Position(30f, 45f, 0.13f, 3f, 2f);
                            SetUpTag(gameScreen.TopRightTag, playerProfile);
                        }
                        else
                        {
                            // up-left
                            hand.Position(150f, 135f, 0.13f, 3f, 2f);
                            SetUpTag(gameScreen.TopLeftTag, playerProfile);
                        }
                        break;
                    }
                    case 4:
                    {
                        if (Mathf.Abs(seatDistance) == 2)
                        {
                            // up
                            hand.Position(90f, 90f, -0.1f, 3f, 2f);
                            SetUpTag(gameScreen.TopTag, playerProfile);
                        }
                        else if (seatDistance == 1)
                        {
                            // right
                            hand.Position(0f, 0f, -0.1f, 3f, 2f);
                            SetUpTag(gameScreen.RightTag, playerProfile);
                        }
                        else
                        {
                            // left
                            hand.Position(180f, 180f, -0.1f, 3f, 2f);
                            SetUpTag(gameScreen.LeftTag, playerProfile);
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

        private void SetUpTag(SubTitle subTitle, UserDto playerProfile)
        {
            subTitle.parent.style.display = DisplayStyle.Flex;
            if (_gameDataService.IsAgainstComputer)
            {
                subTitle.Label.text = $"{playerProfile.Username}";
            }
            else
            {
                subTitle.Label.text = $"{playerProfile.Username} ({playerProfile.Pronouns})";
            }
        }
    }
}