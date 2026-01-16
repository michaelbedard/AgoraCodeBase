using System;
using System.Text;
using System.Threading.Tasks;
using _src.Code.Core.Interfaces.Services;
using Microsoft.AspNetCore.SignalR.Client;
using UnityEngine;
using Zenject;

namespace _src.Code.App.Services
{
    public class ConnectionService : IConnectionService, IDisposable
    {
        // private
        private readonly SignalBus _signalBus;
        
        [Inject]
        public ConnectionService(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }
        
        /// <summary>
        /// Connect
        /// </summary>
        public async Task ConnectAsync(string username)
        {
            // Debug.Log("HubConnection Initialization Started");
            //
            // _hubConnections = new Dictionary<HubType, HubConnection>
            // {
            //     { HubType.Friend, new HubConnectionBuilder().WithUrl($"{Globals.Instance.ServerUrl}/friendHub?username={username}").AddJsonProtocol(options =>
            //     {
            //         options.PayloadSerializerOptions.Converters.Add(new PolymorphicConverterFactory());
            //     }).Build() },
            //     { HubType.Channel, new HubConnectionBuilder().WithUrl($"{Globals.Instance.ServerUrl}/channelHub?username={username}").AddJsonProtocol(options =>
            //     {
            //         options.PayloadSerializerOptions.Converters.Add(new PolymorphicConverterFactory());
            //     }).Build() },
            //     { HubType.Floor, new HubConnectionBuilder().WithUrl($"{Globals.Instance.ServerUrl}/floorHub?username={username}").AddJsonProtocol(options =>
            //     {
            //         options.PayloadSerializerOptions.Converters.Add(new PolymorphicConverterFactory());
            //     }).Build() },
            //     { HubType.Table, new HubConnectionBuilder().WithUrl($"{Globals.Instance.ServerUrl}/tableHub?username={username}").AddJsonProtocol(options =>
            //     {
            //         options.PayloadSerializerOptions.Converters.Add(new PolymorphicConverterFactory());
            //     }).Build() }
            // };
            //
            // var connectionTasks = _hubConnections.Select(async hub =>
            // {
            //     var (hubType, connection) = hub;
            //     try
            //     {
            //         await connection.StartAsync();
            //         Debug.Log($"Successfully connected to {hubType} hub.");
            //
            //         _signalBus.Fire(new HubConnectionSignal
            //         {
            //             HubType = hubType,
            //             HubConnection = connection
            //         });
            //     }
            //     catch (Exception ex)
            //     {
            //         LogConnectionError(hubType, connection, ex);
            //         throw;
            //     }
            // });
            //
            // try
            // {
            //     await Task.WhenAll(connectionTasks);
            //     Debug.Log("All hub connections established successfully.");
            // }
            // catch (Exception ex)
            // {
            //     Debug.LogError($"One or more hub connections failed: {ex.Message}");
            // }
        }
        
        private void LogConnectionError(HubConnection connection, Exception ex)
        {
            var logMessage = new StringBuilder();
            logMessage.AppendLine($"Exception Message: {ex.Message}");
            logMessage.AppendLine($"Inner Exception: {ex.InnerException?.Message}");
            logMessage.AppendLine($"Stack Trace: {ex.StackTrace}");

            Debug.LogError(logMessage.ToString());
        }
        
        /// <summary>
        /// Dispose
        /// </summary>
        public async void Dispose()
        {
            // // stop hubs
            // var stopTasks = new List<Task>();
            // foreach (var hubConnection in _hubConnections.Values)
            // {
            //     stopTasks.Add(hubConnection.StopAsync());
            // }
            //
            // await Task.WhenAll(stopTasks);
            //
            // // dispose hubs
            // var disposeTasks = new List<Task>();
            // foreach (var hubConnection in _hubConnections.Values)
            // {
            //     disposeTasks.Add(hubConnection.DisposeAsync().AsTask());
            // }
            //
            // await Task.WhenAll(disposeTasks);
        }
    }
}