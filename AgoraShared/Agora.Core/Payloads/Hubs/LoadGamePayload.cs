using System.Numerics;
using Agora.Core.Dtos;
using Agora.Core.Dtos.Game.GameModules;

namespace Agora.Core.Payloads.Hubs;

public class LoadGamePayload
{
    public int Id { get; set; }
    public string Title { get; set; }
    public float HorizontalPlayerPositionOffset { get; set; }
    public float DiagonalPlayerPositionOffset { get; set; }
    public string EnvironmentAddress { get; set; }
    public string MusicAddress { get; set; }
    public string BoardAddress { get; set; }
    public string SimplifiedRulesAddress { get; set; }
    public string[] CompleteRulesAddresses { get; set; }
    public Vector3 InitialCameraPosition { get; set; }
    public List<GameModuleDto> GameModules { get; set; }
    public List<UserDto> Players { get; set; }
    public Dictionary<string, string> PlayerUsernameToGameModuleId { get; set; }
    public Dictionary<string, int> PlayerUsernameToSeat { get; set; }
}