using Loggerlibrary.LogTarget;
using Loggerlibrary.Model;
using System.Threading.Tasks;

namespace Loggerlibrary
{
    public class Logger : ILogger
    {
        public LogLevel DefaultLevel = LogLevel.Info;
        private readonly ILogTarget _logTarget;
        public Logger(ILogTarget logTarget)
        {
            this._logTarget = logTarget;
        }
            
        public Task WriteLog(string msg , LogLevel? level = null)
        {
            _logTarget.Write(new LogModel() { Msg = msg, Level= level ?? DefaultLevel }).Wait();
            return Task.CompletedTask;
        }
    }
}
