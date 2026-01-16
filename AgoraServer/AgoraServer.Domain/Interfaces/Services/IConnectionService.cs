using Domain.Entities.Runtime;

namespace Domain.Interfaces.Services;

public interface IConnectionService
{
    // Connection Tracking
    void RegisterConnection(string connectionId, RuntimeUser user);
    void UnregisterConnection(string connectionId);
    RuntimeUser? GetUserByConnectionId(string connectionId);

    // Group Tracking
    void AddUserToGroup(string groupId, RuntimeUser user);
    void RemoveUserFromGroup(string groupId, RuntimeUser user);
    List<RuntimeUser> GetUsersByGroup(string groupId);
}