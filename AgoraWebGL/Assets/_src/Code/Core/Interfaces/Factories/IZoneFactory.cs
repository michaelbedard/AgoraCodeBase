using System.Threading.Tasks;
using _src.Code.Core.Interfaces.GameModules;
using Agora.Core.Dtos.Game.GameModules;

namespace _src.Code.Core.Interfaces.Factories
{
    public interface IZoneFactory
    {
        Task<IZone> CreateAsync(ZoneDto loadData);
        IZone Create();
    }
}