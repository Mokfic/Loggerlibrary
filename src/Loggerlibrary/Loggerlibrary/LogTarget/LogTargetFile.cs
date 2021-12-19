using Loggerlibrary.Model;
using Loggerlibrary.Extension;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Threading;

namespace Loggerlibrary.LogTarget
{
    public class LogTargetFile : ILogTarget
    {
        object _sync = new object();

        private IConfiguration configuration;
        public LogTargetFile(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        protected string GetFileName()
        {
            var filename = "log1.txt";

            var filepath = Path.Combine(configuration.LoggerDir, filename);
            return filepath;
        }
            

        public Task Write(LogModel log)
        {

            UTF8Encoding uniencoding = new UTF8Encoding();
            string filename = GetFileName();

            byte[] result = uniencoding.GetBytes(log.ToText());

            return WriteFile(filename, result);
        }

        protected Task WriteFile(string filename,  byte[] data)
        {
            Task writetask = Task.Factory.StartNew(() =>
            {
                if (!Monitor.TryEnter(_sync, 10000))
                    return;

                try
                {              

                    using FileStream SourceStream = File.Open(filename, FileMode.OpenOrCreate);
                    SourceStream.Seek(0, SeekOrigin.End);
                    SourceStream.Write(data, 0, data.Length);
                    SourceStream.Close();
                }
                finally
                {
                    Monitor.Exit(_sync);
                }

            });

            return writetask;
        }

        public Task WriteAll(IEnumerable<LogModel> log)
        {
            UTF8Encoding uniencoding = new UTF8Encoding();
            string filename = GetFileName();

            StringBuilder sb = new StringBuilder();
            foreach (var l in log)
            {
                sb.Append(l.ToText());
            }

            byte[] logtextbytes = uniencoding.GetBytes(sb.ToString());

            return WriteFile(filename, logtextbytes);
        }
    }
}
