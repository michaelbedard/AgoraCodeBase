using System.Collections.Generic;

namespace _src.Code.Core.Interfaces.Services
{
    public interface IClientDataService 
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public int AvatarId { get; set; }
        public int Pronouns { get; set; }
        
        // audio
        public float MusicVolume { get; set; }
        public float SoundEffectVolume { get; set; }
        
        // lobby
        public string ChannelId { get; set; }
        public string LobbyId { get; set; }
        

        /// <summary>
        /// ResetProfileData
        /// </summary>
        public void ResetProfileData();
    }
}