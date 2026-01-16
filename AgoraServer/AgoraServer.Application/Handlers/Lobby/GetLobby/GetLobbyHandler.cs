using Agora.Core.Actors;
using Agora.Core.Dtos.Lobby;
using Domain.Interfaces.Services;
using Domain.Mappers;
using MediatR;

namespace Application.Handlers.Lobby.GetLobby;

public class GetLobbyHandler : IRequestHandler<GetLobbyQuery, Result<LobbyDto>>
{
    private readonly ILobbyService _lobbyService;

    public GetLobbyHandler(ILobbyService lobbyService)
    {
        _lobbyService = lobbyService;
    }
    
    public async Task<Result<LobbyDto>> Handle(GetLobbyQuery query, CancellationToken cancellationToken)
    {
        var gameResult = _lobbyService.GetLobby(query.Id);
        if (!gameResult.IsSuccess)
        {
            return Result<LobbyDto>.Failure(gameResult.Error);
        }

        return Result<LobbyDto>.Success(gameResult.Value.ToLobbyDto());
    }
}