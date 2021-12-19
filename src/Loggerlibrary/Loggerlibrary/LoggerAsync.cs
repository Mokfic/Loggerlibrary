using Loggerlibrary.LogTarget;
using Loggerlibrary.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Loggerlibrary
{
    public class LoggerAsync : ILogger
    {
        public LogLevel DefaultLevel = LogLevel.info;
        private ILogTarget logTarget;
        public LoggerAsync(ILogTarget logTarget)
        {
            this.logTarget = logTarget;
        }

        public async Task WriteLog(string msg, Model.LogLevel? level = null)
        {                    
            await logTarget.Write(new Model.LogModel() { Msg = msg, Level = level ?? DefaultLevel });
        }
    }
}
