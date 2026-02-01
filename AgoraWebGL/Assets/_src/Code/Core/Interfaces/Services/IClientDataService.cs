using System.Collections.Generic;
using _src.Code.Core.Interfaces.GameModules.Other;
using Agora.Core.Dtos;
using Agora.Core.Enums;
using Agora.Core.Payloads.Hubs;

namespace _src.Code.Core.Interfaces.Services
{
    public interface IClientDataService 
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public int AvatarId { get; set; }
        public int Pronouns { get; set; }
        public string ChannelId { get; set; }
        
        // audio
        public float MusicVolume { get; set; }
        public float SoundEffectVolume { get; set; }
        
        // lobby
        public string LobbyId { get; set; }
        public List<UserDto> UsersInLobby { get; set; }

        // Game
        public GameKey GameKey { get; set; }
        public string GameTitle { get; set; }
        public string RulesAddresses { get; set; }
        public List<UserDto> Players { get; set; }
        public Dictionary<string, IHand> PlayerIdToHand { get; set; }

        /// <summary>
        /// Methods
        /// </summary>
        public void ResetProfileData();
        public void LoadGameData(LoadGamePayload loadGameDto);
    }
}