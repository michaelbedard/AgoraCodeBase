using _src.Code.Core.Interfaces.Proxies;
using _src.Code.Core.Interfaces.Services;

namespace _src.Code.Network.HttpProxies
{
    public class GameHttpProxy: BaseHttpProxy, IGameHttpProxy
    {
        private string URL => BaseUrl + "/game";
        
        protected GameHttpProxy(IVisualElementService visualElementService, IClientDataService clientDataService) 
            : base(visualElementService, clientDataService)
        {
        }
    }
}