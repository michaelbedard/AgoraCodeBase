using System.Threading.Tasks;
using _src.Code.Core.Utility;
using Agora.Core.Contracts.Client;
using Agora.Core.Dtos;
using Agora.Core.Enums;
using Microsoft.AspNetCore.SignalR.Client;

namespace _src.Code.Network.HubController
{
    public partial class HubController : ILobbyClientContract
    {
        partial void RegisterLobbyListeners(HubConnection connection)
        {
            connection.On<UserDto>(nameof(UserJoinedLobby), UserJoinedLobby);
            connection.On<string>(nameof(UserLeavedLobby), UserLeavedLobby);
            connection.On<GameKey>(nameof(GameSelected), GameSelected);
        }

        public async Task UserJoinedLobby(UserDto userDto)
        {
            await UnityMainThreadDispatcher.Instance().EnqueueAsync(async () =>
            {
                await _entryLogic.PlayerJoinLobby(userDto);
            });
        }

        public async Task UserLeavedLobby(string userId)
        {
            await UnityMainThreadDispatcher.Instance().EnqueueAsync(async () =>
            {
                await _entryLogic.PlayerLeaveLobby(userId);
            });
        }
        
        public async Task GameSelected(GameKey gameKey)
        {
            await UnityMainThreadDispatcher.Instance().EnqueueAsync(async () =>
            {
                await _entryLogic.GameSelected(gameKey);
            });
        }
    }
}