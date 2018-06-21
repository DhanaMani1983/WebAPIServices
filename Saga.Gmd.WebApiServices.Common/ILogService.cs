using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using log4net.Core;

namespace Saga.Gmd.WebApiServices.Common
{
    public interface ILogService<T>
    {
        void Debug(object message, Exception exception);
        void Debug(object message);
        void DebugFormat(IFormatProvider provider, string format, params object[] args);
        log4net.Core.ILogger Logger { get; }

        void Info(object message, Exception exception);
    }
    public class LogService<T> : ILogService<T>
    {
        private ILog log;

        public LogService()
        {
            log = log4net.LogManager.GetLogger(typeof(T));
        }
        public void Debug(object message, Exception exception)
        {
            log.Debug(message, exception);
        }

        public void Debug(object message)
        {
            log.Debug(message);
        }

        public void DebugFormat(IFormatProvider provider, string format, params object[] args)
        {
            log.DebugFormat(provider, format, args);
        }

        /// <summary>
        /// 
        /// </summary>
        public ILogger Logger => log.Logger;

        public void Info(object message, Exception exception)
        {
            log.Info(message, exception);
        }
    }
}
