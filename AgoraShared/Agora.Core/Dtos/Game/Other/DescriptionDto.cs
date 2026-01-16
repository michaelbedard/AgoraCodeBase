
using Newtonsoft.Json;

namespace Agora.Core.Dtos.Game.Other;

public class DescriptionDto
{
    [JsonProperty(Order = 1)] 
    public string GameModuleId { get; set; }

    [JsonProperty(Order = 0)]
    public string Description { get; set; }
}