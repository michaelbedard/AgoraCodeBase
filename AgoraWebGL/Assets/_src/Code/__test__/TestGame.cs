using System.Collections.Generic;
using _src.Code.Core.Actors;
using _src.Code.Core.Extensions;
using _src.Code.Core.Interfaces.Managers;
using _src.Code.Core.Interfaces.Services;
using _src.Code.UI.Scenes.Game;
using Agora.Core.Dtos;
using Agora.Core.Dtos.Game;
using Agora.Core.Dtos.Game.Commands;
using Agora.Core.Dtos.Game.Commands.Actions;
using Agora.Core.Dtos.Game.Commands.Inputs;
using Agora.Core.Dtos.Game.Commands.Other;
using Agora.Core.Dtos.Game.GameModules;
using Agora.Core.Dtos.Game.Other;
using Agora.Core.Enums;
using Agora.Core.Payloads.Hubs;
using UnityEngine;
using Zenject;
using Position = Agora.Core.Actors.Position;
using Random = System.Random;
using Vector3 = System.Numerics.Vector3;

namespace _src.Code.__test__
{
    public class TestGame : BaseTest
    {
        private ISceneService _sceneService;
        private IVisualElementService _visualElementService;

        private LoadGamePayload loadGameDto = new LoadGamePayload()
        {
            Title = "ExampleGame",
            EnvironmentAddress = "Environments/Outdoor.prefab",
            MusicAddress = "Assets/_src/Audio/439_Goodhaven.mp3",
            PlayerUsernameToSeat = new Dictionary<string, int>()
            {
                { "bob", 0 },
                { "Cedric", 1 },
                { "mic", 2 },
            },
            PlayerUsernameToGameModuleId = new Dictionary<string, string>()
            {
                { "bob", "Player_0" },
                { "Cedric", "Player_1" },
                { "mic", "Player_2" },
            },
            GameModules = new List<GameModuleDto>()
            {
                // Decks
                new DeckDto()
                {
                    Id = "Deck1",
                    Name = "Drawing Deck",
                    Type = GameModuleType.Deck,
                    Color = "#6E260E",
                    Position = new Position()
                    {
                        X = 1.3f,
                        Y = 0
                    },
                    TopImage = "Assets/_src/Games/Uno/back.png",
                },

                // Zone
                new ZoneDto()
                {
                    Id = "Zone1",
                    Name = "Playing Zone",
                    Type = GameModuleType.Zone,
                    Position = new Position()
                    {
                        X = -1.3f,
                        Y = 0,
                    },
                    Width = 2,
                    Height = 3,
                    StackingMethod = ZoneStackingMethod.Diagonal
                },
                
                new ZoneDto()
                {
                    Id = "Zone2",
                    Name = "Playing Zone",
                    Type = GameModuleType.Zone,
                    Position = new Position()
                    {
                        X = -1.3f,
                        Y = -2,
                    },
                    Width = 2,
                    Height = 3
                },

                // Cards
                new CardDto()
                {
                    Id = "C_0_blue",
                    Name = "0 Blue",
                    Type = GameModuleType.Card,
                    FrontImage = "Assets/_src/Games/Uno/blue/0_blue.png",
                    BackImage = "Assets/_src/Games/Uno/back.png"
                },
                new CardDto()
                {
                    Id = "C_7_yellow",
                    Type = GameModuleType.Card,
                    FrontImage = "Assets/_src/Games/Uno/yellow/7_yellow.png",
                    BackImage = "Assets/_src/Games/Uno/back.png"
                },
                new CardDto()
                {
                    Id = "C_5_blue",
                    Type = GameModuleType.Card,
                    FrontImage = "Assets/_src/Games/Uno/blue/5_blue.png",
                    BackImage = "Assets/_src/Games/Uno/back.png"
                },
                new CardDto()
                {
                    Id = "C_0_red",
                    Type = GameModuleType.Card,
                    FrontImage = "Assets/_src/Games/Uno/red/0_red.png",
                    BackImage = "Assets/_src/Games/Uno/back.png"
                },
                new CardDto()
                {
                    Id = "C_1_red",
                    Type = GameModuleType.Card,
                    FrontImage = "Assets/_src/Games/Uno/red/1_red.png",
                    BackImage = "Assets/_src/Games/Uno/back.png"
                },
                new CardDto()
                {
                    Id = "C_2_red",
                    Type = GameModuleType.Card,
                    FrontImage = "Assets/_src/Games/Uno/red/2_red.png",
                    BackImage = "Assets/_src/Games/Uno/back.png"
                },
                new CardDto()
                {
                    Id = "C_3_red",
                    Type = GameModuleType.Card,
                    FrontImage = "Assets/_src/Games/Uno/red/3_red.png",
                    BackImage = "Assets/_src/Games/Uno/back.png"
                },
                new CardDto()
                {
                    Id = "C_4_red",
                    Type = GameModuleType.Card,
                    FrontImage = "Assets/_src/Games/Uno/red/4_red.png",
                    BackImage = "Assets/_src/Games/Uno/back.png"
                },
                // Tokens
                // new TokenDto()
                // {
                //     Id = "Token1",
                //     Type = GameModuleType.Token,
                //     StartZoneId = "Zone1",
                // },
                // new TokenDto()
                // {
                //     Id = "Token2",
                //     Type = GameModuleType.Token,
                //     StartZoneId = "Zone1",
                // },
                // new TokenDto()
                // {
                //     Id = "Token3",
                //     Type = GameModuleType.Token,
                //     StartZoneId = "Zone1",
                // },
                // new TokenDto()
                // {
                //     Id = "Token4",
                //     Type = GameModuleType.Token,
                //     StartZoneId = "Zone1",
                // },
                // new TokenDto()
                // {
                //     Id = "Token5",
                //     Type = GameModuleType.Token,
                //     StartZoneId = "Zone1",
                // },
                // new TokenDto()
                // {
                //     Id = "Token6",
                //     Type = GameModuleType.Token,
                //     StartZoneId = "Zone1",
                // },
                // new TokenDto()
                // {
                //     Id = "Token7",
                //     Type = GameModuleType.Token,
                //     StartZoneId = "Zone1",
                // },
                // new TokenDto()
                // {
                //     Id = "Token8",
                //     Type = GameModuleType.Token,
                //     StartZoneId = "Zone1",
                // },
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
                Debug.Log("LOAD GAME");
            }
            
            if (Input.GetKeyDown(KeyCode.X))
            {
                var p = await _visualElementService.GetOrCreate<PlayerTurnPopup>();
                p.Hide();
            }
            
            if (Input.GetKeyDown(KeyCode.S))
            {
                Debug.Log("START GAME");
            }
            
            if (Input.GetKeyDown(KeyCode.R))
            {
                ServiceLocator.GetService<IInputManager>().MouseOverUI = false;
            }

            if (Input.GetKeyDown(KeyCode.U))
            {
                Debug.Log("OnUpdateGameStateClicked!!!");

                // _ = SignalBus.Send(new UpdateGameRequest(new UpdateGamePayload()
                // {
                //     Animations = new List<CommandDto>()
                //     {
                //         new FlipDeckTopCardAnimationDto()
                //         {
                //             PlayerId = null,
                //             DeckId = "Deck1",
                //             CardId = "C_0_blue",
                //             ZoneId = "Zone1"
                //         },
                //         new FlipDeckTopCardAnimationDto()
                //         {
                //             PlayerId = null,
                //             DeckId = "Deck1",
                //             CardId = "C_7_yellow",
                //             ZoneId = "Zone1"
                //         },
                //         new ReturnCardToDeckAnimationDto()
                //         {
                //             PlayerId = null,
                //             DeckId = "Deck1",
                //             CardId = "C_7_yellow",
                //         },
                //         new ReturnCardToDeckAnimationDto()
                //         {
                //             PlayerId = null,
                //             DeckId = "Deck1",
                //             CardId = "C_0_blue",
                //         },
                //         new FlipDeckTopCardAnimationDto()
                //         {
                //             PlayerId = null,
                //             DeckId = "Deck1",
                //             CardId = "C_3_red",
                //             ZoneId = "Zone1"
                //         },
                //         // new DrawCardAnimationDto()
                //         // {
                //         //     PlayerId = "Player_2",
                //         //     DeckId = "Deck1",
                //         //     CardId = "C_0_blue"
                //         // },
                //         // new TransferTokenAnimationDto()
                //         // {
                //         //     PlayerId = null,
                //         //     TokenId = "Token8",
                //         //     ZoneId = "Zone2"
                //         // },
                //         // new DrawCardAnimationDto()
                //         // {
                //         //     PlayerId = "Player_2",
                //         //     DeckId = "Deck1",
                //         //     CardId = "C_7_yellow"
                //         // },
                //         // new DrawCardAnimationDto()
                //         // {
                //         //     PlayerId = "Player_2",
                //         //     DeckId = "Deck1",
                //         //     CardId = "C_0_red"
                //         // },
                //         // new DrawCardAnimationDto()
                //         // {
                //         //     PlayerId = "Player_2",
                //         //     DeckId = "Deck1",
                //         //     CardId = "C_5_blue"
                //         // },
                //         // new DrawCardAnimationDto()
                //         // {
                //         //     PlayerId = "Player_2",
                //         //     DeckId = "Deck1",
                //         //     CardId = "C_1_red"
                //         // },
                //         // new DrawCardAnimationDto()
                //         // {
                //         //     PlayerId = "Player_2",
                //         //     DeckId = "Deck1",
                //         //     CardId = "C_2_red"
                //         // },
                //         // new DrawCardAnimationDto()
                //         // {
                //         //     PlayerId = "Player_2",
                //         //     DeckId = "Deck1",
                //         //     CardId = "C_3_red"
                //         // },
                //         // new DrawCardAnimationDto()
                //         // {
                //         //     PlayerId = "Player_2",
                //         //     DeckId = "Deck1",
                //         //     CardId = "C_4_red"
                //         // },
                //         // new LeastCardInHandEndDto()
                //         // {
                //         //     OrderedPlayerUsernames = new string[] { "mic", "bob" },
                //         //     PlayerUsernameToNumberOfCards = new Dictionary<string, int>() { { "mic", 1 }, { "bob", 2 } },
                //         // },
                //         // new FlipDeckTopCardAnimationDto()
                //         // {
                //         //     PlayerId = null,
                //         //     DeckId = "Deck1",
                //         //     ZoneId = "Zone1",
                //         //     CardId = "C_3_red"
                //         // }
                //     },
                //     Actions = new List<CommandDto>()
                //     {
                //         // new ActivateZoneActionDto()
                //         // {
                //         //     Id = 9,
                //         //     PlayerId = "Player_2",
                //         //     ZoneId = "Zone2"
                //         // }
                //         // new ActivateCardActionDto()
                //         // {
                //         //     Id = 10,
                //         //     PlayerId = "Player_2",
                //         //     CardId = "C_7_yellow",
                //         // },
                //         // new PlayCardInsideZoneActionDto()
                //         // {
                //         //     Id = 10,
                //         //     PlayerId = "Player_2",
                //         //     CardId = "C_7_yellow",
                //         //     ZoneId = "Zone1",
                //         //     CanDropAnywhere = true,
                //         // },
                //     },
                //     Inputs = new List<CommandDto>()
                //     {
                //         new ButtonInputDto()
                //         {
                //             Id = 10,
                //             PlayerId = "Player_2",
                //             Text = "GGG",
                //             ImageAddress = "Assets/_src/Games/Uno/red/2_red.png"
                //         },
                //         
                //         // new SelectAmountDto()
                //         // {
                //         //     Id = 10,
                //         //     Label = "Who much?",
                //         //     Min = 5,
                //         //     Max = 10,
                //         //     PlayerId = "Player_2",
                //         //     
                //         // },
                //         // new ChoiceInputDto()
                //         // {
                //         //     Id = 2,
                //         //     Label = "Choose color!",
                //         //     Choices = new ChoiceDto[]
                //         //     {
                //         //         new TextChoiceDto()
                //         //         {
                //         //             Text = "AAA",
                //         //             CardBackgroundAsset = "Assets/_src/Games/Uno/red/4_red.png"
                //         //         },
                //         //         new TextChoiceDto()
                //         //         {
                //         //             Text = "bbb",
                //         //             CardBackgroundAsset = "Assets/_src/Games/Uno/red/4_red.png"
                //         //         },
                //         //         new TextChoiceDto()
                //         //         {
                //         //             Text = "ccc",
                //         //             CardBackgroundAsset = "Assets/_src/Games/Uno/red/4_red.png"
                //         //         },
                //         //     }
                //         // }
                //     },
                //     Descriptions = new List<DescriptionDto>()
                //     {
                //         new DescriptionDto()
                //         {
                //             GameModuleId = "Deck1",
                //             Description = "DDD"
                //         },
                //         new DescriptionDto()
                //         {
                //             GameModuleId = "Zone1",
                //             Description = "ZZZ"
                //         },
                //         new DescriptionDto()
                //         {
                //             GameModuleId = "Player_1",
                //             Description = "Player_1"
                //         },
                //         new DescriptionDto()
                //         {
                //             GameModuleId = "Player_2",
                //             Description = "Player_2"
                //         },
                //         new DescriptionDto()
                //         {
                //             GameModuleId = "Player_0",
                //             Description = "Player_0"
                //         },
                //     },
                //     PlayersTakingTurn = new List<string>()
                //     {
                //         "Player_1",
                //     }
                // }));
            } 
            
            if (Input.GetKeyDown(KeyCode.B))
            {
                // _ = SignalBus.Send(new UpdateGameRequest(new UpdateGamePayload()
                // {
                //     Animations = new List<CommandDto>()
                //     {
                //         new PlayerTurnCommandDto()
                //         {
                //             PlayersTakingTurn = new List<string>()
                //             {
                //                 "Player_0"
                //             }
                //         },
                //     },
                //     Actions = new List<CommandDto>(),
                //     Inputs = new List<CommandDto>(),
                //     Descriptions = new List<DescriptionDto>(),
                //     PlayersTakingTurn = new List<string>()
                //     {
                //         "Player_0",
                //     }
                // }));
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