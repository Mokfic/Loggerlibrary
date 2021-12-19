using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Loggerlibrary
{
    public interface ILogger
    {
        Task WriteLog(string msg, Model.LogLevel? level= null);
    }
}
