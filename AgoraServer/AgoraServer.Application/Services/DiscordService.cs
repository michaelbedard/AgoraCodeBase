using System.Net.Http.Json;
using Agora.Core.Actors;
using Domain.Entities;
using Domain.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class DiscordAuthService : IDiscordAuthService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;
    private readonly ILogger<DiscordAuthService> _logger;

    public DiscordAuthService(HttpClient httpClient, IConfiguration config, ILogger<DiscordAuthService> logger)
    {
        _httpClient = httpClient;
        _config = config;
        _logger = logger;
    }

    public async Task<Result<string>> ExchangeCodeForAccessTokenAsync(string code)
    {
        try
        {
            var clientSecret = Environment.GetEnvironmentVariable("DISCORD_CLIENT_SECRET") ?? "";
            
            var payload = new Dictionary<string, string>
            {
                { "client_id", _config["Discord:ClientId"]! },
                { "client_secret", clientSecret },
                { "grant_type", "authorization_code" },
                { "code", code },
                { "redirect_uri", _config["Discord:RedirectUri"]! }
            };

            var response = await _httpClient.PostAsync("https://discord.com/api/oauth2/token", new FormUrlEncodedContent(payload));

            if (!response.IsSuccessStatusCode)
            {
                var errorBody = await response.Content.ReadAsStringAsync();
                _logger.LogError($"Discord Token Exchange Failed. Status: {response.StatusCode}. Response: {errorBody}");
                return Result<string>.Failure($"Discord Token Error: {response.StatusCode} - {errorBody}");
            }

            var json = await response.Content.ReadFromJsonAsync<DiscordTokenResponse>();
            
            if (json == null || string.IsNullOrEmpty(json.access_token))
            {
                _logger.LogError("Discord returned success but access_token was null.");
                return Result<string>.Failure("Failed to deserialize Discord token.");
            }

            return Result<string>.Success(json.access_token);
        }
        catch (HttpRequestException httpEx)
        {
            _logger.LogError(httpEx, "Network error connecting to Discord API.");
            return Result<string>.Failure("Network error contacting Discord.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error in ExchangeCodeForAccessTokenAsync");
            return Result<string>.Failure($"Internal Server Error: {ex.Message}");
        }
    }

    public async Task<Result<DiscordUser>> GetUserAsync(string accessToken)
    {
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "https://discord.com/api/users/@me");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var errorBody = await response.Content.ReadAsStringAsync();
                _logger.LogError($"Discord User Profile Failed. Status: {response.StatusCode}. Response: {errorBody}");
                return Result<DiscordUser>.Failure($"Failed to fetch Discord user profile. Status: {response.StatusCode}");
            }

            var user = await response.Content.ReadFromJsonAsync<DiscordUser>();
            
            if (user == null)
            {
                _logger.LogError("Discord returned success but user profile was null.");
                return Result<DiscordUser>.Failure("Failed to deserialize Discord user.");
            }

            return Result<DiscordUser>.Success(user);
        }
        catch (HttpRequestException httpEx)
        {
            _logger.LogError(httpEx, "Network error fetching user profile.");
            return Result<DiscordUser>.Failure("Network error fetching user profile.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error in GetUserAsync");
            return Result<DiscordUser>.Failure($"Internal Server Error: {ex.Message}");
        }
    }
    
    // Helper Classes for JSON Deserialization
    private class DiscordTokenResponse { public string access_token { get; set; } }
}