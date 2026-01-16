using System.Threading.Tasks;

namespace _src.Code.Core.Interfaces.Managers
{
    public interface IDescriptionManager
    {
        Task Start();
        void Stop();
    }
}