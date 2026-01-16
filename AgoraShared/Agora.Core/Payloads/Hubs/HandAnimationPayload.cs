namespace Agora.Core.Payloads.Hubs;

public class HandAnimationPayload
{
    public string Username { get; set; } // only used on client
    public int CardStartIndex { get; set; }
    public int CardEndIndex { get; set; }
}