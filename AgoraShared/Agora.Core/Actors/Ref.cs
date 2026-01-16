namespace Agora.Core.Actors;

public class Ref
{
    public object? Value { get; set; }

    public Ref(object? value)
    {
        Value = value;
    }
}