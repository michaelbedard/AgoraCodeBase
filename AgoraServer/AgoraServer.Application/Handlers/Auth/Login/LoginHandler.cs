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
        var runtimeUser = new RuntimeUser
        {
            Id = GenerateGuidFromDiscordId(discordUser.Id),
            Username = discordUser.Username,
        };

        // 4. Store in Session Service (So they are "Logged In")
        var sessionResult = _sessionService.AddSession(runtimeUser);
        
        if (!sessionResult.IsSuccess)
        {
            return Result<UserDto>.Failure("Failed to create session: " + sessionResult.Error);
        }

        // 5. Return success to the client
        return Result<UserDto>.Success(runtimeUser.ToUserDto());
    }
    
    // private
    private Guid GenerateGuidFromDiscordId(string discordId)
    {
        // Hashing the Discord ID to get a consistent GUID
        using (var md5 = System.Security.Cryptography.MD5.Create())
        {
            var hash = md5.ComputeHash(System.Text.Encoding.Default.GetBytes(discordId));
            return new Guid(hash);
        }
    }
}

// Validator using FluentValidationkk
public class LoginCommandValidator : AbstractValidator<LoginRequest>
{
    public LoginCommandValidator()
    {

    }
}