using Loggerlibrary.LogTarget;
using Loggerlibrary.Model;
using System.Threading.Tasks;

namespace Loggerlibrary
{
    public class LoggerAsync : ILogger
    {
        public LogLevel DefaultLevel = LogLevel.Info;
        private readonly ILogTarget _logTarget;
        public LoggerAsync(ILogTarget logTarget)
        {
            this._logTarget = logTarget;
        }

        public async Task WriteLog(string msg, LogLevel? level = null)
        {                    
            await _logTarget.Write(new LogModel() { Msg = msg, Level = level ?? DefaultLevel });
        }
    }
}
