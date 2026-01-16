using Agora.Core.Actors;
using Agora.Core.Dtos;

namespace Application.Handlers.Auth.Login;

public class LoginRequest : BaseRequest<Result<UserDto>>
{
    public string OAuthCode { get; set; }
}
