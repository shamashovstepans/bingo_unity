using System;

namespace BingoGame.Modules.Logger
{
    public interface ILogger
    {
        void Error(string message, Exception exception = null);
        void Warning(string message, Exception exception = null);
        void Info(string message);
        void Debug(string message);
    }
}