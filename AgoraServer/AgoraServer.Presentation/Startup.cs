using Domain.Entities.Runtime;
using Domain.Interfaces.Services;

namespace Presentation;

public class Startup
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<Startup> _logger;
    private readonly ISessionService _sessionService;

    public Startup(IHttpClientFactory httpClientFactory, ILogger<Startup> logger, ISessionService sessionService)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
        _sessionService = sessionService;
    }

    public void OnStartedAsync()
    {
        _logger.LogInformation("Server has started");
        
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        if (environment == "Development")
        {
            // create dummies client for testing
            CreateRuntimeClient("00000000-0000-0000-0000-000000000001", "Alice");
            CreateRuntimeClient("00000000-0000-0000-0000-000000000002", "Bob");
            CreateRuntimeClient("00000000-0000-0000-0000-000000000003", "Charlie");
            CreateRuntimeClient("00000000-0000-0000-0000-000000000004", "David");
        }
    }

    private void CreateRuntimeClient(string userId, string username, int pronouns = 0)
    {
        var runtimeUser = new RuntimeUser
        {
            Id = Guid.Parse(userId),
            Username = username,
            Pronouns = pronouns
        };

        var sessionResult = _sessionService.AddSession(runtimeUser);
        if (sessionResult.IsSuccess)
        {
            _logger.LogInformation($"Created Dummy User: {username} ({userId})");
        }
        else
        {
            _logger.LogError($"Failed to create dummy user {username}: {sessionResult.Error}");
        }
    }
}