using System.Threading.Tasks;

namespace _src.Code.Core.Interfaces.Factories.Other
{
    public interface IBaseFactory<TIModule, in TProps>
    {
        Task<TIModule> CreateAsync(TProps props);
        TIModule Create();
    }
}