using _src.Code.Core.Interfaces.GameModules.Other;

namespace _src.Code.Core.Interfaces.Managers
{
    public interface IHandManager
    {
        IHand CreateHand(string playerId);
    }
}