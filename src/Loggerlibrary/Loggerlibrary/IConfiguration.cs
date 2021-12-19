using System;
using System.Collections.Generic;
using System.Text;

namespace Loggerlibrary
{
    public interface IConfiguration
    {
        string LoggerDir { get; }
        long MaxFileSize { get; }
        int QueueTimerPeriodMs { get; }
    }
}
