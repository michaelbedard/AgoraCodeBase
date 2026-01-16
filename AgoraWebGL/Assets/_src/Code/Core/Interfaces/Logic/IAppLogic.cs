using System.Threading.Tasks;

namespace _src.Code.Core.Interfaces.Handlers
{
    public interface IAppLogic
    {
        Task Login(string channelId, string authCode);
        Task Logout();
    }
}