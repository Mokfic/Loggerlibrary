using Loggerlibrary.LogTarget;
using Loggerlibrary.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Loggerlibrary
{
    public class LoggerQueue : ILogger
    {
        public LogLevel DefaultLevel = LogLevel.info;
        protected ConcurrentQueue<LogModel> Buffer = new ConcurrentQueue<LogModel>();
        private ILogTarget logTarget;
        private IConfiguration configuration;
        private System.Threading.Timer writeTimer;

        public LoggerQueue(ILogTarget logTarget, IConfiguration configuration)
        {
            this.logTarget = logTarget;
            this.configuration = configuration;

            if (configuration.QueueTimerPeriodMs > 0)
                writeTimer = new System.Threading.Timer(onWriteTimer, null, configuration.QueueTimerPeriodMs,0 );
        }

        /// <summary>
        /// Only add model to queue
        /// </summary>
        /// <param name="msg">log model</param>
        /// <param name="level">log level</param>
        /// <returns></returns>
        public Task WriteLog(string msg, Model.LogLevel? level = null)
        {
            Buffer.Enqueue(new Model.LogModel() { Msg = msg, Level = level ?? DefaultLevel });
            return Task.CompletedTask;
        }


        void onWriteTimer(object tag)
        {
            if (!Buffer.IsEmpty)
            {

                //removes item from quea and add to wire list
                List<LogModel> modelstowrite = new List<LogModel>();
                while (Buffer.TryDequeue(out LogModel log))
                {
                    modelstowrite.Add(log);
                }

                //doing the mass write
                logTarget.WriteAll(modelstowrite).Wait();
            }

            //only just now restart the timer
            writeTimer = new System.Threading.Timer(onWriteTimer, null, configuration.QueueTimerPeriodMs, 0);
        }
    }
}
