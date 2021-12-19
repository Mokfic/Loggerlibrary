using Loggerlibrary.Model;
using Loggerlibrary.Extension;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;

namespace Loggerlibrary.LogTarget
{
    public class LogTargetFile : ILogTarget
    {
        private object _sync = new object();

        private IConfiguration configuration;

        private const int MAX_WAIT = 10000;
        public LogTargetFile(IConfiguration configuration)
        {
            this.configuration = configuration;
        }


        public Task Write(LogModel log)
        {

            UTF8Encoding utfencoding = new UTF8Encoding();
            string filename = GetFileName();

            byte[] result = utfencoding.GetBytes(log.ToText());

            return WriteFile(filename, result);
        }

        /// <summary>
        /// Mass write, faster than one by one
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        public Task WriteAll(IEnumerable<LogModel> log)
        {
            UTF8Encoding utfencoding = new UTF8Encoding();
            string filename = GetFileName();

            StringBuilder sb = new StringBuilder();
            foreach (var l in log)
            {
                sb.Append(l.ToText());
            }

            byte[] logtextbytes = utfencoding.GetBytes(sb.ToString());

            return WriteFile(filename, logtextbytes);
        }

        /// <summary>
        /// Writes bytes to file, in background, then release file handle
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        protected Task WriteFile(string filename, byte[] data)
        {
            Task writetask = Task.Factory.StartNew(() =>
            {
                if (!Monitor.TryEnter(_sync, MAX_WAIT))
                {
                    throw new Exception("File access error");
                }

                try
                {

                    using FileStream filestream = File.Open(filename, FileMode.OpenOrCreate);
                    filestream.Seek(0, SeekOrigin.End);
                    filestream.Write(data, 0, data.Length);

                    //check: if file is full, create new empty
                    if (filestream.Length > configuration.MaxFileSize)
                    {
                        using (File.Create(GetFileName(true))) { }
                    }

                    filestream.Close();
                }
                finally
                {
                    Monitor.Exit(_sync);
                }

            });

            return writetask;
        }


        /// <summary>
        /// Return tha latest logfile name, or the next file name
        /// </summary>
        /// <param name="newFile">to get the next</param>
        /// <returns></returns>
        protected string GetFileName(bool newFile = false)
        {
            const int FIELDCOUNT = 2;
            const int COUNTER_INDEX = 1;
            const int PREFIX_INDEX = 0;

            var files = Directory.GetFiles(configuration.LoggerDir);
            int lognum = 0;

            //only txt files
            foreach (var file in files.ToList().Where(f => f.EndsWith(".txt")))
            {
                //filename fields separated by .
                var fields = Path.GetFileName(file).Split('.');

                //parse only log files, and get the number
                if ((fields.Length > FIELDCOUNT) && (fields[PREFIX_INDEX] == "log"))
                {
                    if (int.TryParse(fields[COUNTER_INDEX], out int intv))
                    {
                        //find the latest number
                        lognum = Math.Max(lognum, intv);
                    }
                }
            }

            if (newFile) lognum++;
            
            var filename = lognum > 0 ? $"log.{lognum}.txt" : "log.txt";

            var filepath = Path.Combine(configuration.LoggerDir, filename);
            return filepath;
        }

    }
}
