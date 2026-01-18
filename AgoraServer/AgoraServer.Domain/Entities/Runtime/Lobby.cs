

using Agora.Engine;

namespace Domain.Entities.Runtime
{
    public class Lobby
    {
        public string Id { get; private set; }
        public GameEngine GameEngine { get; set; }
        public List<RuntimeUser> Players { get; set; }
        public string GameId { get; set; }
        public bool GameIsRunning { get; set; }

        public Lobby(string id)
        {
            Id = id;
            Players = new List<RuntimeUser>();
        }
    }
}