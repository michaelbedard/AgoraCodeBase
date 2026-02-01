using Agora.Core.Actors;
using Application.Handlers.Game.ReadyToStart;
using Domain.Interfaces.Services;

namespace Application.Handlers.Game.ExecuteInput;

public class ReadyToStartHandler : BaseHandler<ReadyToStartRequest, Result>
{
    private readonly IGameService _gameService;

    public ReadyToStartHandler(IGameService gameService)
    {
        _gameService = gameService;
    }
    
    public override async Task<Result> Handle(ReadyToStartRequest request, CancellationToken cancellationToken)
    {
        var user = request.User;
        
        ///////////////////////////////////////////////////////////////////////////
        // Edge Cases
        ///////////////////////////////////////////////////////////////////////////

        if (user.Lobby == null || user.Lobby.GameEngine == null)
        {
            return Result.Failure("XXX");
        }
        
        ///////////////////////////////////////////////////////////////////////////
        // Logic
        ///////////////////////////////////////////////////////////////////////////

        if (!user.Lobby.PlayersReady.Contains(user))
        {
            user.Lobby.PlayersReady.Add(user);
        }

        // everyone is ready
        if (user.Lobby.PlayersReady.Count == user.Lobby.Players.Count)
        {
            await _gameService.StartGameAsync(user.Lobby);
        }
        
        return Result.Success();
    }
}