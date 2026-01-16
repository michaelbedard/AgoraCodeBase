using Agora.Core.Actors;

namespace Application.Handlers.Lobby.LeaveLobby;

public class LeaveLobbyRequest : BaseRequest<Result>
{
    public string LobbyId { get; set; }
}