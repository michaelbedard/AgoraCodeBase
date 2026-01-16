using System.Collections.Concurrent;
using Domain.Entities.Runtime;
using Domain.Interfaces.Services;

// You'll need to create this interface

namespace Application.Services;

public class ConnectionService : IConnectionService
{
    // Map: ConnectionId -> User
    private readonly ConcurrentDictionary<string, RuntimeUser> _connections = new();

    // Map: GroupId -> List of Users
    private readonly ConcurrentDictionary<string, List<RuntimeUser>> _groups = new();

    public void RegisterConnection(string connectionId, RuntimeUser user)
    {
        _connections.TryAdd(connectionId, user);
    }

    public void UnregisterConnection(string connectionId)
    {
        _connections.TryRemove(connectionId, out _);
    }

    public RuntimeUser? GetUserByConnectionId(string connectionId)
    {
        _connections.TryGetValue(connectionId, out var user);
        return user;
    }

    public void AddUserToGroup(string groupId, RuntimeUser user)
    {
        var group = _groups.GetOrAdd(groupId, _ => new List<RuntimeUser>());
        lock (group)
        {
            if (!group.Contains(user))
            {
                group.Add(user);
            }
        }
    }

    public void RemoveUserFromGroup(string groupId, RuntimeUser user)
    {
        if (_groups.TryGetValue(groupId, out var group))
        {
            lock (group)
            {
                group.Remove(user);
            }
        }
    }

    public List<RuntimeUser> GetUsersByGroup(string groupId)
    {
        if (_groups.TryGetValue(groupId, out var group))
        {
            lock (group)
            {
                return new List<RuntimeUser>(group);
            }
        }
        return new List<RuntimeUser>();
    }
}