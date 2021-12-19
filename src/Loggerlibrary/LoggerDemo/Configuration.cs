using Loggerlibrary;
using System;
using System.Collections.Generic;
using System.Text;

namespace LoggerDemo
{
    public class Configuration : IConfiguration
    {
        /// <summary>
        /// where creates log files
        /// </summary>
        public string LoggerDir { get => AppContext.BaseDirectory; }

        /// <summary>
        /// 5K 
        /// </summary>
        public long MaxFileSize { get => 1024*5; }

        /// <summary>
        /// Writes data from buffer to target if zero, then disabled 
        /// </summary>
        public int QueueTimerPeriodMs { get => 1000; }
        
    }
}
