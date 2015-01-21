using Microsoft.Owin.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace passive_custom_sts.Logging
{
    public class CommonLoggingLoggerFactory : ILoggerFactory
    {

        public ILogger Create(string name)
        {
            return new CommonLoggingLogger(name);
        }
    }
}