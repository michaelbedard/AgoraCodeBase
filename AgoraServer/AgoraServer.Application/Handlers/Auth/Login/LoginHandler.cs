using Agora.Core.Actors;
using Agora.Core.Dtos;
using Domain.Entities;
using Domain.Entities.Runtime;
using Domain.Interfaces.Services;
using Domain.Mappers;
using FluentValidation;
using MediatR;

namespace Application.Handlers.Auth.Login;

public class LoginHandler : IRequestHandler<LoginRequest, Result<UserDto>>
{
    private readonly ISessionService _sessionService;
    private readonly IDiscordAuthService _discordService;

    public LoginHandler(ISessionService sessionService, IDiscordAuthService discordService)
    {
        _sessionService = sessionService;
        _discordService = discordService;
    }

    public async Task<Result<UserDto>> Handle(LoginRequest request, CancellationToken cancellationToken)
    {
        DiscordUser discordUser;
        
        if (!string.IsNullOrEmpty(request.OAuthCode) && request.OAuthCode.StartsWith("mock_"))
        {
            var mockUsername = request.OAuthCode.Replace("mock_", ""); 
        
            discordUser = new DiscordUser()
            {
                Id = request.OAuthCode,
                Username = char.ToUpper(mockUsername[0]) + mockUsername.Substring(1),
                Discriminator = "0000"
            };
        }
        else
        {
            // 1. Get Access Token from Discord
            var tokenResult = await _discordService.ExchangeCodeForAccessTokenAsync(request.OAuthCode);
        
            if (!tokenResult.IsSuccess)
            {
                return Result<UserDto>.Failure($"OAuth Failed: {tokenResult.Error}");
            }

            // 2. Get User Profile from Discord
            var discordUserResult = await _discordService.GetUserAsync(tokenResult.Value);
        
            if (!discordUserResult.IsSuccess)
            {
                return Result<UserDto>.Failure($"Profile Fetch Failed: {discordUserResult.Error}");
            }

            discordUser = discordUserResult.Value;
        }

        // 3. Convert to your internal RuntimeUser
        var runtimeUser = new RuntimeUser(discordUser.Id, discordUser.Username)
        {
            ChannelId = request.ChannelId,
            Avatar = GetBestAvatar(request.ChannelId)
        };

        // 4. Store in Session Service (So they are "Logged In")
        var sessionResult = _sessionService.AddSession(runtimeUser);
        
        if (!sessionResult.IsSuccess)
        {
            return Result<UserDto>.Failure("Failed to create session: " + sessionResult.Error);
        }

        return Result<UserDto>.Success(runtimeUser.ToUserDto());
    }

    private int GetBestAvatar(string channelId)
    {
        var allChannelUsers = _sessionService.GetAllSessions().Where(u => u.ChannelId == channelId).ToList();

        int[] avatarCounts = new int[4]; // Initializes to [0, 0, 0, 0]

        foreach (var user in allChannelUsers)
        {
            if (user.Avatar >= 0 && user.Avatar < 4)
            {
                avatarCounts[user.Avatar]++;
            }
        }

        int bestAvatar = 0;
        int minCount = int.MaxValue;

        for (int i = 0; i < 4; i++)
        {
            if (avatarCounts[i] < minCount)
            {
                minCount = avatarCounts[i];
                bestAvatar = i;
            }
        }

        return bestAvatar;
    }
}

// Validator using FluentValidationkk
public class LoginCommandValidator : AbstractValidator<LoginRequest>
{
    public LoginCommandValidator()
    {

    }
}