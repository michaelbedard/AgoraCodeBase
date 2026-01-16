using Agora.Core.Actors;
using Domain.Entities.Runtime;

namespace Domain.Interfaces.Services;

public interface ISessionService
{
    // Lifecycle
    Result AddSession(RuntimeUser user);
    Result RemoveSession(Guid userId);

    // Lookup
    Result<RuntimeUser> GetSessionByUsername(string username);
    Result<RuntimeUser> GetSessionById(Guid userId);
    bool SessionExist(Guid userId);
    
    // Optional: Useful for debugging or admin panels
    IEnumerable<RuntimeUser> GetAllSessions();
}