using UnityEngine;
using System.Runtime.InteropServices;
using _src.Code.Core.Interfaces.Handlers;
using Zenject;

public class WebGLLifecycle : MonoBehaviour
{
    private IAppLogic _appLogic;

    [Inject]
    public void Construct(IAppLogic appLogic)
    {
        _appLogic = appLogic;
    }

    [DllImport("__Internal")]
    private static extern void RegisterTabCloseListener();

    void Start()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
            RegisterTabCloseListener();
#endif
    }

    // 1. Called by JavaScript (Browser Close/Refresh)
    public void OnBrowserClose()
    {
        Debug.Log("[WebGL] Browser tab closing.");
        TriggerLogout();
    }

    // 2. Called by Unity Editor (Stop Button or App Close)
    private void OnApplicationQuit()
    {
#if UNITY_EDITOR
        Debug.Log("[Editor] Application quitting.");
        TriggerLogout();
#endif
    }

    // Shared Logic
    private void TriggerLogout()
    {
        // Ensure this is a "Fire and Forget" call or synchronous
        _ = _appLogic.Logout(); 
    }
}