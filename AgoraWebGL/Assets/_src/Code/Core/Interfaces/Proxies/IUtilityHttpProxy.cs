using System.Threading.Tasks;
using _src.Code.Core.Actors;

namespace _src.Code.Core.Interfaces.Proxies
{
    public interface IUtilityHttpProxy
    {
        Task<HttpResponseResult<string>> GetVersion();
    }
}