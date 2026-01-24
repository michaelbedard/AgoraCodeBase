using Agora.Core.Actors;
using Agora.Core.Enums;

namespace Application.Handlers.Lobby.LaunchGame;

public class LaunchGameRequest : BaseRequest<Result>
{
    public GameKey GameKey { get; set; }
}