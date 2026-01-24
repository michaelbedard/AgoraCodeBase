using System.Threading.Tasks;
using Agora.Core.Actors;
using Agora.Core.Dtos;
using Agora.Core.Enums;

namespace _src.Code.Core.Interfaces.Handlers
{
    public interface IEntryLogic
    {
        Task<Result> PlayerJoinLobby(UserDto player);
        Task<Result> PlayerLeaveLobby(string userId);
        Task<Result> GameSelected(GameKey gameKey);
    }
}