using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Loggerlibrary.LogTarget
{
    public interface ILogTarget
    {
        /// <summary>
        /// writes one log element
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        Task Write(Model.LogModel log);

        /// <summary>
        /// writes list of log elements
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        Task WriteAll(IEnumerable<Model.LogModel> log);
    }
}
