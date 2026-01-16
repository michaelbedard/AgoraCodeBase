using System.Threading.Tasks;
using _src.Code.Core.Actors;
using _src.Code.Core.Interfaces.Proxies;
using _src.Code.Core.Interfaces.Services;
using Agora.Core.Dtos;
using Agora.Core.Payloads.Http.Auth;

namespace _src.Code.Network.HttpProxies
{
    public class AuthHttpProxy : BaseHttpProxy, IAuthHttpProxy
    {
        private string URL => BaseUrl + "/auth";
        
        protected AuthHttpProxy(IVisualElementService visualElementService, IClientDataService clientDataService) 
            : base(visualElementService, clientDataService)
        {
        }
        
        public async Task<HttpResponseResult<UserDto>> Login(LoginPayload payload)
        {
            return await SendHttpRequest<UserDto>(URL + "/login", "POST", payload);
        }

        public async Task<HttpResponseResult<UserDto>> Logout()
        {
            return await SendHttpRequest<UserDto>(URL + "/logout", "POST", null);
        }
    }
}