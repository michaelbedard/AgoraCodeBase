using System.Threading.Tasks;
using _src.Code.Core.Interfaces.GameModules;
using Agora.Core.Dtos.Game.GameModules;

namespace _src.Code.Core.Interfaces.Factories
{
    public interface ITokenFactory
    {
        Task<IToken> CreateAsync(TokenDto loadData);
        IToken Create();
    }
}