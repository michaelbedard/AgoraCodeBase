using Agora.Core.Actors;
using Agora.Core.Enums;

namespace Application.Handlers.Lobby.SelectGame;

public class SelectGameRequest : BaseRequest<Result>
{
    public GameKey GameKey { get; set; }
}