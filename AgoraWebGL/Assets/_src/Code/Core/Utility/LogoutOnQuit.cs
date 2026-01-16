using _src.Code.Core.Interfaces.Proxies;
using _src.Code.Core.Interfaces.Services;
using UnityEngine;
using Zenject;

namespace _src.Code.Core.Utility
{
    public class LogoutOnQuit : MonoBehaviour
    {
        private IClientDataService _clientDataService;
        private IAuthHttpProxy _authHttpProxy;
        
        private bool _isLogout = false;
        
        [Inject]
        public void Construct(IClientDataService clientDataService, IAuthHttpProxy authHttpProxy)
        {
            _clientDataService = clientDataService;
            _authHttpProxy = authHttpProxy;
        }

        void OnEnable()
        {
            Application.quitting += OnApplicationQuit;
        }

        void OnDisable()
        {
            Application.quitting -= OnApplicationQuit;
        }

        async void OnApplicationQuit()
        {
            // try
            // {
            //     if (!_isLogout)
            //     {
            //         _isLogout = true;
            //
            //         if (_clientDataService.IsLoggedIn)
            //         {
            //             Debug.Log("Logout");
            //             await _authHttpProxy.Logout(new LogoutPayload()
            //             {
            //                 Username = _clientDataService.Username
            //             });
            //         }
            //     }
            // }
            // catch (Exception e)
            // {
            //     Debug.LogError(e);
            // }
        }
    }
}