using System;

namespace ExchangeCurrency.Logger
{
    public interface ILog
    {
        void Information(string message, Exception ex = null);
        void Warning(string message, Exception ex = null);
        void Debug(string message, Exception ex = null);
        void Error(string message, Exception ex = null);
    }
}
