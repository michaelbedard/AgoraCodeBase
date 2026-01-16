using System;
using System.Threading.Tasks;
using _src.Code.Core.Actors;
using Agora.Core.Actors;
using UnityEngine;
using Zenject;

namespace _src.Code.Core.Extensions
{
    public static class SignalBusExtensions
    {
        public static void SubscribeAsync<TSignal>(this SignalBus signalBus, Func<Task> callbackAsync)
        {
            signalBus.Subscribe<TSignal>(async () =>
            {
                try
                {
                    // Invoke the asynchronous callback and await it
                    await callbackAsync().ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    // Handle any exceptions that occur during the async operation
                    Debug.LogError($"Error in async signal handler: {ex}");
                }
            });
        }
        
        public static async Task WaitFor<TSignal>(this SignalBus signalBus)
        {
            var tcs = new TaskCompletionSource<bool>();
            Action signalHandler = null; // Declare the variable first
            signalHandler = () => // Then assign it
            {
                signalBus.Unsubscribe<TSignal>(signalHandler);
                tcs.SetResult(true);
            };

            signalBus.Subscribe<TSignal>(signalHandler);
            await tcs.Task;
        }
    }
}