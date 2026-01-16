using Agora.Core.Actors;
using Agora.Core.Dtos;
using Domain.Entities.Runtime;

namespace Application.Handlers.Utility.GetAllSessions;

public class GetConnectedUsersQuery : BaseRequest<Result<List<UserDto>>>
{
    
}