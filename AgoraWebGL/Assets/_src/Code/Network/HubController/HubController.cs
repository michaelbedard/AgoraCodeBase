using System.Threading.Tasks;
using _src.Code.Core.Actors;
using _src.Code.Core.Interfaces.Logic;
using _src.Code.Core.Interfaces.Proxies;
using _src.Code.Core.Interfaces.Services;
using _src.Code.Core.Utility;
using _src.Code.UI.Shared;
using Microsoft.AspNetCore.SignalR.Client;
using UnityEngine;
using Zenject;

namespace _src.Code.Network.HubController
{
    public partial class HubController : IHubController
    {
        private IEntryLogic _entryLogic;
        private IGameLogic _gameLogic;

        // Partial hooks
        partial void RegisterGameListeners(HubConnection connection);
        partial void RegisterLobbyListeners(HubConnection connection);

        [Inject]
        public HubController()
        {
        }

        // CHANGE: Make this PUBLIC so HubProxy can call it
        public void InitializeListeners(HubConnection connection)
        {
            // Base listeners
            connection.On<string>(nameof(HandleError), HandleError);
            
            // Sub-feature listeners (Partial files)
            RegisterGameListeners(connection);
            RegisterLobbyListeners(connection);
        }
        
        // Register Logic Methods
        
        public void RegisterEntryLogic(IEntryLogic logic)
        {
            _entryLogic = logic;
        }
        
        public void RegisterGameLogic(IGameLogic logic)
        {
            _gameLogic = logic;
        }
        
        // Handle Error
        
        public async Task HandleError(string errorMessage)
        {
            await UnityMainThreadDispatcher.Instance().EnqueueAsync(async () =>
            {
                Debug.LogError($"[HubController] Hub Error: {errorMessage}");
                
                var popup = await ServiceLocator.GetService<IVisualElementService>().GetOrCreate<WarningPopup>();
                popup.Title.Label.text = "Server Error";
                popup.Message.text = errorMessage;
                popup.Show();
            });
        }
    }
}