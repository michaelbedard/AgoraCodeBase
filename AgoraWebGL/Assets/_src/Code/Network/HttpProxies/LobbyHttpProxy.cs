using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using _src.Code.Core.Actors;
using _src.Code.Core.Interfaces.Proxies;
using _src.Code.Core.Interfaces.Services;
using Agora.Core.Dtos;
using Agora.Core.Dtos.Lobby;
using UnityEngine.Networking;

namespace _src.Code.Network.HttpProxies
{
    public class LobbyHttpProxy : BaseHttpProxy, ILobbyHttpProxy
    {
        private string ServiceUrl => BaseUrl + "/lobby";
        
        protected LobbyHttpProxy(IVisualElementService visualElementService, IClientDataService clientDataService) 
            : base(visualElementService, clientDataService)
        {
        }

        /// <summary>
        /// Joins a lobby. Expects the server to return the Lobby state (T).
        /// </summary>
        public async Task<HttpResponseResult<LobbyDto>> JoinLobby(string lobbyId)
        {
            var url = $"{ServiceUrl}/join";
            
            return await SendHttpRequest<LobbyDto>(url, UnityWebRequest.kHttpVerbPOST, lobbyId);
        }

        /// <summary>
        /// Leaves a lobby. Expects a boolean or simple success message.
        /// </summary>
        public async Task<HttpResponseResult> LeaveLobby(string lobbyId)
        {
            var url = $"{ServiceUrl}/leave";
            
            return await SendHttpRequest<bool>(url, UnityWebRequest.kHttpVerbPOST, lobbyId);
        }
    }
}