using Agora.Core.Payloads.Hubs;

namespace Agora.Core.Contracts.Server;

public interface IGameServerContract
{
    // game
    Task ExecuteAction(ExecuteActionPayload executeActionPayload);
    Task ExecuteInput(ExecuteInputPayload executeInputPayload);
}