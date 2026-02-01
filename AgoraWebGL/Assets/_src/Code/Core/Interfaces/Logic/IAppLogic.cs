using System.Threading.Tasks;

namespace _src.Code.Core.Interfaces.Logic
{
    public interface IAppLogic
    {
        Task Login(string channelId, string authCode);
        Task Logout();
    }
}