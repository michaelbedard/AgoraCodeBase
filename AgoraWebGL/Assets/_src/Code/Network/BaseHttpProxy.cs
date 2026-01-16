using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using _src.Code.Core;
using _src.Code.Core.Actors;
using _src.Code.Core.Interfaces.Services;
using Newtonsoft.Json;
using UnityEngine.Networking;
using Debug = UnityEngine.Debug;

namespace _src.Code.Network
{
    public class BaseHttpProxy
    {
        protected string BaseUrl => Globals.Instance.ServerUrl;
        
        private readonly IVisualElementService _visualElementService;
        private readonly IClientDataService _clientDataService;

        protected BaseHttpProxy(IVisualElementService visualElementService, IClientDataService clientDataService)
        {
            _visualElementService = visualElementService;
            _clientDataService = clientDataService;
        }
        
        protected async Task<HttpResponseResult<T>> SendHttpRequest<T>(string url, string method, object data)
        { 
            UnityWebRequest request = null;
            string jsonData = null;

            var userId = _clientDataService.Id;
            if (!string.IsNullOrEmpty(userId))
            {
                string separator = url.Contains("?") ? "&" : "?";
                url += $"{separator}userId={UnityWebRequest.EscapeURL(userId)}";
            }

            try
            {
                // 3. Create request with the modified URL
                request = new UnityWebRequest(url, method);
                
                jsonData = data != null ? JsonConvert.SerializeObject(data) : null;

                if (!string.IsNullOrEmpty(jsonData))
                {
                    byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
                    request.uploadHandler = new UploadHandlerRaw(bodyRaw);
                }

                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");

                var operation = request.SendWebRequest();

                while (!operation.isDone)
                    await Task.Yield(); 

                if (request.result == UnityWebRequest.Result.Success)
                {
                    var responseText = request.downloadHandler.text;

                    if (string.IsNullOrEmpty(responseText))
                    {
                        return new HttpResponseResult<T>
                        {
                            IsSuccess = true,
                            Message = "Request succeeded, but response was empty.",
                            Data = default
                        };
                    }

                    return new HttpResponseResult<T>
                    {
                        IsSuccess = true,
                        Message = "Request succeeded.",
                        Data = JsonConvert.DeserializeObject<T>(responseText),
                    };
                }
                else
                {
                    await LogErrorDetails(request, url, method, jsonData);
                    return new HttpResponseResult<T>
                    {
                        IsSuccess = false,
                        Message = $"HTTP Request failed: {request.downloadHandler?.text}",
                        Data = default
                    };
                }
            }
            catch (Exception ex)
            {
                await LogErrorDetails(request, url, method, jsonData, ex);
                return new HttpResponseResult<T>
                {
                    IsSuccess = false,
                    Message = $"Exception: {ex.Message}",
                    Data = default
                };
            }
        }
        
        private async Task LogErrorDetails(UnityWebRequest request, string url, string method, string jsonData, Exception ex = null)
        {
            var logMessage = new StringBuilder();
            logMessage.AppendLine($"HTTP Request Failed : {request?.error}");
            logMessage.AppendLine($"URL: {url}");
            logMessage.AppendLine($"Method: {method}");
            logMessage.AppendLine($"Status Code: {(request != null ? (int)request.responseCode : 0)}");
            logMessage.AppendLine($"Response: {request?.downloadHandler?.text}");
            if (!string.IsNullOrEmpty(jsonData))
            {
                logMessage.AppendLine($"Request Data: {jsonData}");
            }

            if (ex != null)
            {
                logMessage.AppendLine($"Exception Message: {ex.Message}");
                logMessage.AppendLine($"Inner Exception: {ex.InnerException?.Message}");
            }

            Debug.LogError(logMessage.ToString());
        }
    }
}