using System.Collections.Concurrent;
using Agora.Core.Actors;
using Domain.Interfaces.Proxies;
using Domain.Interfaces.Services;
using MediatR;

namespace Application.Handlers.Auth.Logout;

public class LogoutHandler : IRequestHandler<LogoutRequest, Result>
{
    private static readonly ConcurrentDictionary<string, SemaphoreSlim> UserLocks = new();
    
    private readonly ISessionService _sessionService;
    private readonly ILobbyService _lobbyService;
    
    public LogoutHandler(
        ISessionService sessionService, 
        ILobbyService lobbyService)
    {
        _sessionService = sessionService;
        _lobbyService = lobbyService;
    }
    
    public async Task<Result> Handle(LogoutRequest request, CancellationToken cancellationToken)
    {
        var user = request.User;
        
        // Get or create a semaphore for the user
        var userLock = UserLocks.GetOrAdd(user.Username, _ => new SemaphoreSlim(1, 1));
        
        await userLock.WaitAsync(cancellationToken);
        try
        {
            ///////////////////////////////////////////////////////////////////////////
            // Edge Cases
            ///////////////////////////////////////////////////////////////////////////
            
            
            
            ///////////////////////////////////////////////////////////////////////////
            // Logic
            ///////////////////////////////////////////////////////////////////////////

            // leave lobby, if needed
            if (user.Lobby != null)
            {
                // stop game, if needed
                if (user.Lobby.GameIsRunning)
                {
                    
                }
                
                // leave lobby
                var leaveResult = await _lobbyService.LeavePlayerAsync(user.Lobby.Id, user);
                if (!leaveResult.IsSuccess)
                {
                    return Result.Failure(leaveResult.Error);
                }
            }
            
            // remove runtime user
            var removeSessionResult = _sessionService.RemoveSession(user.Id);
            if (!removeSessionResult.IsSuccess)
            {
                return Result.Failure(removeSessionResult.Error);
            }
            
            Console.WriteLine($"User {user.Username} has successfully logged out.");
        }
        finally
        {
            userLock.Release();
        }
        
        return Result.Success();
    }
}