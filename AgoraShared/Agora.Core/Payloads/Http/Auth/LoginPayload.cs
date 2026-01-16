namespace Agora.Core.Payloads.Http.Auth;

public class LoginPayload
{
    public string OAuthCode { get; set; }
    public string LobbyId { get; set; }
}