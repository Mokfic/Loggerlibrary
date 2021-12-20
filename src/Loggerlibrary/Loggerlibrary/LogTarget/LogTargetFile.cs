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
        private readonly object _sync = new object();

        private readonly IConfiguration _configuration;

        private const int MaxWait = 10000;
        public LogTargetFile(IConfiguration configuration)
        {
            this._configuration = configuration;
        }


        public Task Write(LogModel log)
        {

            UTF8Encoding utfEncoding = new UTF8Encoding();
            string filename = GetFileName();

            byte[] result = utfEncoding.GetBytes(log.ToText());

            return WriteFile(filename, result);
        }

        /// <summary>
        /// Mass write, faster than one by one
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        public Task WriteAll(IEnumerable<LogModel> log)
        {
            UTF8Encoding utfEncoding = new UTF8Encoding();
            string filename = GetFileName();

            StringBuilder sb = new StringBuilder();
            foreach (var l in log)
            {
                sb.Append(l.ToText());
            }

            byte[] logTextBytes = utfEncoding.GetBytes(sb.ToString());

            return WriteFile(filename, logTextBytes);
        }

        /// <summary>
        /// Writes bytes to file, in background, then release file handle
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        protected Task WriteFile(string filename, byte[] data)
        {
            Task writeTask = Task.Factory.StartNew(() =>
            {
                if (!Monitor.TryEnter(_sync, MaxWait))
                {
                    throw new Exception("File access error");
                }

                try
                {

                    using FileStream fileStream = File.Open(filename, FileMode.OpenOrCreate);
                    fileStream.Seek(0, SeekOrigin.End);
                    fileStream.Write(data, 0, data.Length);

                    //check: if file is full, create new empty
                    if (fileStream.Length > _configuration.MaxFileSize)
                    {
                        using (File.Create(GetFileName(true))) { }
                    }

                    fileStream.Close();
                }
                finally
                {
                    Monitor.Exit(_sync);
                }

            });

            return writeTask;
        }


        /// <summary>
        /// Return tha latest logfile name, or the next file name
        /// </summary>
        /// <param name="newFile">to get the next</param>
        /// <returns></returns>
        protected string GetFileName(bool newFile = false)
        {
            const int fieldCount = 2;
            const int counterIndex = 1;
            const int prefixIndex = 0;

            var files = Directory.GetFiles(_configuration.LoggerDir);
            int logFileIndex = 0;

            //only txt files
            foreach (var file in files.ToList().Where(f => f.EndsWith(".txt")))
            {
                //filename fields separated by .
                var fields = Path.GetFileName(file).Split('.');

                //parse only log files, and get the number
                if ((fields.Length > fieldCount) && (fields[prefixIndex] == "log"))
                {
                    if (int.TryParse(fields[counterIndex], out int intValue))
                    {
                        //find the latest number
                        logFileIndex = Math.Max(logFileIndex, intValue);
                    }
                }
            }

            if (newFile) logFileIndex++;
            
            var filename = logFileIndex > 0 ? $"log.{logFileIndex}.txt" : "log.txt";

            var filepath = Path.Combine(_configuration.LoggerDir, filename);
            return filepath;
        }

    }
}
