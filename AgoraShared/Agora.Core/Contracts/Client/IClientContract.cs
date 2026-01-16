namespace Agora.Core.Contracts.Client;

public interface IClientContract : IGameClientContract, ILobbyClientContract
{
    Task HandleError(string errorMessage);
}