using Agora.Core.Actors;
using Domain.Entities;

namespace Domain.Interfaces.Services;

public interface IDiscordAuthService
{
    Task<Result<string>> ExchangeCodeForAccessTokenAsync(string code);
    Task<Result<DiscordUser>> GetUserAsync(string accessToken);
}