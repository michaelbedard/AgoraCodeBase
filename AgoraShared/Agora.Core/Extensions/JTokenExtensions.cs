using System.Text;
using Agora.Core.Actors;
using Newtonsoft.Json.Linq;
using NJsonSchema;
using NJsonSchema.Generation;
using NJsonSchema.Validation;

namespace Agora.Core.Extensions;

public static class JTokenExtensions
{
    public static JArray AsValidArray(this JToken token)
    {
        if (token == null || token.Type != JTokenType.Array)
        {
            throw new ArgumentException("Expected a non-null JArray with values.");
        }
        return (JArray)token;
    }

    // /// <summary>
    // /// Attempts to retrieve the value associated with the specified key from the JToken.
    // /// Return converted value to the specified type
    // /// </summary>
    // public static Result<T> GetItem<T>(this JToken token, string key)
    // {
    //     var valueToken = token[key];
    //
    //     if (valueToken == null)
    //     {
    //         return Result<T>.Failure($"Missing required field: {key}");
    //     }
    //
    //     // Attempt to convert the token to the specified type.
    //     var value = valueToken.ToObject<T>();
    //
    //     return Result<T>.Success(value);
    // }
    //
    // /// <summary>
    // /// Attempts to retrieve the value associated with the specified key from the JToken. Return the JToken object
    // /// </summary>
    // public static Result<JToken> GetItem(this JToken token, string key)
    // {
    //     try
    //     {
    //         return Result<JToken>.Success(token[key]);
    //     }
    //     catch (Exception e)
    //     {
    //         return Result<JToken>.Failure($"JToken does not have key {key}: {e.Message}");
    //     }
    // }
    
    
    /// <summary>
    /// Retrieves the value associated with the specified key from the JToken.
    /// If the key is missing or the value is null, returns the provided default value.
    /// </summary>
    public static T GetItemOrDefault<T>(this JToken token, string key, T defaultValue)
    {
        var valueToken = token[key];

        if (valueToken == null || valueToken.Type == JTokenType.Null)
        {
            return defaultValue;
        }
        
        var schema = JsonSchema.FromType<T>(new JsonSchemaGeneratorSettings());
        var validator = new JsonSchemaValidator();
        var errors = validator.Validate(valueToken, schema);
        
        if (errors.Count > 0)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("Validation errors: ");
            foreach (var error in errors)
            {
                stringBuilder.AppendLine($"- {error.Path}: {error.Kind}: {valueToken}");
            }
            
            throw new InvalidOperationException($"Cannot parse token to type '{typeof(T).Name}'. Validation errors : \n{stringBuilder.ToString()}");
        }
        

        return valueToken.ToObject<T>() ?? throw new InvalidOperationException($"Cannot parse token to type '{typeof(T).Name}'. Token is : {valueToken}");
    }
    
    /// <summary>
    /// Retrieves the value associated with the specified key from the JToken and converts it to the specified type.
    /// Throws an exception if the key exists but the value is not convertible to the specified type.
    /// </summary>
    public static T GetItemOrThrow<T>(this JToken token, string key, bool canBeNull = false)
    {
        var valueToken = token[key];

        if (valueToken == null)
        {
            throw new KeyNotFoundException($"Key '{key}' not found in the provided JToken.");
        }

        if (valueToken.Type == JTokenType.Null)
        {
            if (canBeNull)
                return default;

            throw new InvalidOperationException($"Key '{key}' cannot be null : {token}");
        }

        return valueToken.ToObject<T>() ?? throw new InvalidOperationException($"Cannot parse token to type '{typeof(T).Name}' : {valueToken}");
    }
    
    /// <summary>
    /// Attempts to retrieve the value JToken with the specified key from the original JToken.
    /// If fails, throw an exception
    /// </summary>
    public static JToken GetItemOrThrow(this JToken token, string key)
    {
        var valueToken = token[key];

        if (valueToken == null)
        {
            throw new KeyNotFoundException($"Key '{key}' not found in the provided JToken.");
        }
        
        if (valueToken.Type == JTokenType.Null)
        {
            return valueToken;
        }
        
        try
        {
            return valueToken;
        }
        catch (Exception ex)
        {
            throw new InvalidCastException($"Failed to convert the value of '{key}' to type 'JToken'.", ex);
        }
    }
    
    /// <summary>
    /// Attempts to convert the given JToken to the specified type. 
    /// </summary>
    public static Result<T> TryGetValue<T>(this JToken valueToken)
    {
        try
        {
            var value = valueToken.ToObject<T>();
            return Result<T>.Success(value);
        }
        catch (InvalidCastException)
        {
            return Result<T>.Failure($"The token does not match the expected type {typeof(T).Name}.");
        }
        catch (Exception ex)
        {
            return Result<T>.Failure($"Failed to convert the token to type {typeof(T).Name}: {ex.Message}");
        }
    }
}
