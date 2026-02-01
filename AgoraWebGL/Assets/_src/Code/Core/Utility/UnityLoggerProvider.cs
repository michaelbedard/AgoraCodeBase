using System;
using Microsoft.Extensions.Logging;
using UnityEngine;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace _src.Code.Core.Utility
{
    // This class creates the logger
    public class UnityLoggerProvider : ILoggerProvider
    {
        public ILogger CreateLogger(string categoryName)
        {
            return new UnityLogger();
        }

        public void Dispose() { }
    }

// This class actually writes to the Unity Console
    public class UnityLogger : ILogger
    {
        public IDisposable BeginScope<TState>(TState state) => null;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            // Format the message
            string message = formatter(state, exception);
        
            // Route to the correct Unity log type
            switch (logLevel)
            {
                case LogLevel.Error:
                case LogLevel.Critical:
                    Debug.LogError($"[SignalR] {message}");
                    break;
                case LogLevel.Warning:
                    Debug.LogWarning($"[SignalR] {message}");
                    break;
                default:
                    Debug.Log($"[SignalR] {message}");
                    break;
            }
        }
    }
}