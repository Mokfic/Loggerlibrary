using Loggerlibrary.Model;
using Loggerlibrary.Extension;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace Loggerlibrary.LogTarget
{
    public class LogTargetStream : ILogTarget
    {
        protected MemoryStream Stream= new MemoryStream();
        public Task Write(LogModel log)
        {
            UTF8Encoding utfEncoding = new UTF8Encoding(); 
            var data = utfEncoding.GetBytes(log.ToText());
            Stream.Seek(0, SeekOrigin.End);
            return Stream.WriteAsync(data,0,data.Length);
        }

        public Task WriteAll(IEnumerable<LogModel> log)
        {
            UTF8Encoding utfEncoding = new UTF8Encoding();
            StringBuilder sb = new StringBuilder();
            foreach (var l in log)
            {
                sb.Append(l.ToText());
            }

            byte[] logTextBytes = utfEncoding.GetBytes(sb.ToString());

            Stream.Seek(0, SeekOrigin.End);
            return Stream.WriteAsync(logTextBytes, 0, logTextBytes.Length);
        }
    }
}
