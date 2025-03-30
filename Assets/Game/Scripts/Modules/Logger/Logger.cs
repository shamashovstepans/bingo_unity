using System;

namespace BingoGame.Modules.Logger
{
    internal class Logger : ILogger
    {
        private readonly string _category;

        public Logger(string category)
        {
            _category = category;
        }

        public void Error(string message, Exception exception = null)
        {
            if (exception != null)
            {
                UnityEngine.Debug.LogError($"[{_category}] {message}. {exception.GetType()} {exception.Message}");
            }
            else
            {
                UnityEngine.Debug.LogError($"[{_category}] {message}");
            }
        }

        public void Warning(string message, Exception exception = null)
        {
            if (exception != null)
            {
                UnityEngine.Debug.LogWarning($"[{_category}] {message}. {exception.GetType()} {exception?.Message}");
            }
            else
            {
                UnityEngine.Debug.LogWarning($"[{_category}] {message}");
            }
        }

        public void Info(string message)
        {
            UnityEngine.Debug.Log($"[{_category}] {message}");
        }

        public void Debug(string message)
        {
            UnityEngine.Debug.Log($"[{_category}] {message}");
        }
    }
}