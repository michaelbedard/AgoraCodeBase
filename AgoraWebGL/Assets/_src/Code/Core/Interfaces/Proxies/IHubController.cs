using _src.Code.Core.Interfaces.Handlers;
using Agora.Core.Contracts.Client;
using Microsoft.AspNetCore.SignalR.Client;

namespace _src.Code.Core.Interfaces.Proxies
{
    public interface IHubController : IClientContract
    {
        void RegisterEntryLogic(IEntryLogic logic);
        void InitializeListeners(HubConnection connection);
    }
}