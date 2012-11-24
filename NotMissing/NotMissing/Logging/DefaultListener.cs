using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NotMissing.Logging
{
    public class DefaultListener : ILogListener, IDisposable
    {
        public LogHandler LogFunc { get; set; }

        public Levels Level { get; set; }

        public List<ILogParent> Parents { get; set; }

        public DefaultListener(Levels level, LogHandler logfunc)
        {
            Parents = new List<ILogParent>();
            Level = level;
            LogFunc = logfunc;
        }

        protected virtual void Dispose(bool disp)
        {
            if (disp && Parents != null)
            {
                foreach (var parent in Parents)
                    parent.Unregister(this);
                Parents = null;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        ~DefaultListener()
        {
            Dispose(false);
        }
    }
}
