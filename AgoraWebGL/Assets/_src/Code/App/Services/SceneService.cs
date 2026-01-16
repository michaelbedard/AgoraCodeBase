using System;
using System.Threading.Tasks;
using _src.Code.App.Managers;
using _src.Code.Core;
using _src.Code.Core.Extensions;
using _src.Code.Core.Interfaces.Managers;
using _src.Code.Core.Interfaces.Services;
using _src.Code.Core.Signals.Other;
using _src.Code.Core.Utility;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace _src.Code.App.Services
{
    public class SceneService : ISceneService, IInitializable
    {
        private readonly SignalBus _signalBus;
        private readonly IInputManager _inputManager;

        [Inject]
        public SceneService(SignalBus signalBus, IInputManager inputManager)
        {
            _signalBus = signalBus;
            _inputManager = inputManager;
        }

        public void Initialize()
        {
        }

        /// <summary>
        /// LoadEntryScene
        /// </summary>
        public async Task LoadEntryScreen(Func<Task> callback = null)
        {
            await LoadScene("EntryScene", false, callback);
        }

        /// <summary>
        /// LoadGameScene
        /// </summary>
        public async Task LoadGameScene(Func<Task> callback = null, bool forceReload = false)
        {
            await LoadScene("GameScene", forceReload, callback);
        }

        /// <summary>
        /// LoadAdditiveScene
        /// </summary>
        public async Task LoadAdditiveScene()
        {
            await UnityMainThreadDispatcher.Instance().EnqueueAsync(async () =>
            {
                try
                {
                    if (!SceneManager.GetSceneByName("Additive").isLoaded)
                    {
                        var asyncLoad = SceneManager.LoadSceneAsync("Additive", LoadSceneMode.Additive);

                        while (!asyncLoad.isDone)
                        {
                            await Task.Yield();
                        }
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                    throw;
                }
            });
        }
        
        /// <summary>
        /// LoadScene (generic)
        /// </summary>
        private async Task LoadScene(string sceneName, bool forceReload, Func<Task> callback)
        {
            await UnityMainThreadDispatcher.Instance().EnqueueAsync(async () =>
            {
                try
                {
                    if (forceReload || SceneManager.GetActiveScene().name != sceneName)
                    {
                        // fade in
                        await FadeManager.Instance.FadeIn();

                        // Start and wait until the scene is almost fully loaded
                        var asyncLoad = SceneManager.LoadSceneAsync(sceneName);
                        while (asyncLoad.progress < 0.9f)
                        {
                            await Task.Yield();
                        }

                        // Perform pre-scene-activation setup if needed
                        // Example: Load additional assets, set up lighting, etc.
                        _inputManager.MouseOverUI = false;
                        Globals.Instance.BlurCount = 0;
                        Globals.Instance.MouseOverCount = 0;

                        // Allow the scene to activate
                        asyncLoad.allowSceneActivation = true;

                        // Wait until the scene is completely loaded and set active
                        while (!asyncLoad.isDone)
                        {
                            await Task.Yield();
                        }
                    }

                    await LoadAdditiveScene();
                
                    _signalBus.Fire<SceneReadySignal>();
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                    throw;
                }
            });
            
            await _signalBus.WaitFor<SceneReadySignal>();
            
            Debug.Log($"Scene '{sceneName}' loaded");

            if (callback != null)
            {
                await callback();
            }
            await FadeManager.Instance.FadeOut();
        }
    }
}
