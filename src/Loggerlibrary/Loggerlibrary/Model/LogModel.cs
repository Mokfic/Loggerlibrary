using System;
using System.Collections.Generic;
using System.Text;

namespace Loggerlibrary.Model
{
    public class LogModel
    {
        public DateTime Time { get; set; } = DateTime.Now;
        public string Msg { get; set; }
        public LogLevel Level {get;set;}

    }
}
