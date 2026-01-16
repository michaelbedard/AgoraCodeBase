using Agora.Core.Actors;
using Agora.Core.Dtos.Lobby;

namespace Application.Handlers.Lobby.GetLobby;

public class GetLobbyQuery : BaseRequest<Result<LobbyDto>>
{
    public string Id { get; set; }
}