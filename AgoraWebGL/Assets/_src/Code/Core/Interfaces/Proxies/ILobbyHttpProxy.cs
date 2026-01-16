using System.Threading.Tasks;
using _src.Code.Core.Actors;
using Agora.Core.Dtos.Lobby;

namespace _src.Code.Core.Interfaces.Proxies
{
    public interface ILobbyHttpProxy
    {
        Task<HttpResponseResult<LobbyDto>> JoinLobby(string lobbyId);
        Task<HttpResponseResult> LeaveLobby(string lobbyId);
    }
}