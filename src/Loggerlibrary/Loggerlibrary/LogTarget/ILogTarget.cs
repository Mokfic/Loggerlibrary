using System;
using System.Collections.Generic;
using System.Text;

namespace Loggerlibrary.LogTarget
{
    public interface ILogTarget
    {
        void Write(Model.Log log);
    }
}
