using Newtonsoft.Json;

namespace Agora.Core.Dtos.Game.Commands;

public class CommandDto
{
    [JsonProperty(Order = 2)] 
    public int Id { get; set; } = -1;
    
    [JsonProperty(Order = 1)] 
    public string Key { get; set; }
    
    [JsonProperty(Order = 0)]
    public string? PlayerId { get; set; }
}