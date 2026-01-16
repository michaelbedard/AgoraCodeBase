using System.Threading.Tasks;
using Agora.Core.Payloads.Hubs;

namespace _src.Code.Network.HubProxies
{
    public partial class HubProxy
    {
        public async Task ExecuteAction(ExecuteActionPayload executedActionPayload)
        {
            await InvokeAsync(e => e.ExecuteAction(executedActionPayload));
        }

        public async Task ResolveInput(ExecuteInputPayload executeInputPayload)
        {
            await InvokeAsync(e => e.ExecuteInput(executeInputPayload));
        }
    }
}