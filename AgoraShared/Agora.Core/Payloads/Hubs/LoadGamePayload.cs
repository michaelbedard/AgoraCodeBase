using System.Numerics;
using Agora.Core.Dtos;
using Agora.Core.Dtos.Game.GameModules;

namespace Agora.Core.Payloads.Hubs;

public class LoadGamePayload
{
    public string Title { get; set; }
    public string EnvironmentAddress { get; set; }
    public string MusicAddress { get; set; }
    public string RulesAddress { get; set; }
    public List<GameModuleDto> GameModules { get; set; }
    public List<UserDto> Players { get; set; }
    public Dictionary<string, string> PlayerUsernameToGameModuleId { get; set; }
    public Dictionary<string, int> PlayerUsernameToSeat { get; set; }
}