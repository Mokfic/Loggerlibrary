using Loggerlibrary.LogTarget;
using Loggerlibrary.Model;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Loggerlibrary
{
    public class LoggerQueue : ILogger
    {
        public LogLevel DefaultLevel = LogLevel.Info;
        protected ConcurrentQueue<LogModel> Buffer = new ConcurrentQueue<LogModel>();
        private readonly ILogTarget _logTarget;
        private readonly IConfiguration _configuration;
        private System.Threading.Timer _writeTimer;

        public LoggerQueue(ILogTarget logTarget, IConfiguration configuration)
        {
            this._logTarget = logTarget;
            this._configuration = configuration;

            if (configuration.QueueTimerPeriodMs > 0)
                _writeTimer = new System.Threading.Timer(OnWriteTimer, null, configuration.QueueTimerPeriodMs,0 );
        }

        /// <summary>
        /// Only add model to queue
        /// </summary>
        /// <param name="msg">log model</param>
        /// <param name="level">log level</param>
        /// <returns></returns>
        public Task WriteLog(string msg, LogLevel? level = null)
        {
            Buffer.Enqueue(new LogModel() { Msg = msg, Level = level ?? DefaultLevel });
            return Task.CompletedTask;
        }

        /// <summary>
        /// Periodically remove log items from queue to write 
        /// </summary>
        /// <param name="tag"></param>
        void OnWriteTimer(object tag)
        {
            if (!Buffer.IsEmpty)
            {

                //removes item from queue and add to wire list
                List<LogModel> modelsToWrite = new List<LogModel>();
                while (Buffer.TryDequeue(out LogModel log))
                {
                    modelsToWrite.Add(log);
                }

                //doing the mass write
                _logTarget.WriteAll(modelsToWrite).Wait();
            }

            //only just now restart the timer
            _writeTimer = new System.Threading.Timer(OnWriteTimer, null, _configuration.QueueTimerPeriodMs, 0);
        }
    }
}
