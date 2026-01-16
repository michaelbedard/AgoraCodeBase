using Agora.Core.Actors;
using Domain.Entities.Runtime;
using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers.Other;

[ApiController]
[Route("[controller]")]
public abstract class BaseApiController : ControllerBase
{
    private ISessionService? _sessionService;
    private ILogger? _logger;
    
    // Lazy load services so you don't need super() constructors everywhere
    protected ISessionService SessionService => 
        _sessionService ??= HttpContext.RequestServices.GetRequiredService<ISessionService>();

    protected ILogger Logger => 
        _logger ??= HttpContext.RequestServices.GetRequiredService<ILogger<BaseApiController>>();

    /// <summary>
    /// Tries to get the user from the ?userId= query parameter.
    /// Logs a warning and returns Failure if missing or not found.
    /// </summary>
    protected Result<RuntimeUser> GetCurrentUser()
    {
        // 1. Extract from Query String
        string? userIdString = HttpContext.Request.Query["userId"].ToString();

        if (string.IsNullOrEmpty(userIdString))
        {
            Logger.LogWarning($"[{HttpContext.Request.Method} {HttpContext.Request.Path}] Missing 'userId' query parameter.");
            return Result<RuntimeUser>.Failure("Missing 'userId' query parameter.");
        }
        
        Guid userId;

        // ---------------------------------------------------------
        // DEVELOPMENT HACK: Allow "1" to "9" for easier Postman testing
        // ---------------------------------------------------------
        
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        if (environment == "Development" && userIdString.Length == 1 && userIdString[0] >= '1' && userIdString[0] <= '9')
        {
            // Manually construct the GUID: 0000...0001
            string devGuid = $"00000000-0000-0000-0000-00000000000{userIdString}";
            userId = Guid.Parse(devGuid);
        }
        else
        {
            // Parse GUID normally
            if (!Guid.TryParse(userIdString, out userId))
            {
                Logger.LogWarning($"[{HttpContext.Request.Method} {HttpContext.Request.Path}] Invalid GUID format: '{userIdString}'.");
                return Result<RuntimeUser>.Failure($"Invalid 'userId' format. Expected a GUID.");
            }
        }

        // 3. Lookup in Memory
        var userResult = SessionService.GetSessionById(userId);

        if (!userResult.IsSuccess)
        {
            Logger.LogWarning($"[{HttpContext.Request.Method} {HttpContext.Request.Path}] User '{userId}' not found in runtime.");
            return Result<RuntimeUser>.Failure($"User '{userId}' not found.");
        }
        
        return userResult;
    }
}