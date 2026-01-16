using System.Threading.Tasks;
using System.Collections.Generic;
using Agora.Core.Dtos;
using Agora.Core.Dtos.Lobby;

namespace _src.Code.Core.Interfaces.Services
{
    public interface ILobbyService
    {
        List<UserDto> PlayersOnFloor {get;set;}
       
        // players
        public bool IsPlayerInFloor(UserDto player);
        public void JoinPlayer(UserDto player);
        public void LeavePlayer(UserDto player);
        
        // lobby
        public Task SetupLobby(LobbyDto lobby);
    }
}