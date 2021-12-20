using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Loggerlibrary
{
    /// <summary>
    /// Tha main logger interface
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Writes a log
        /// </summary>
        /// <param name="msg">log message</param>
        /// <param name="level">log level</param>
        /// <returns>task uses in async mode</returns>
        Task WriteLog(string msg, Model.LogLevel? level= null);
    }
}
