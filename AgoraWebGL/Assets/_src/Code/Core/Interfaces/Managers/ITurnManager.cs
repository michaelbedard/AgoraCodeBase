using System.Collections.Generic;
using System.Threading.Tasks;

namespace _src.Code.Core.Interfaces.Managers
{
    public interface ITurnManager
    {
        Task UpdateVisual(List<string> playersTakingTurnId);
    }
}