using System.Collections.Generic;
using System.Threading.Tasks;
using _src.Code.Core.Actors;
using _src.Code.Core.Interfaces.GameModules;
using _src.Code.Core.Interfaces.GameModules.Other;
using Agora.Core.Dtos.Game.GameModules;
using Agora.Core.Enums;

namespace _src.Code.Core.Interfaces.Services
{
    public interface IGameModuleService
    {
        IBaseGameModule GetGameModuleById(string id);
        T GetGameModuleById<T>(string id) where T : IBaseGameModule;
        IBaseGameModule GetGameModuleByInputObject(InputObject inputObject);
        List<IBaseGameModule> GetAllGameModules();
        List<T> GetAllGameModules<T>() where T : IBaseGameModule;
        void DeleteGameModule(string id);
        Task<IBaseGameModule> InstantiateGameModuleAsync(GameModuleDto loadData, GameModuleType moduleType);
        Task<T> InstantiateGameModuleAsync<T>(GameModuleDto loadData) where T : IBaseGameModule;
        T InstantiateGameModule<T>() where T : IBaseGameModule;
    }
}