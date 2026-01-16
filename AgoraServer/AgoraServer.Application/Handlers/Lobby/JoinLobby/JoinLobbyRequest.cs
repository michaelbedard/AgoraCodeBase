using Agora.Core.Actors;
using Agora.Core.Dtos.Lobby;

namespace Application.Handlers.Lobby.JoinLobby;

public class JoinLobbyRequest : BaseRequest<Result<LobbyDto>>
{
    public string LobbyId { get; set; }
}