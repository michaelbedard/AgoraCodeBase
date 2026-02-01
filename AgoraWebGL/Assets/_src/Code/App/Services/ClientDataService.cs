using System.Collections.Generic;
using _src.Code.Core.Interfaces.GameModules;
using _src.Code.Core.Interfaces.GameModules.Other;
using _src.Code.Core.Interfaces.Services;
using _src.Code.UI.Common;
using Agora.Core.Dtos;
using Agora.Core.Enums;
using Agora.Core.Payloads.Hubs;
using Zenject;

namespace _src.Code.App.Services
{
    public class ClientDataService : IClientDataService, IInitializable
    {
        public void Initialize()
        {
            // reset data
            #if !UNITY_EDITOR
                ResetProfileData();
                MusicVolume = 10f;
                SoundEffectVolume = 10f;
            #endif
        }

        public string Id { get; set; } = string.Empty;
        public bool IsLoggedIn { get; set; }
        public string Username { get; set; } = "Mic";
        public string Password { get; set; } = "123";
        public string Email { get; set; } = "";
        public int AvatarId { get; set; } = 2;
        public int Pronouns { get; set; } = 0;
        public string ChannelId { get; set; } = string.Empty;

        // audio
        public float MusicVolume { get; set; } = 0f;
        public float SoundEffectVolume { get; set; } = 0f;

        // lobby
        public string LobbyId { get; set; } = string.Empty;
        public List<UserDto> UsersInLobby { get; set; } = new();
        
        // Game
        public GameKey GameKey { get; set; }
        public string GameTitle { get; set; }
        public string RulesAddresses { get; set; }
        public List<UserDto> Players { get; set; } = new();
        public Dictionary<string, IHand> PlayerIdToHand { get; set; } = new();


        /// <summary>
        /// ResetProfileData
        /// </summary>
        /// <returns></returns>
        public void ResetProfileData()
        {
            IsLoggedIn = false;
            Username = string.Empty;
            Password = string.Empty;
            Email = string.Empty;
            AvatarId = 0;
            Pronouns = 0;
        }

        public void LoadGameData(LoadGamePayload payload)
        {
            GameTitle = payload.Title;
            RulesAddresses = payload.RulesAddress;
            Players = payload.Players;
        }
    }
}