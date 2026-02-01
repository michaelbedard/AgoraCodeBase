using System.Threading.Tasks;
using Agora.Core.Actors;
using Agora.Core.Payloads.Hubs;

namespace _src.Code.Core.Interfaces.Logic
{
    public interface IGameLogic
    {
        Task<Result> LoadGame(LoadGamePayload payload);
        Task<Result> UpdateGame(UpdateGamePayload payload);
    }
}