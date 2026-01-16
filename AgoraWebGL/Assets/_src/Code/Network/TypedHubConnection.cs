using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using _src.Code.Core.Interfaces.Services;
using _src.Code.UI.Shared;
using Microsoft.AspNetCore.SignalR.Client;
using UnityEngine;

namespace _src.Code.Network
{
    public class TypedHubConnection<T> where T : class
    {
        private readonly HubConnection _connection;
        private readonly IVisualElementService _visualElementService;

        public TypedHubConnection(HubConnection connection, IVisualElementService visualElementService)
        {
            _connection = connection;
            _visualElementService = visualElementService;
            
            _connection.ServerTimeout = TimeSpan.FromMinutes(15); 
        }

        public async Task InvokeAsync(Expression<Func<T, Task>> expression)
        {
            // 1. Extract Method Name
            var methodCall = (MethodCallExpression)expression.Body;
            var methodName = methodCall.Method.Name;

            // 2. Extract Arguments
            var arguments = methodCall.Arguments
                .Select(arg => Expression.Lambda(arg).Compile().DynamicInvoke())
                .ToArray();

            try
            {
                // 3. Send to Server
                await _connection.InvokeCoreAsync(methodName, arguments);
            }
            catch (Exception ex)
            {
                Debug.LogError($"[SignalR] Error invoking {methodName}: {ex.Message}");
                
                // 4. Centralized UI Feedback
                var popup = await _visualElementService.GetOrCreate<WarningPopup>();
                popup.Title.Label.text = "Connection Error";
                popup.Message.text = ex.Message;
                popup.Show();
                
                throw;
            }
        }
    }
}