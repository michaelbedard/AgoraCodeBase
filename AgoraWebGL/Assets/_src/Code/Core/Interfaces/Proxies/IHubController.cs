using _src.Code.Core.Interfaces.Logic;
using Agora.Core.Contracts.Client;
using Microsoft.AspNetCore.SignalR.Client;

namespace _src.Code.Core.Interfaces.Proxies
{
    public interface IHubController : IClientContract
    {
        void RegisterEntryLogic(IEntryLogic logic);
        void RegisterGameLogic(IGameLogic logic);
        void InitializeListeners(HubConnection connection);
    }
}