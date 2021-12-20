using System;
using System.Text;

namespace Loggerlibrary.Extension
{
    public static class LoggerExtension
    {
        public static string ToTextOnly(this Model.LogModel log)
        {
            return log.Msg;
        }
        public static string ToText(this Model.LogModel log)
        {

            var sb = new StringBuilder();

            sb.Append(log.Time);
            sb.Append(" [");
            sb.Append(log.Level);
            sb.Append("] ");
            sb.Append(log.Msg);
            sb.Append(Environment.NewLine);
            return sb.ToString();
        }
    }
}
