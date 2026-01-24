using Agora.Core.Actors;
using Application.Handlers.Lobby.LaunchGame;
using Domain.Interfaces.Proxies;
using Domain.Interfaces.Services;

namespace Application.Handlers.Lobby.SelectGame;

public class SelectGameHandler: BaseHandler<SelectGameRequest, Result>
{
    private readonly IGameService _gameService;
    private readonly ILobbyService _lobbyService;
    private readonly ILobbyProxy _lobbyProxy;

    public SelectGameHandler(IGameService gameService, ILobbyService lobbyService, ILobbyProxy lobbyProxy)
    {
        _gameService = gameService;
        _lobbyService = lobbyService;
        _lobbyProxy = lobbyProxy;
    }

    public override async Task<Result> Handle(SelectGameRequest request, CancellationToken cancellationToken)
    {
        var user = request.User;
        
        ///////////////////////////////////////////////////////////////////////////
        // Edge Cases
        ///////////////////////////////////////////////////////////////////////////
        
        if (user.Lobby == null)
        {
            return Result.Failure($"User {user.Username} not inside a lobby");
        }

        ///////////////////////////////////////////////////////////////////////////
        // Logic
        ///////////////////////////////////////////////////////////////////////////

        // load game
        user.Lobby.GameKey = request.GameKey;

        await _lobbyProxy.BroadcastGameSelection(user.Lobby.Id, request.GameKey);
        
        return Result.Success();
    }
}