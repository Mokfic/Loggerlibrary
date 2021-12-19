using Loggerlibrary.LogTarget;
using Loggerlibrary.Model;
using System;
using System.Collections.Generic;
using System.Text;


namespace Loggerlibrary
{
    public class Logger : ILogger
    {
        public LogLevel DefaultLevel = LogLevel.info;
        private ILogTarget logTarget;
        public Logger(ILogTarget logTarget)
        {
            this.logTarget = logTarget;
        }
            
        public void WriteLog(string msg , Model.LogLevel? level = null)
        {
            logTarget.Write(new Model.Log() { Msg = msg, Level= level ?? DefaultLevel });  
        }
    }
}
