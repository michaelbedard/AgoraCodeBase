using System.Threading.Tasks;
using _src.Code.Core.Interfaces.GameModules;
using Agora.Core.Dtos.Game.GameModules;
using JetBrains.Annotations;

namespace _src.Code.Core.Interfaces.Factories
{
    public interface ICardFactory
    {
        Task<ICard> CreateAsync([CanBeNull] CardDto loadData);
        ICard Create();
    }
}