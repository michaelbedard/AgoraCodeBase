using System.Threading.Tasks;
using Agora.Core.Enums;

namespace _src.Code.Core.Interfaces.UI
{
    public interface ITableWindow
    {
        Task AddPlayer(string username, string pronouns, Language[] languages);
        Task RemovePlayer(string username);
    }
}