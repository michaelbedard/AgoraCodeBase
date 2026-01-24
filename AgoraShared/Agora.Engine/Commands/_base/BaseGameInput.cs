using Newtonsoft.Json.Linq;

namespace Agora.Engine.Commands._base;

public abstract class BaseGameInput<TPayload, TResult>(Player player) : BaseCommand(player), IGameInput
{
    public TResult? Result { get; protected set; }

    protected abstract GameActionResult Execute(TPayload payload);
    

    public GameActionResult Execute(object payload)
    {
        if (payload == null)
            throw new InvalidOperationException($"Input {GetType().Name} requires payload of type {typeof(TPayload).Name}, but got null.");

        TPayload typedPayload;
        try 
        {
            // Handle JObject (Newtonsoft) or Direct Casting
            if (payload is JObject jObj)
                typedPayload = jObj.ToObject<TPayload>();
            else
                typedPayload = (TPayload)payload;
        }
        catch (Exception ex)
        {
            throw new ArgumentException($"Invalid Payload. Expected {typeof(TPayload).Name}. Error: {ex.Message}");
        }

        return Execute(typedPayload);
    }
}