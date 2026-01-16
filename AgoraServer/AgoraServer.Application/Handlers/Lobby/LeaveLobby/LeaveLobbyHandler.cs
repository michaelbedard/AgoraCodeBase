using Agora.Core.Actors;
using Domain.Interfaces.Proxies;
using Domain.Interfaces.Services;
using Domain.Mappers;

namespace Application.Handlers.Lobby.LeaveLobby;

public class LeaveLobbyHandler : BaseHandler<LeaveLobbyRequest, Result>
{
    private readonly ILobbyService _lobbyService;
    private readonly ILobbyProxy _lobbyProxy;

    public LeaveLobbyHandler(
        ILobbyProxy lobbyProxy, 
        ILobbyService lobbyService)
    {
        _lobbyProxy = lobbyProxy;
        _lobbyService = lobbyService;
    }
    
    public override async Task<Result> Handle(LeaveLobbyRequest request, CancellationToken cancellationToken)
    {
        var user = request.User;
        
        ///////////////////////////////////////////////////////////////////////////
        // Edge Cases
        ///////////////////////////////////////////////////////////////////////////

        if (user.Lobby == null || user.Lobby.Id != request.LobbyId)
        {
            return Result.Failure("user in not currently inside this lobby");
        }
        
        ///////////////////////////////////////////////////////////////////////////
        // Logic
        ///////////////////////////////////////////////////////////////////////////

        var lobby = user.Lobby;
        
        // leave lobby
        var leaveResult = await _lobbyService.LeavePlayerAsync(request.LobbyId, user);
        if (!leaveResult.IsSuccess)
        {
            return Result.Failure(leaveResult.Error);
        }

        if (lobby.Players.Count < 1)
        {
            // delete lobby
            var deleteResult = _lobbyService.RemoveLobby(request.LobbyId);
            if (!deleteResult.IsSuccess)
            {
                return Result.Failure(deleteResult.Error);
            }
        }
        
        return Result.Success();
    }
}