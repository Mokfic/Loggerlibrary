using Loggerlibrary.Model;
using Loggerlibrary.Extension;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Loggerlibrary.LogTarget
{
    public class LogTargetConsole : ILogTarget
    {
        object _sync = new object();

        public Task Write(LogModel log)
        {
            Task t = Task.Factory.StartNew(() =>
            {
                if (!Monitor.TryEnter(_sync,1000)) return;
                AddToConsole(log);
                Monitor.Exit(_sync);
            });

            return t;
        }

        public Task WriteAll(IEnumerable<LogModel> log)
        {
            Task t = Task.Factory.StartNew(() =>
            {
                if (!Monitor.TryEnter(_sync, 1000)) return;

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
                case LogLevel.debug:
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
                case LogLevel.info:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case LogLevel.error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                default:
                    break;
            }
            Console.Write($"{log.ToText()}");
            Console.ResetColor();
        }
    }
}
