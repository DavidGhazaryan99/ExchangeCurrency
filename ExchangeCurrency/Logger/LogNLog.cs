using NLog;
using System;

namespace ExchangeCurrency.Logger
{
    public class LogNLog : ILog
    {
        private static ILogger logger = LogManager.GetCurrentClassLogger();

        public LogNLog()
        {
        }

        public void Information(string message, Exception ex = null)
        {
            if (ex == null)
            {
                logger.Info(message);
            }
            else
            {
                logger.Info(ex, message);
            }
        }

        public void Warning(string message, Exception ex = null)
        {
            if (ex == null)
            {
                logger.Warn(message);
            }
            else
            {
                logger.Warn(ex, message);
            }
        }

        public void Debug(string message, Exception ex = null)
        {
            if (ex == null)
            {
                logger.Debug(message);
            }
            else
            {
                logger.Debug(ex, message);
            }
        }

        public void Error(string message, Exception ex = null)
        {
            if (ex == null)
            {
                logger.Error(message);
            }
            else
            {
                logger.Error(ex, message);
            }
        }
    }
}
