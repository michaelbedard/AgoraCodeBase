using System.Threading.Tasks;
using Agora.Core.Payloads.Http.Lobby;
using Agora.Core.Payloads.Hubs;

namespace _src.Code.Core.Interfaces.Proxies
{
    public interface IHubProxy
    {
        Task<bool> ConnectAsync();

        Task SelectGame(SelectGamePayload payload);
        Task LaunchGame(LaunchGamePayload payload);
        Task ExecuteAction(ExecuteActionPayload payload);
        Task ExecuteInput(ExecuteInputPayload payload);
    }
}