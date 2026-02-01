using System.Threading.Tasks;
using Agora.Core.Actors;
using Agora.Core.Dtos;
using Agora.Core.Enums;
using Agora.Core.Payloads.Hubs;

namespace _src.Code.Core.Interfaces.Logic
{
    public interface IEntryLogic
    {
        Task<Result> PlayerJoinLobby(UserDto player);
        Task<Result> PlayerLeaveLobby(string userId);
        Task<Result> GameSelected(GameKey gameKey);
        Task<Result> LaunchGame(LoadGamePayload payload);
        Task DisplayAvatar();
    }
}