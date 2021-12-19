using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Loggerlibrary.LogTarget
{
    public interface ILogTarget
    {
        Task Write(Model.LogModel log);
        Task WriteAll(IEnumerable<Model.LogModel> log);
    }
}
