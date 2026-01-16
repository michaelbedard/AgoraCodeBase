using System.Threading.Tasks;

namespace _src.Code.Core.Interfaces.Proxies
{
    public interface IHubProxy
    {
        Task<bool> ConnectAsync();
    }
}