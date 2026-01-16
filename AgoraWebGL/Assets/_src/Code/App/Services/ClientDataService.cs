using _src.Code.Core.Interfaces.Services;
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

        // audio
        public float MusicVolume { get; set; } = 0f;
        public float SoundEffectVolume { get; set; } = 0f;

        // lobby
        public string ChannelId { get; set; } = string.Empty;
        public string LobbyId { get; set; } = string.Empty;
        

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
    }
}