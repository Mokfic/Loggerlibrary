using Loggerlibrary;
using System;
using System.Collections.Generic;
using System.Text;

namespace LoggerDemo
{
    public class Configuration : IConfiguration
    {
        public string LoggerDir { get => AppContext.BaseDirectory; }

        /// <summary>
        /// 5K 
        /// </summary>
        public long MaxFileSize { get => 100; }

        /// <summary>
        /// Writes data from buffer to target
        /// </summary>
        public int QueueTimerPeriodMs { get => 1000; }
        
    }
}
