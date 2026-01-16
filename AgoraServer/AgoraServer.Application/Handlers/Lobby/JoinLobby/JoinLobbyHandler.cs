using Agora.Core.Actors;
using Agora.Core.Dtos.Lobby;
using Domain.Interfaces.Services;
using Domain.Mappers;

namespace Application.Handlers.Lobby.JoinLobby;

public class JoinLobbyHandler : BaseHandler<JoinLobbyRequest, Result<LobbyDto>>
{
    private readonly ILobbyService _lobbyService;

    public JoinLobbyHandler(
        ILobbyService lobbyService)
    {
        _lobbyService = lobbyService;
    }
    
    public override async Task<Result<LobbyDto>> Handle(JoinLobbyRequest request, CancellationToken cancellationToken)
    {
        var user = request.User;
        
        ///////////////////////////////////////////////////////////////////////////
        // Edge Cases
        ///////////////////////////////////////////////////////////////////////////

        if (user.Lobby != null)
        {
            if (user.Lobby.Id == request.LobbyId)
            {
                return Result<LobbyDto>.Success(user.Lobby.ToLobbyDto());
            }
            else
            {
                // disconnect
                var leaveResult = await _lobbyService.LeavePlayerAsync(user.Lobby.Id, user);
                if (!leaveResult.IsSuccess)
                {
                    return Result<LobbyDto>.Failure(leaveResult.Error);
                }
            }
        }
        
        ///////////////////////////////////////////////////////////////////////////
        // Logic
        ///////////////////////////////////////////////////////////////////////////
        
        // create lobby if doesn't exist
        if (!_lobbyService.LobbyExist(request.LobbyId))
        {
            var newLobby = new Domain.Entities.Runtime.Lobby(request.LobbyId);
            var registerResult = _lobbyService.RegisterLobby(newLobby);
            if (!registerResult.IsSuccess)
            {
                return Result<LobbyDto>.Failure(registerResult.Error);
            }
        }
        
        // join lobby
        var joinResult = await _lobbyService.JoinPlayerAsync(request.LobbyId, user);
        if (!joinResult.IsSuccess)
        {
            return Result<LobbyDto>.Failure(joinResult.Error);
        }

        return Result<LobbyDto>.Success(joinResult.Value.ToLobbyDto());
    }
}