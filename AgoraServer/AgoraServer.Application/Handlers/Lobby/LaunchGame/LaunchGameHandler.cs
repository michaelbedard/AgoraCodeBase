using Agora.Core.Actors;
using Domain.Interfaces.Proxies;
using Domain.Interfaces.Services;

namespace Application.Handlers.Lobby.LaunchGame;

public class LaunchGameHandler : BaseHandler<LaunchGameRequest, Result>
{
    private readonly IGameService _gameService;
    private readonly ILobbyService _lobbyService;
    private readonly ILobbyProxy _lobbyProxy;

    public LaunchGameHandler(IGameService gameService, ILobbyService lobbyService, ILobbyProxy lobbyProxy)
    {
        _gameService = gameService;
        _lobbyService = lobbyService;
        _lobbyProxy = lobbyProxy;
    }

    public override async Task<Result> Handle(LaunchGameRequest request, CancellationToken cancellationToken)
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
        var loadGameResult = await _gameService.LoadGameAsync(user.Lobby);
        if (!loadGameResult.IsSuccess)
        {
            return Result.Failure($"Game could not be launched: {loadGameResult.Error}");
        }
        
        return Result.Success();
    }
}