using Loggerlibrary.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Loggerlibrary.LogTarget
{
    public class LogTargetConsole : ILogTarget
    {
        public void Write(Log log)
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
            Console.WriteLine($"{log}");

            Console.ResetColor();
        }
    }
}
