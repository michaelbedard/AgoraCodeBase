using System.Reflection;
using Newtonsoft.Json;

namespace Agora.Core.Dtos.Game.Commands;

public class CommandDto
{
    [JsonProperty(Order = 2)] 
    public int Id { get; set; } = -1;
    
    [JsonProperty(Order = 1)] 
    public string Key => GetType().Name;
    
    [JsonProperty(Order = 0)]
    public string? PlayerId { get; set; }
    
    
    /// <summary>
    /// Checks if the other object is the same type and has equal values for all properties.
    /// </summary>
    public bool IsEqual(CommandDto? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;

        // 3. Check if they are the exact same Runtime Type
        Type type = this.GetType();
        if (type != other.GetType()) return false;

        // 4. Iterate over ALL properties (Parent + Child)
        PropertyInfo[] properties = type.GetProperties();

        foreach (PropertyInfo property in properties)
        {
            if (property.Name == nameof(Id)) 
            {
                continue; 
            }
            
            var value1 = property.GetValue(this);
            var value2 = property.GetValue(other);

            // Compare values
            if (!object.Equals(value1, value2))
            {
                return false;
            }
        }

        return true;
    }
}