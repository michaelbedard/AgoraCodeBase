using Agora.Core.Dtos;
using Domain.Entities.Runtime;

namespace Domain.Mappers;

public static class RuntimeUserMapper
{
    public static UserDto ToUserDto(this RuntimeUser runtimeUser)
    {
        if (runtimeUser == null) throw new ArgumentNullException(nameof(runtimeUser));

        return new UserDto()
        {
            Id = runtimeUser.Id.ToString(),
            Username = runtimeUser.Username,
            Avatar = runtimeUser.Avatar,
            Pronouns = runtimeUser.Pronouns,
            LobbyId = runtimeUser.Lobby?.Id
        };
    }
}