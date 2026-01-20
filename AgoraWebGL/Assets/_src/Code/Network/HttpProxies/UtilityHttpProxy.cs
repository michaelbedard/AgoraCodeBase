using System.Threading.Tasks;
using _src.Code.Core.Actors;
using _src.Code.Core.Interfaces.Proxies;
using _src.Code.Core.Interfaces.Services;

namespace _src.Code.Network.HttpProxies
{
    public class UtilityHttpProxy : BaseHttpProxy, IUtilityHttpProxy
    {
        private string ServiceUrl => BaseUrl + "/Utility";
        
        protected UtilityHttpProxy(IVisualElementService visualElementService, IClientDataService clientDataService) 
            : base(visualElementService, clientDataService)
        {
        }

        public async Task<HttpResponseResult<string>> GetVersion()
        {
            var url = $"{ServiceUrl}/version";
            return await SendHttpRequest<string>(url, "GET", null);
        }
    }
}