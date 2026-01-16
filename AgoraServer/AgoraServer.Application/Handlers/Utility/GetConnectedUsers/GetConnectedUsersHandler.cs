using Agora.Core.Actors;
using Agora.Core.Dtos;
using Domain.Interfaces.Services;
using Domain.Mappers;
using MediatR;

namespace Application.Handlers.Utility.GetAllSessions;

public class GetConnectedUsersHandler : IRequestHandler<GetConnectedUsersQuery, Result<List<UserDto>>>
{
    private readonly ISessionService _sessionService;

    public GetConnectedUsersHandler(ISessionService sessionService)
    {
        _sessionService = sessionService;
    }
    
    public async Task<Result<List<UserDto>>> Handle(GetConnectedUsersQuery request, CancellationToken cancellationToken)
    {
        return Result<List<UserDto>>.Success(_sessionService.GetAllSessions().Select(s => s.ToUserDto()).ToList());
    }
}