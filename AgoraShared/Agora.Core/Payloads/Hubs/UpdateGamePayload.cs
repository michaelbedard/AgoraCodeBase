using Agora.Core.Dtos.Game.Commands;
using Agora.Core.Dtos.Game.Other;

namespace Agora.Core.Payloads.Hubs;

public class UpdateGamePayload
{
    public List<CommandDto> Animations { get; set; }
    public List<CommandDto> Actions { get; set; }
    public List<CommandDto> Inputs { get; set; }
    public List<DescriptionDto> Descriptions { get; set; }
    public List<string> PlayersTakingTurn { get; set; }
}