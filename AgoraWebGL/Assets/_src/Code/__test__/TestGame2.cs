using System.Collections.Generic;
using _src.Code.Core.Actors;
using _src.Code.Core.Extensions;
using Agora.Core.Actors;
using Agora.Core.Dtos;
using Agora.Core.Dtos.Game;
using Agora.Core.Dtos.Game.Commands;
using Agora.Core.Dtos.Game.Commands.Actions;
using Agora.Core.Dtos.Game.GameModules;
using Agora.Core.Dtos.Game.Other;
using Agora.Core.Enums;
using Agora.Core.Payloads.Hubs;
using UnityEngine;
using Zenject;
using Vector3 = System.Numerics.Vector3;

namespace _src.Code.__test__
{
    public class TestGame2 : BaseTest
    {
        [Inject]
        public void Construct()
        {
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                Debug.Log("LOAD GAME 2");

                // _ = SignalBus.Send(new LoadGameRequest(new LoadGamePayload()
                // {
                //     Title = "ExampleGame",
                //     DiagonalPlayerPositionOffset = 2f,
                //     HorizontalPlayerPositionOffset = 2f,
                //     EnvironmentAddress = "Assets/_src/Environments/OutdoorMedium.prefab",
                //     MusicAddress = "Assets/_src/Audio/439_Goodhaven.mp3",
                //     SimplifiedRulesAddress = "Assets/_src/Games/Uno/HowToPlayUno.jpg",
                //     CompleteRulesAddresses = new []{ "Assets/_src/Games/Uno/HowToPlayUno.jpg" },
                //     BoardAddress = "Assets/_src/Sprites/Games/Snake/SnakeAndLadderBoard.png",
                //     InitialCameraPosition = new Vector3(0, 11, -8),
                //     PlayerUsernameToSeat = new Dictionary<string, int>()
                //     {
                //         { "mic", 0 },
                //         { "bob", 1 },
                //         { "cedric", 2 },
                //         // { "david", 3 },
                //     },
                //     PlayerUsernameToGameModuleId = new Dictionary<string, string>()
                //     {
                //         { "mic", "Player_0" },
                //         { "bob", "Player_1" },
                //         { "cedric", "Player_2" },
                //         // { "david", "Player_3" },
                //     },
                //     GameModules = new List<GameModuleDto>()
                //     {
                //         // new PlayerDto()
                //         // {
                //         //     Id = "Player_3",
                //         //     Type = GameModuleType.Player,
                //         //     Position = new Position()
                //         //     {
                //         //         X = 0,
                //         //         Y = -6
                //         //     },
                //         //     IsPlayerModule = true,
                //         //     CharacterId = 0
                //         // },
                //         // Markers
                //         new MarkerDto()
                //         {
                //             Id = "Player_1_Marker",
                //             Type = GameModuleType.Marker,
                //             IsPlayerModule = false,
                //             StartZoneId = "Zone1",
                //             IdentifiableColor = "#ff4000"
                //         },
                //         new MarkerDto()
                //         {
                //             Id = "Player_2_Marker",
                //             Type = GameModuleType.Marker,
                //             IsPlayerModule = false,
                //             StartZoneId = "Zone1"
                //         },
                //         new MarkerDto()
                //         {
                //             Id = "Player_0_Marker",
                //             Type = GameModuleType.Marker,
                //             IsPlayerModule = true,
                //             StartZoneId = "Zone1",
                //             Position = null
                //         },
                //         // Zone
                //         new ZoneDto()
                //         {
                //             Id = "Zone1",
                //             Type = GameModuleType.Zone,
                //             Position = new Position()
                //             {
                //                 X = -0.78f,
                //                 Y = -3.88f
                //             },
                //             Height = 1.5f,
                //             Width = 1.5f
                //         },
                //         new ZoneDto()
                //         {
                //             Id = "Zone2",
                //             Type = GameModuleType.Zone,
                //             Position = new Position()
                //             {
                //                 X = 0.78f,
                //                 Y = -3.88f
                //             },
                //             Height = 1.56f,
                //             Width = 1.56f
                //         },
                //         new ZoneDto()
                //         {
                //             Id = "Zone3",
                //             Type = GameModuleType.Zone,
                //             Position = new Position()
                //             {
                //                 X = 2f,
                //                 Y = -3.88f
                //             },
                //             Height = 1.56f,
                //             Width = 1.56f
                //         },
                //         new ZoneDto()
                //         {
                //             Id = "Zone4",
                //             Type = GameModuleType.Zone,
                //             Position = new Position()
                //             {
                //                 X = 3.56f,
                //                 Y = -3.88f
                //             },
                //             Height = 1.56f,
                //             Width = 1.56f
                //         },
                //         
                //         // Dice
                //         new DiceDto()
                //         {
                //             Id = "Dice",
                //             Type = GameModuleType.Dice,
                //             Position = new Position()
                //             {
                //                 X = 2,
                //                 Y = 0
                //             },
                //             NumberOfSides = 6,
                //         },
                //     }
                // }));
            }
            
            if (Input.GetKeyDown(KeyCode.S))
            {
                Debug.Log("START GAME");
            }
            
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("END GAME");
                
                // _ = SignalBus.Send(new EndGameSignal(new EndGamePayload()
                // {
                //     // EndCommand = new LeastCardInHandEndDto()
                //     // {
                //     //     OrderedPlayerUsernames = new string[] { "mic", "bob", "cedric", "david" },
                //     //
                //     //     PlayerUsernameToNumberOfCards = new Dictionary<string, int>() { { "mic", 1 }, { "bob", 2 }, {"cedric", 9}, {"david", 3}},
                //     // }
                // }));
            }
        }
    }
}