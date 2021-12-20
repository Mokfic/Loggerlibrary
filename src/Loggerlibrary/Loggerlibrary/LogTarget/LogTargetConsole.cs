﻿using Loggerlibrary.Model;
using Loggerlibrary.Extension;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace Loggerlibrary.LogTarget
{
    public class LogTargetConsole : ILogTarget
    {
        readonly object _sync = new object();
        
        /// <summary>
        /// console wait max time is 1 sec
        /// </summary>
        private const int MaxWait = 1000;

        public Task Write(LogModel log)
        {
            Task t = Task.Factory.StartNew(() =>
            {
                //if expires timeout, then cancels the write
                if (!Monitor.TryEnter(_sync, MaxWait)) 
                    return;
                AddToConsole(log);
                Monitor.Exit(_sync);
            });

            return t;
        }

        public Task WriteAll(IEnumerable<LogModel> log)
        {
            Task t = Task.Factory.StartNew(() =>
            {
                if (!Monitor.TryEnter(_sync, MaxWait)) return;

                foreach (var l in log)
                {
                    AddToConsole(l);
                }
          
                Monitor.Exit(_sync);
            });

            return t;
        }

        protected void AddToConsole(LogModel log)
        {
            switch (log.Level)
            {
                case LogLevel.Debug:
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
                case LogLevel.Info:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case LogLevel.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
            }
            Console.Write($"{log.ToText()}");
            Console.ResetColor();
        }
    }
}
