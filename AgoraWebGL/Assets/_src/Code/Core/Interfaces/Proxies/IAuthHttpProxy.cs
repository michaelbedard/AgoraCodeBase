using System.Threading.Tasks;
using _src.Code.Core.Actors;
using Agora.Core.Dtos;
using Agora.Core.Payloads.Http.Auth;

namespace _src.Code.Core.Interfaces.Proxies
{
    public interface IAuthHttpProxy
    {
        Task<HttpResponseResult<UserDto>> Login(LoginPayload loginPayload);
        Task<HttpResponseResult<UserDto>> Logout();
    }
}