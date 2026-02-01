using System.Collections.Generic;
using _src.Code.Core.Actors;
using _src.Code.Core.Interfaces.Logic;
using _src.Code.Core.Interfaces.Managers;
using _src.Code.Core.Interfaces.Services;
using _src.Code.UI.Scenes.Game;
using Agora.Core.Dtos;
using Agora.Core.Dtos.Game.Commands;
using Agora.Core.Dtos.Game.Commands.Actions;
using Agora.Core.Dtos.Game.Commands.Inputs;
using Agora.Core.Dtos.Game.GameModules;
using Agora.Core.Dtos.Game.Other;
using Agora.Core.Enums;
using Agora.Core.Payloads.Hubs;
using UnityEngine;
using Zenject;
using Position = Agora.Core.Actors.Position;

namespace _src.Code.__test__
{
    public class TestGame : BaseTest
    {
        private ISceneService _sceneService;
        private IVisualElementService _visualElementService;
        [Inject]
        public IGameLogic GameLogic;
        [Inject]
        public IClientDataService ClientDataService;

        private LoadGamePayload loadGameDto = new LoadGamePayload()
        {
            Title = "ExampleGame",
            EnvironmentAddress = "Environments/Outdoor.prefab",
            MusicAddress = "Assets/_src/Audio/439_Goodhaven.mp3",
            Players = new List<UserDto>()
            {
                new UserDto() { Id = "mock_jade", Username = "jade", Avatar = 0},
                new UserDto() { Id = "bob_id", Username = "bob", Avatar = 1},
                new UserDto() { Id = "cedric_id", Username = "cedric", Avatar = 2},
            },
            PlayerIdToSeat = new Dictionary<string, int>()
            {
                { "mock_jade", 0 },
                { "bob_id", 1 },
                { "cedric_id", 2 },
            },
            GameModules = new List<GameModuleDto>()
            {
                // Decks
                new DeckDto()
                {
                    Id = "Deck1",
                    Name = "Drawing Deck",
                    Color = "#228B22",
                    OwnerId = "",
                    Position = new Position()
                    {
                        X = 1.3f,
                        Y = 0
                    },
                    TopImage = "Assets/_src/Games/Uno/back.png",
                    Cards = new List<CardDto>()
                    {
                        // Cards
                        new CardDto()
                        {
                            Id = "C_0_blue",
                            Name = "0 Blue",
                            FrontImage = "Assets/_src/Games/Uno/blue/0_blue.png",
                            BackImage = "Assets/_src/Games/Uno/back.png"
                        },
                        new CardDto()
                        {
                            Id = "C_7_yellow",
                            FrontImage = "Assets/_src/Games/Uno/yellow/7_yellow.png",
                            BackImage = "Assets/_src/Games/Uno/back.png"
                        },
                        new CardDto()
                        {
                            Id = "C_5_blue",
                            FrontImage = "Assets/_src/Games/Uno/blue/5_blue.png",
                            BackImage = "Assets/_src/Games/Uno/back.png"
                        },
                        new CardDto()
                        {
                            Id = "C_0_red",
                            FrontImage = "Assets/_src/Games/Uno/red/0_red.png",
                            BackImage = "Assets/_src/Games/Uno/back.png"
                        },
                        new CardDto()
                        {
                            Id = "C_1_red",
                            FrontImage = "Assets/_src/Games/Uno/red/1_red.png",
                            BackImage = "Assets/_src/Games/Uno/back.png"
                        },
                        new CardDto()
                        {
                            Id = "C_2_red",
                            FrontImage = "Assets/_src/Games/Uno/red/2_red.png",
                            BackImage = "Assets/_src/Games/Uno/back.png"
                        },
                        new CardDto()
                        {
                            Id = "C_3_red",
                            FrontImage = "Assets/_src/Games/Uno/red/3_red.png",
                            BackImage = "Assets/_src/Games/Uno/back.png"
                        },
                        new CardDto()
                        {
                            Id = "C_4_red",
                            FrontImage = "Assets/_src/Games/Uno/red/4_red.png",
                            BackImage = "Assets/_src/Games/Uno/back.png"
                        },
                    }
                },

                // Zone
                new ZoneDto()
                {
                    Id = "Zone__A",
                    Name = "Playing Zone",
                    Position = new Position()
                    {
                        X = 0,
                        Y = -3f,
                    },
                    OwnerId = "mock_jade",
                    Width = 2,
                    Height = 3,
                    StackingMethod = ZoneStackingMethod.Diagonal
                },
                new ZoneDto()
                {
                    Id = "Zone__B",
                    Name = "Playing Zone",
                    Position = new Position()
                    {
                        X = 0,
                        Y = -3f,
                    },
                    OwnerId = "bob_id",
                    Width = 2,
                    Height = 3
                },
                new ZoneDto()
                {
                    Id = "Zone__C",
                    Name = "Playing Zone",
                    Position = new Position()
                    {
                        X = 0,
                        Y = -3f,
                    },
                    OwnerId = "cedric_id",
                    Width = 2,
                    Height = 3
                },
            }
        };
        
        [Inject]
        public void Construct(ISceneService sceneService, IVisualElementService visualElementService)
        {
            _sceneService = sceneService;
            _visualElementService = visualElementService;
        }

        
        public async void Update()
        {
            
            if (Input.GetKeyDown(KeyCode.L))
            {
                await GameLogic.LoadGame(loadGameDto);
            }
            
            if (Input.GetKeyDown(KeyCode.U))
            {
                Debug.Log("OnUpdateGameStateClicked!!!");

                await GameLogic.UpdateGame(new UpdateGamePayload()
                {
                    Animations = new List<CommandDto>()
                    {
                        // new FlipTopCardAnimationDto()
                        // {
                        //     PlayerId = null,
                        //     CardId = "C_7_yellow",
                        //     DeckId = "Deck1",
                        //     ZoneId = "Zone__C"
                        // },
                        // new DrawCardAnimationDto()
                        // {
                        //     PlayerId = "mock_jade",
                        //     DeckId = "Deck1",
                        //     CardId = "C_0_blue"
                        // },
                        // new DrawCardAnimationDto()
                        // {
                        //     PlayerId = "mock_jade",
                        //     DeckId = "Deck1",
                        //     CardId = "C_1_red"
                        // },
                        // new DrawCardAnimationDto()
                        // {
                        //     PlayerId = "mock_jade",
                        //     DeckId = "Deck1",
                        //     CardId = "C_2_red"
                        // },
                        // new DrawCardAnimationDto()
                        // {
                        //     PlayerId = "bob_id",
                        //     DeckId = "Deck1",
                        //     CardId = "C_4_red"
                        // },
                    },
                    Actions = new List<CommandDto>()
                    {
                        new DrawCardActionDto()
                        {
                            Id = 8,
                            PlayerId = "mock_jade",
                            DeckId = "Deck1",
                        },
                        new PlayCardActionDto()
                        {
                            Id = 9,
                            PlayerId = "mock_jade",
                            CardId = "C_1_red",
                        },
                        new PlayCardInsideZoneActionDto()
                        {
                            Id = 10,
                            PlayerId = "mock_jade",
                            CardId = "C_2_red",
                            ZoneId = "Zone__C",
                            CanDropAnywhere = true,
                        },
                    },
                    Input = null,
                    Descriptions = new Dictionary<string, string>()
                    {
                        { "Deck1",  "A DECK!" },
                        { "bob_id",  "this is bob" },
                    },
                    PlayersTakingTurn = new List<string>()
                    {
                        "mock_jade",
                    }
                });
            } 
            
            if (Input.GetKeyDown(KeyCode.B))
            { 
                await GameLogic.UpdateGame(new UpdateGamePayload()
                {
                    Animations = new List<CommandDto>()
                    {
                    },
                    Actions = new List<CommandDto>()
                    {
                    },
                    Input = null,
                    Descriptions = new Dictionary<string, string>()
                    {
                        { "Deck1",  "A DECK!" },
                        { "bob_id",  "this is bob" },
                    },
                    PlayersTakingTurn = new List<string>()
                    {
                        "bob_id",
                    }
                });
            }
            
            if (Input.GetKeyDown(KeyCode.N))
            {
                // _ = SignalBus.Send(new UpdateGameRequest(new UpdateGamePayload()
                // {
                //     Animations = new List<CommandDto>()
                //     {
                //         new PlayerTurnCommandDto()
                //         {
                //             PlayersTakingTurn = new List<string>()
                //             {
                //                 "Player_2"
                //             }
                //         },
                //     },
                //     Actions = new List<CommandDto>(),
                //     Inputs = new List<CommandDto>(),
                //     Descriptions = new List<DescriptionDto>(),
                //     PlayersTakingTurn = new List<string>()
                //     {
                //         "Player_2",
                //     }
                // }));
            } 
            
            if (Input.GetKeyDown(KeyCode.V))
            {
                Debug.Log("OnUpdateGameStateClicked!!! 2");

                // _ = SignalBus.Send(new UpdateGameRequest(new UpdateGamePayload()
                // {
                //     Animations = new List<CommandDto>()
                //     {
                //         new PlayCardInsideZoneAnimationDto()
                //         {
                //             PlayerId = "Player_2",
                //             ZoneId = "Zone1",
                //             CardId = "C_black"
                //         },
                //         new PlayerTurnCommandDto()
                //         {
                //             PlayersTakingTurn = new List<string>()
                //             {
                //                 "Player_0"
                //             }
                //         },
                //     },
                //     Actions = new List<CommandDto>()
                //     {
                //         // new PlayCardInsideZoneActionDto()
                //         // {
                //         //     Id = 10,
                //         //     PlayerId = "Player_1",
                //         //     ZoneId = "Zone1",
                //         //     CardId = "C_0_blue",
                //         //     GlowColor = "#3495eb"
                //         // },
                //     },
                //     Inputs = new List<CommandDto>()
                // }));
            }
            
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("END GAME");
                
                // SignalBus.Fire(new EndGameSignal(new EndGamePayload()
                // {
                //     EndCommand = new LeastCardInHandEndDto()
                //     {
                //         OrderedPlayerUsernames = new string[] { "mic", "bob" },
                //     
                //         PlayerUsernameToNumberOfCards = new Dictionary<string, int>() { { "mic", 1 }, { "bob", 2 } },
                //     }
                // }));
            }
        }
    }
}