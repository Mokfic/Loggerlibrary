﻿using Loggerlibrary;
using System;

namespace LoggerDemo
{
    /// <summary>
    /// Demo config 
    /// </summary>
    public class Configuration : IConfiguration
    {
        /// <summary>
        /// where creates log files: App folder
        /// </summary>
        public string LoggerDir => AppContext.BaseDirectory;

        /// <summary>
        /// 5K 
        /// </summary>
        public long MaxFileSize => 1024*5;

        /// <summary>
        /// Writes data from buffer to target if zero, then disabled 
        /// One sec timer.
        /// </summary>
        public int QueueTimerPeriodMs => 1000;
    }
}
