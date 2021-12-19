using Loggerlibrary.Model;
using Loggerlibrary.Extension;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace Loggerlibrary.LogTarget
{
    public class LogTargetStream : ILogTarget
    {
        protected MemoryStream stream= new MemoryStream();
        public Task Write(LogModel log)
        {
            UTF8Encoding uniencoding = new UTF8Encoding(); 
            var data = uniencoding.GetBytes(log.ToText());
            stream.Seek(0, SeekOrigin.End);
            return stream.WriteAsync(data,0,data.Length);
        }

        public Task WriteAll(IEnumerable<LogModel> log)
        {
            throw new NotImplementedException();
        }
    }
}
