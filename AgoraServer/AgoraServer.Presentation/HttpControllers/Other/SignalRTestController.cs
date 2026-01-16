using System.Collections.Concurrent;
using Agora.Core.Payloads.Http.Lobby;
using Agora.Core.Payloads.Hubs;
using Agora.Core.Settings;
using Application.Handlers.Auth.Login;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;

namespace Presentation.Controllers.Other;

[ApiController]
[Route("[controller]")]
public class SignalRTestController : ControllerBase
{
    private static readonly ConcurrentDictionary<int, HubConnection> Clients = new();
    
    private readonly IMediator _mediator;
    
    // Note: Ensure "mainHub" matches whatever you mapped in Program.cs (app.MapHub<MainHub>("/mainHub"))
    private const string HubName = "HubController";

    public SignalRTestController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    /// <summary>
    /// Game Actions
    /// </summary>
    #region Game Actions
    
    [HttpPost(nameof(MarkAsReadyToStartGame))]
    public async Task<IActionResult> MarkAsReadyToStartGame(int clientId)
    {
        return await SendMessage(clientId, nameof(MarkAsReadyToStartGame));
    }

    [HttpPost(nameof(BroadcastHandAnimation))]
    public async Task<IActionResult> BroadcastHandAnimation(int clientId, HandAnimationPayload data)
    {
        return await SendMessage(clientId, nameof(BroadcastHandAnimation), data);
    }
    
    [HttpPost(nameof(BroadcastAvatarAnimation))]
    public async Task<IActionResult> BroadcastAvatarAnimation(int clientId, AvatarAnimationPayload data)
    {
        return await SendMessage(clientId, nameof(BroadcastAvatarAnimation), data);
    }
        
    [HttpPost(nameof(ExecuteAction))]
    public async Task<IActionResult> ExecuteAction(int clientId, ExecuteActionPayload data)
    {
        return await SendMessage(clientId, nameof(ExecuteAction), data);
    }
    
    [HttpPost(nameof(ResolveInput))]
    public async Task<IActionResult> ResolveInput(int clientId, ExecuteInputPayload data)
    {
        return await SendMessage(clientId, nameof(ResolveInput), data);
    }
    
    #endregion
    
    /// <summary>
    /// Lobby Actions
    /// </summary>
    #region Lobby Actions
    
    [HttpPost(nameof(JoinLobby))]
    public async Task<IActionResult> JoinLobby(int clientId, JoinLobbyPayload data)
    {
        return await SendMessage(clientId, nameof(JoinLobby), data);
    }
    
    [HttpPost(nameof(LeaveLobby))]
    public async Task<IActionResult> LeaveLobby(int clientId, LeaveLobbyPayload data)
    {
        return await SendMessage(clientId, nameof(LeaveLobby), data);
    }
    
    #endregion
    
    /// <summary>
    /// CreateClient
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateClient(int clientId)
    {
        // 1. Login to get a valid user
        var command = new LoginRequest
        {
            OAuthCode = $"client_{clientId}",
        };
        
        var result = await _mediator.Send(command);
        if (!result.IsSuccess)
        {
            return BadRequest(result.Error);
        }

        var user = result.Value;
        
        // 2. Build the SINGLE connection
        var connection = BuildHubConnection(HubName, user.Username);
        
        try 
        {
            // 3. Start the connection
            await connection.StartAsync();
            Console.WriteLine($"SignalR client {user.Username} connected (ID: {connection.ConnectionId})");

            // 4. Store it
            Clients.AddOrUpdate(clientId, connection, (key, oldConnection) => 
            {
                oldConnection.DisposeAsync();
                return connection;
            });
            
            return Ok(JsonConvert.SerializeObject(user));
        }
        catch (Exception ex)
        {
            return BadRequest($"Failed to connect to SignalR: {ex.Message}");
        }
    }
    
    private HubConnection BuildHubConnection(string endpoint, string username)
    {
        var connection = new HubConnectionBuilder()
            .WithUrl($"http://localhost:5039/{endpoint}?username={username}")
            .WithAutomaticReconnect()
            .AddJsonProtocol(options => 
            { 
                options.PayloadSerializerOptions.Converters.Add(new PolymorphicConverterFactory()); 
            })
            .Build();

        connection.ServerTimeout = TimeSpan.FromMinutes(20);
        connection.HandshakeTimeout = TimeSpan.FromMinutes(20);
        return connection;
    }

    /// <summary>
    /// SendMessage (with data)
    /// </summary>
    private async Task<IActionResult> SendMessage(int clientId, string methodName, object message)
    {
        if (Clients.TryGetValue(clientId, out var connection))
        {
            if (connection.State != HubConnectionState.Connected)
            {
                return BadRequest($"Client {clientId} is not connected (State: {connection.State})");
            }

            await connection.InvokeAsync(methodName, message);
            return Ok($"Sent {methodName} from client {clientId}.");
        }
        
        return NotFound($"Client {clientId} not found.");
    }
    
    /// <summary>
    /// SendMessage (no data)
    /// </summary>
    private async Task<IActionResult> SendMessage(int clientId, string methodName)
    {
        if (Clients.TryGetValue(clientId, out var connection))
        {
            if (connection.State != HubConnectionState.Connected)
            {
                return BadRequest($"Client {clientId} is not connected (State: {connection.State})");
            }

            await connection.InvokeAsync(methodName);
            return Ok($"Sent {methodName} from client {clientId}.");
        }
        
        return NotFound($"Client {clientId} not found.");
    }
}