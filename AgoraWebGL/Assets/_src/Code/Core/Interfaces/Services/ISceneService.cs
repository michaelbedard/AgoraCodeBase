using System;
using System.Threading.Tasks;

namespace _src.Code.Core.Interfaces.Services
{
    public interface ISceneService
    {
        Task LoadEntryScreen(Func<Task> callback = null);
        Task LoadGameScene(Func<Task> callback = null, bool forceReload = false);
        Task LoadAdditiveScene();
    }
}