using System;
using System.Threading.Tasks;
using _src.Code.Core.Utility;
using Agora.Core.Contracts;
using Agora.Core.Contracts.Client;
using Agora.Core.Payloads.Hubs;
using Microsoft.AspNetCore.SignalR.Client;

namespace _src.Code.Network.Controllers
{
    public partial class HubController : IGameClientContract
    {
        partial void RegisterGameListeners(HubConnection connection)
        {
            connection.On<LoadGamePayload>(nameof(LoadGame), LoadGame);
            connection.On(nameof(StartGame), StartGame);
            connection.On<UpdateGamePayload>(nameof(UpdateGame), UpdateGame);
            connection.On<EndGamePayload>(nameof(EndGame), EndGame);
        }

        public async Task LoadGame(LoadGamePayload loadGamePayload)
        {
            await UnityMainThreadDispatcher.Instance().EnqueueAsync(() =>
            {
                // _signalBus is accessible here because it is protected in the main file
                // _ = _signalBus.Send(new LoadSceneAndGameRequest(loadGamePayload));
            });
        }

        public async Task StartGame()
        {
            await UnityMainThreadDispatcher.Instance().EnqueueAsync(() =>
            {
                // _ = _signalBus.Send(new StartGameRequest());
            });
        }

        public async Task UpdateGame(UpdateGamePayload updateGamePayload)
        {
            await UnityMainThreadDispatcher.Instance().EnqueueAsync(() =>
            {
                // _ = _signalBus.Send(new UpdateGameRequest(updateGamePayload));
            });
        }

        public Task EndGame(EndGamePayload endGamePayload)
        {
            throw new NotImplementedException();
        }
    }
}