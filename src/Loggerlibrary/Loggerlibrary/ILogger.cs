using System;
using System.Collections.Generic;
using System.Text;

namespace Loggerlibrary
{
    public interface ILogger
    {
        void WriteLog(string msg, Model.LogLevel? level= null);
    }
}
