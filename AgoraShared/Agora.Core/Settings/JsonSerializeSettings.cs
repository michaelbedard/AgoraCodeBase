using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Agora.Core.Settings;

public static class JsonSerializationSettings
{
    /// <summary>
    /// DefaultSettings
    /// </summary>
    public static readonly JsonSerializerSettings DefaultSettings = new JsonSerializerSettings
    {
        TypeNameHandling = TypeNameHandling.Auto,
        NullValueHandling = NullValueHandling.Ignore,
        Formatting = Formatting.None,
    };
    
    /// <summary>
    /// JsonSerializerSettings
    /// </summary>
    public static readonly JsonSerializerSettings GameBuildSettings = new JsonSerializerSettings
    {
        TypeNameHandling = TypeNameHandling.Auto,
    };
    
    /// <summary>
    /// PrintAnySettings
    /// </summary>
    public static readonly JsonSerializerSettings PrintAnySettings = new JsonSerializerSettings
    {
        TypeNameHandling = TypeNameHandling.Auto,
        NullValueHandling = NullValueHandling.Include,
        Formatting = Formatting.Indented,
        ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver
        {
            DefaultMembersSearchFlags = System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance
        }
    };
    
    public static readonly JsonSerializerSettings PrintCommandSettings = new JsonSerializerSettings
    {
        TypeNameHandling = TypeNameHandling.Auto,
        NullValueHandling = NullValueHandling.Ignore,
        Formatting = Formatting.None,
        ContractResolver = new ReversePropertyOrderContractResolver()
    };
    
    private class ReversePropertyOrderContractResolver : DefaultContractResolver
    {
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            // Get the properties as usual
            var properties = base.CreateProperties(type, memberSerialization);

            // Reverse the order of properties
            return properties.Reverse().ToList();
        }
    }
}