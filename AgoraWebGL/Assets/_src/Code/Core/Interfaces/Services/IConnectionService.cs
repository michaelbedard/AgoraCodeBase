using System.Threading.Tasks;

namespace _src.Code.Core.Interfaces.Services
{
    public interface IConnectionService
    {
        Task ConnectAsync(string username);
    }
}