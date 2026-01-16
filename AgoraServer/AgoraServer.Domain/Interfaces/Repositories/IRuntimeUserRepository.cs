using Domain.Entities;
using Domain.Entities.Runtime;

namespace Domain.Interfaces.Repositories;

public interface IRuntimeUserRepository
{
    bool AddUser(RuntimeUser user);
    RuntimeUser GetUserById(Guid guid);
    RuntimeUser GetUserByUsername(string username);
    bool RemoveUser(Guid guid);
    bool UserExists(Guid guid);
    IEnumerable<RuntimeUser> GetAllUsers();
    bool UpdateUser(RuntimeUser updatedUser);
}