using System;
using System.Runtime.InteropServices;
using _src.Code.Core.Interfaces.Handlers;
using _src.Code.Core.Interfaces.Services;
using UnityEngine;
using Zenject;

namespace _src.Code.Core.Utility
{
    [Serializable]
    public class DiscordPayload
    {
        public string channelId;
        public string authCode;
    }

    public class DiscordBridge : MonoBehaviour
    {
        [Inject] private IAppLogic _appLogic;
        [Inject] private IVisualElementService _visualElementService;
        
        // --- 1. Define the external JS function ---
        [DllImport("__Internal")]
        private static extern void RequestDiscordData();

        public static string CurrentChannelId { get; private set; }
        public static string AuthCode { get; private set; }

        private void Start()
        {
            // Only run this in WebGL build
#if !UNITY_EDITOR && UNITY_WEBGL
        Debug.Log("[DiscordBridge] Requesting data from Svelte...");
        RequestDiscordData(); // Call the .jslib function
#else
            Debug.Log("[DiscordBridge] Running in Editor - Skipping JS Bridge.");
#endif
        }

        // --- 2. The Listener (Called by Svelte) ---
        public void OnDiscordDataReceived(string jsonPayload)
        {
            Debug.Log($"[DiscordBridge] Received: {jsonPayload}");
            try 
            {
                var data = JsonUtility.FromJson<DiscordPayload>(jsonPayload);

                if (_appLogic != null)
                {
                    _appLogic.Login(data.channelId, data.authCode);
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"[DiscordBridge] Parse Error: {e.Message}");
            }
        }
        
        public async void OnDiscordError(string errorMessage)
        {
            Debug.LogError($"[DiscordBridge] JS ERROR RECEIVED: {errorMessage}");
            await _visualElementService.ShowWarning(errorMessage);
        }
    }
}