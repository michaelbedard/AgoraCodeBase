using System.Collections.Generic;
using Agora.Core.Payloads.Hubs;

namespace _src.Code.Core.Interfaces.Services
{
    public interface IGameDataService
    {
        public void LoadData(LoadGamePayload loadGameDto);
        public string PlayerId { get; set; }
        public int GameId { get; set; }
        public string GameTitle { get; set; }
        public string SimplifiedRulesAddress { get; set; }
        public string[] CompleteRulesAddresses { get; set; }
        public Dictionary<string, string> PlayerUsernameToGameModuleId { get; set; }
        public bool IsAgainstComputer { get; set; }
    }
}