using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NotMissing.Logging
{
    [Flags]
    public enum Levels
    {
        Trace = 1,
        Debug = 2,
        Warning = 4,
        Info = 8,
        Error = 16,
        Fatal = 32,
        All = 63,
    }
}
