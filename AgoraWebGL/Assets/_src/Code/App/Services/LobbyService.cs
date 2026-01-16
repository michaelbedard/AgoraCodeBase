using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using _src.Code.Core.Interfaces.Services;
using Agora.Core.Dtos;
using Agora.Core.Dtos.Lobby;
using Zenject;

namespace _src.Code.App.Services
{
    public class LobbyService : ILobbyService, IInitializable{
        private readonly SignalBus _signalBus;

        [Inject]
        public LobbyService(SignalBus signalBus)
        {
            _signalBus = signalBus;
            
        }

        public void Initialize() // maybe call DisplayTables() in that
        {
        }

        public List<UserDto> PlayersOnFloor { get; set; }
        public bool IsPlayerInFloor(UserDto player)
        {
            throw new NotImplementedException();
        }

        public void JoinPlayer(UserDto player)
        {
            throw new NotImplementedException();
        }

        public void LeavePlayer(UserDto player)
        {
            throw new NotImplementedException();
        }

        public Task SetupLobby(LobbyDto lobby)
        {
            throw new NotImplementedException();
        }
    }
}

