using System.Net.Http.Json;
using Agora.Core.Actors;
using Domain.Entities;
using Domain.Interfaces.Services;
using Microsoft.Extensions.Configuration;

public class DiscordAuthService : IDiscordAuthService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;

    public DiscordAuthService(HttpClient httpClient, IConfiguration config)
    {
        _httpClient = httpClient;
        _config = config;
    }

    public async Task<Result<string>> ExchangeCodeForAccessTokenAsync(string code)
    {
        var payload = new Dictionary<string, string>
        {
            { "client_id", _config["Discord:ClientId"]! },
            { "client_secret", _config["Discord:ClientSecret"]! },
            { "grant_type", "authorization_code" },
            { "code", code },
             { "redirect_uri", _config["Discord:RedirectUri"]! } 
        };

        var response = await _httpClient.PostAsync("https://discord.com/api/oauth2/token", new FormUrlEncodedContent(payload));

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            return Result<string>.Failure($"Discord Token Error: {response.StatusCode} - {error}");
        }

        var json = await response.Content.ReadFromJsonAsync<DiscordTokenResponse>();
        return Result<string>.Success(json.access_token);
    }

    public async Task<Result<DiscordUser>> GetUserAsync(string accessToken)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "https://discord.com/api/users/@me");
        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

        var response = await _httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            return Result<DiscordUser>.Failure("Failed to fetch Discord user profile.");
        }

        var user = await response.Content.ReadFromJsonAsync<DiscordUser>();
        return Result<DiscordUser>.Success(user);
    }
    
    // Helper Classes for JSON Deserialization
    private class DiscordTokenResponse { public string access_token { get; set; } }
}