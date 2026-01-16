namespace Agora.Core.Payloads.Hubs;

public class AvatarAnimationPayload
{
    public string Username { get; set; } // only used on client
    public string AnimationKey { get; set; }
}