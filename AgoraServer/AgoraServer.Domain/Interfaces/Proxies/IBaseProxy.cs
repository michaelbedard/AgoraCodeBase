using Domain.Entities.Runtime;

namespace Domain.Interfaces.Proxies;

public interface IBaseProxy
{
    Task SendError(string connectionId, string errorMessage);
    Task AddUserToGroupAsync(RuntimeUser user, string groupId);
    Task RemoveUserFromGroupAsync(RuntimeUser user, string groupId);
}