using System.Threading.Tasks;
using Agora.Core.Contracts.Server;
using Agora.Core.Payloads.Http.Lobby;
using Agora.Core.Payloads.Hubs;

namespace _src.Code.Network.HubProxies
{
    public partial class HubProxy : IGameServerContract
    {
        public async Task SelectGame(SelectGamePayload payload)
        {
            await InvokeAsync(e => e.SelectGame(payload));
        }
        
        public async Task LaunchGame(LaunchGamePayload payload)
        {
            await InvokeAsync(e => e.LaunchGame(payload));
        }
        
        public async Task ExecuteAction(ExecuteActionPayload payload)
        {
            await InvokeAsync(e => e.ExecuteAction(payload));
        }

        public async Task ExecuteInput(ExecuteInputPayload payload)
        {
            await InvokeAsync(e => e.ExecuteInput(payload));
        }
    }
}