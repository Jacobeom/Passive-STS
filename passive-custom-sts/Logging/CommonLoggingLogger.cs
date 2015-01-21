using Common.Logging;
using Microsoft.Owin.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace passive_custom_sts.Logging
{
    public class CommonLoggingLogger : ILogger
    {
        private readonly ILog _commonLogger;
        public CommonLoggingLogger(string loggerName)
        {
            _commonLogger = LogManager.GetLogger(loggerName);
        }
        public bool WriteCore(System.Diagnostics.TraceEventType eventType, int eventId, object state, Exception exception, Func<object, Exception, string> formatter)
        {
            switch (eventType)
            {
                case System.Diagnostics.TraceEventType.Warning:
                    _commonLogger.Warn(string.Format("{0} {1} ",eventId, formatter(state, exception)));
                    break;
                case System.Diagnostics.TraceEventType.Verbose:
                    _commonLogger.Debug(string.Format("{0} {1} ", eventId, formatter(state, exception)));
                    break;
                case System.Diagnostics.TraceEventType.Information:
                    _commonLogger.Info(string.Format("{0} {1} ", eventId, formatter(state, exception)));
                    break;
                case System.Diagnostics.TraceEventType.Error:
                    _commonLogger.Error(string.Format("{0} {1} ", eventId, formatter(state, exception)));
                    break;
                case System.Diagnostics.TraceEventType.Critical:
                   _commonLogger.Fatal(string.Format("{0} {1} ", eventId, formatter(state, exception)));
                   break;
                default:
                   _commonLogger.Warn(string.Format("{0} {1} ", eventId, formatter(state, exception)));
                   break;
            }
            return true;
        }
    }
}