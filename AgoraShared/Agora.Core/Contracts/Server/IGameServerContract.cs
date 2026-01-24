using Agora.Core.Payloads.Http.Lobby;
using Agora.Core.Payloads.Hubs;

namespace Agora.Core.Contracts.Server;

public interface IGameServerContract
{
    // game
    Task SelectGame(SelectGamePayload payload);
    Task LaunchGame(LaunchGamePayload payload);
    Task ExecuteAction(ExecuteActionPayload executeActionPayload);
    Task ExecuteInput(ExecuteInputPayload executeInputPayload);
}