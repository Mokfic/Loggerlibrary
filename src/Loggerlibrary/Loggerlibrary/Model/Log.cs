using System;
using System.Collections.Generic;
using System.Text;

namespace Loggerlibrary.Model
{
    public class Log
    {
        public string Msg { get; set; }

        public LogLevel Level {get;set;}

        public override string ToString()
        {
            return $"[{Level}] {Msg}";
        }
    }
}
