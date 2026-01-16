using System.Text.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

/*
 * This is used by SignalR when transmitting data over a network
 *
 * IMPORTANT : do not use derived type as root object, it will type information
 */

namespace Agora.Core.Settings;

public class JsonPolymorphicConverter<T> : System.Text.Json.Serialization.JsonConverter<T>
{
    public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // Read the JSON string
        using (var jsonDocument = JsonDocument.ParseValue(ref reader))
        {
            var jsonString = jsonDocument.RootElement.GetRawText();

            // Deserialize the JSON string using Json.NET, which handles the $type property
            var settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
                // SerializationBinder = new CustomSerializationBinder()
            };

            return JsonConvert.DeserializeObject<T>(jsonString, settings);
        }
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        var settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto,
            // SerializationBinder = new CustomSerializationBinder()
        };
        
        var serialized = JsonConvert.SerializeObject(value, settings);
        writer.WriteRawValue(serialized);
    }
    
    public class CustomSerializationBinder : DefaultSerializationBinder
    {
        public override Type BindToType(string assemblyName, string typeName)
        {
            // Attempt to resolve the type dynamically
            try
            {
                // Use Type.GetType to dynamically resolve the type by its name
                var resolvedType = Type.GetType($"{typeName}, {assemblyName}");
                if (resolvedType != null)
                    return resolvedType;

                // Fallback to attempt resolution for generic types
                if (typeName.Contains("System.Collections.Generic"))
                {
                    // Handle dictionaries or lists generically
                    if (typeName.StartsWith("System.Collections.Generic.Dictionary"))
                    {
                        var genericArgs = GetGenericArguments(typeName);
                        return typeof(Dictionary<,>).MakeGenericType(genericArgs);
                    }
                    else if (typeName.StartsWith("System.Collections.Generic.List"))
                    {
                        var genericArgs = GetGenericArguments(typeName);
                        return typeof(List<>).MakeGenericType(genericArgs);
                    }
                }
            }
            catch
            {
                // Ignore errors and fallback to default behavior
            }

            return base.BindToType(assemblyName, typeName);
        }

        private static Type[] GetGenericArguments(string typeName)
        {
            // Extract generic arguments from the type name
            var startIndex = typeName.IndexOf("[[") + 2;
            var endIndex = typeName.LastIndexOf("]]");
            var genericArgs = typeName.Substring(startIndex, endIndex - startIndex)
                .Split(new[] { "],[" }, StringSplitOptions.None)
                .Select(arg =>
                {
                    var parts = arg.Split(',');
                    return Type.GetType($"{parts[0]}, {parts[1]}");
                })
                .ToArray();
            return genericArgs;
        }
    }
}