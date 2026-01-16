using System;
using _src.Code.Core.Interfaces.Handlers;
using _src.Code.Core.Interfaces.Proxies;
using _src.Code.Core.Interfaces.Services;
using UnityEngine.UIElements;
using Zenject;

namespace _src.Code.App.Logic.Entry
{
    public partial class EntryLogic : IEntryLogic, IInitializable, IDisposable
    {
        private readonly IVisualElementService _uiService;
        private readonly ILobbyHttpProxy _lobbyHttpProxy;
        private readonly IClientDataService _clientDataService;

        public EntryLogic(
            IVisualElementService uiService,
            ILobbyHttpProxy lobbyHttpProxy,
            IClientDataService clientDataService,
            IHubController hubController)
        {
            _uiService = uiService;
            _lobbyHttpProxy = lobbyHttpProxy;
            _clientDataService = clientDataService;
            
            // for now...
            hubController.RegisterEntryLogic(this);
        }
    }
}