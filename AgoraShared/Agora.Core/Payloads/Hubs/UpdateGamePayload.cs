using Agora.Core.Dtos.Game.Commands;
using Agora.Core.Dtos.Game.Other;

namespace Agora.Core.Payloads.Hubs;

public class UpdateGamePayload
{
    public List<CommandDto> Animations { get; set; }
    public List<CommandDto> Actions { get; set; }
    public CommandDto Input { get; set; }
    public Dictionary<string, string> Descriptions { get; set; }
    public List<string> PlayersTakingTurn { get; set; }
}