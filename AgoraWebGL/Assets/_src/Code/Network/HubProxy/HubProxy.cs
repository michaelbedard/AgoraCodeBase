using System.Threading.Tasks;
using System.Linq.Expressions;
using System;
using _src.Code.Core;
using _src.Code.Core.Interfaces.Proxies;
using _src.Code.Core.Interfaces.Services;
using _src.Code.Network.Controllers;
using Agora.Core.Contracts.Server;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using Zenject;
using UnityEngine;
using Microsoft.Extensions.DependencyInjection;

namespace _src.Code.Network.HubProxies
{
    // Use the Union Interface we discussed earlier
    public partial class HubProxy : IHubProxy
    {
        private TypedHubConnection<IServerContract> _typedConnection;
        private HubConnection _rawConnection;
        
        // Dependencies
        private readonly IClientDataService _clientDataService;
        private readonly IVisualElementService _visualElementService;
        
        // INJECT THE CONTROLLER
        private readonly IHubController _hubController; 

        [Inject]
        public HubProxy(
            IClientDataService clientDataService,
            IVisualElementService visualElementService,
            IHubController hubController)
        {
            _clientDataService = clientDataService;
            _visualElementService = visualElementService;
            _hubController = hubController;
        }

        public async Task<bool> ConnectAsync()
        {
            var userId = _clientDataService.Id;
            if (string.IsNullOrEmpty(userId)) return false;

            // 1. Dispose old connection if exists
            if (_rawConnection != null) await _rawConnection.DisposeAsync();

            // 2. Build URL with Query Param
            var fullUrl = $"{Globals.Instance.ServerUrl}/hub?userId={Uri.EscapeDataString(userId)}";

            // 3. Create Connection
            _rawConnection = new HubConnectionBuilder()
                .WithUrl(fullUrl, options => {
                    // options.Transports = HttpTransportType.WebSockets; 
                    options.SkipNegotiation = true; 
                })
                .AddNewtonsoftJsonProtocol()
                .ConfigureLogging(logging =>
                {
                    logging.SetMinimumLevel(LogLevel.Debug);
                    logging.AddProvider(new UnityLoggerProvider());
                })
                .WithAutomaticReconnect()
                .Build();

            // 4. Setup Outgoing (Proxy)
            _typedConnection = new TypedHubConnection<IServerContract>(_rawConnection, _visualElementService);

            // 5. Setup Incoming (Controller) - CRITICAL STEP
            _hubController.InitializeListeners(_rawConnection);

            // 6. Connect
            try 
            {
                await _rawConnection.StartAsync();
                Debug.Log("[HubProxy] Connected to Hub.");
                return true;
            }
            catch (Exception ex)
            {
                Debug.LogError($"[HubProxy] Connection failed: {ex.Message}");
                return false;
            }
        }
        
        // This is the helper method the partial files will call
        protected async Task InvokeAsync(Expression<Func<IServerContract, Task>> expression)
        {
            if (_rawConnection.State != HubConnectionState.Connected)
            {
                Debug.LogWarning("Cannot invoke hub method: Client not connected.");
                // Optionally try to reconnect here
                return;
            }
            
            await _typedConnection.InvokeAsync(expression);
        }
    }
}