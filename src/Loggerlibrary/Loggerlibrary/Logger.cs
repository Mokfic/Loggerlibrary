﻿using Loggerlibrary.LogTarget;
using Loggerlibrary.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

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
            
        public Task WriteLog(string msg , Model.LogLevel? level = null)
        {
            logTarget.Write(new Model.LogModel() { Msg = msg, Level= level ?? DefaultLevel }).Wait();
            return Task.CompletedTask;
        }
    }
}
