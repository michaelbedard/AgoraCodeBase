using System;
using System.Threading.Tasks;
using _src.Code.Core.Utility;
using Agora.Core.Contracts.Client;
using Agora.Core.Payloads.Hubs;
using Microsoft.AspNetCore.SignalR.Client;

namespace _src.Code.Network.HubController
{
    public partial class HubController : IGameClientContract
    {
        partial void RegisterGameListeners(HubConnection connection)
        {
            connection.On<LoadGamePayload>(nameof(LoadGame), LoadGame);
            connection.On<UpdateGamePayload>(nameof(UpdateGame), UpdateGame);
            connection.On<EndGamePayload>(nameof(EndGame), EndGame);
        }

        public async Task LoadGame(LoadGamePayload payload)
        {
            await UnityMainThreadDispatcher.Instance().EnqueueAsync(() =>
            {
                _entryLogic.LaunchGame(payload);
            });
        }

        public async Task UpdateGame(UpdateGamePayload updateGamePayload)
        {
            await UnityMainThreadDispatcher.Instance().EnqueueAsync(() =>
            {
                _gameLogic.UpdateGame(updateGamePayload);
            });
        }

        public Task EndGame(EndGamePayload endGamePayload)
        {
            throw new NotImplementedException();
        }
    }
}