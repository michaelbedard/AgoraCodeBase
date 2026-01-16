using Agora.Core.Contracts;
using Agora.Core.Enums;
using Domain.Entities;
using Domain.Entities.Runtime;

namespace Domain.Interfaces.Repositories;

public interface IConnectionRepository
{
    bool AddConnection(string connectionId, RuntimeUser user);
    RuntimeUser? GetUserByConnectionId(string connectionId);
    bool RemoveConnection(string connectionId);

    bool AddUserToGroup(string groupId, RuntimeUser user);
    bool RemoveUserFromGroup(string groupId, RuntimeUser user);
    List<RuntimeUser> GetUsersByGroup(string groupId);
}