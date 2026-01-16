using System.Collections.Concurrent;
using Agora.Core.Actors;
using Domain.Entities.Runtime;
using Domain.Interfaces.Services;

namespace Application.Services;

public class SessionService : ISessionService
{
    private readonly ConcurrentDictionary<Guid, RuntimeUser> _users = new();

    public Result AddSession(RuntimeUser user)
    {
        if (_users.TryAdd(user.Id, user))
        {
            return Result.Success();
        }
        return Result.Failure($"User {user.Username} already exists in memory.");
    }

    public Result RemoveSession(Guid userId)
    {
        if (_users.TryRemove(userId, out _))
        {
            return Result.Success();
        }
        return Result.Failure("User not found.");
    }

    public Result<RuntimeUser> GetSessionByUsername(string username)
    {
        // For high performance with 10k+ users, maintain a second Dictionary<string, Guid>
        var user = _users.Values.FirstOrDefault(u => u.Username == username);
        
        if (user == null)
        {
            return Result<RuntimeUser>.Failure($"User '{username}' not found.");
        }
        return Result<RuntimeUser>.Success(user);
    }

    public Result<RuntimeUser> GetSessionById(Guid userId)
    {
        if (_users.TryGetValue(userId, out var user))
        {
            return Result<RuntimeUser>.Success(user);
        }
        return Result<RuntimeUser>.Failure($"User ID {userId} not found.");
    }

    public bool SessionExist(Guid userId)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<RuntimeUser> GetAllSessions()
    {
        return _users.Values;
    }
}