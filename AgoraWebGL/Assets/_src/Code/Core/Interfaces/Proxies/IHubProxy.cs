using System.Threading.Tasks;
using Agora.Core.Payloads.Http.Lobby;
using Agora.Core.Payloads.Hubs;

namespace _src.Code.Core.Interfaces.Proxies
{
    public interface IHubProxy : IGameHubProxy, ILobbyHubProxy
    {
        Task<bool> ConnectAsync();
    }
}