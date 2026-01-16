using System;
using System.Collections.Generic;

namespace _src.Code.Game.Modules
{
    public partial class GameModule<TModel>
    {
        private readonly List<Action> _signalSubscriptions = new();
        
        /// <summary>
        /// Helper to add a signal subscription and its unsubscription action
        /// </summary>
        protected void AddSignalSubscription<TSignal>(Action<TSignal> handler)
        {
            SignalBus.Subscribe(handler);
            _signalSubscriptions.Add(() => SignalBus.TryUnsubscribe(handler));
        }
        
        /// <summary>
        /// Unsubscribes from all signals
        /// </summary>
        private void UnsubscribeFromSignals()
        {
            foreach (var subscription in _signalSubscriptions)
            {
                subscription.Invoke();
            }
            _signalSubscriptions.Clear();
        }
    }
}