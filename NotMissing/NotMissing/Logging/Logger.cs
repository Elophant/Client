using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace NotMissing.Logging
{
    public class Logger : ILogParent, IDisposable
    {
        static readonly Logger instance = new Logger();
        static public Logger Instance
        {
            get { return instance; }
        }
        protected bool InSync = false;
        protected readonly object SyncRoot = new object();
        public List<ILogListener> Listeners = new List<ILogListener>();
        ProcessQueue<LogQueueItem> Queue = new ProcessQueue<LogQueueItem>();

        public Logger()
        {
            Queue.Process += Queue_Process;
        }

        void Queue_Process(object sender, ProcessQueueEventArgs<LogQueueItem> e)
        {
            ILogListener[] listeners;
            lock (SyncRoot)
            {
                listeners = new ILogListener[Listeners.Count];
                Listeners.CopyTo(listeners);
            }
            foreach (var listener in listeners)
            {
                if ((listener.Level & e.Item.Level) != 0 && listener.LogFunc != null)
                {
                    listener.LogFunc(e.Item.Level, e.Item.Object);
                }
            }
        }

        public virtual void Register(ILogListener listener)
        {
            lock (SyncRoot)
            {
                if (Listeners.Contains(listener))
                    return;
                if (listener.Parents != null)
                    listener.Parents.Add(this);
                Listeners.Add(listener);
            }
        }
        public virtual void Unregister(ILogListener listener)
        {
            lock (SyncRoot)
            {
                if (!Listeners.Contains(listener))
                    return;
                Listeners.Remove(listener);
            }
        }

        public virtual void Log(Levels level, object obj)
        {
            Queue.Enqueue(new LogQueueItem { Level = level, Object = obj });
        }

        public virtual void Trace(object obj) { Log(Levels.Trace, obj); }
        public virtual void Debug(object obj) { Log(Levels.Debug, obj); }
        public virtual void Warning(object obj) { Log(Levels.Warning, obj); }
        public virtual void Info(object obj) { Log(Levels.Info, obj); }
        public virtual void Error(object obj) { Log(Levels.Error, obj); }
        public virtual void Fatal(object obj) { Log(Levels.Fatal, obj); }


        #region Dispose
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~Logger()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (Queue != null)
                {
                    Queue.Process -= Queue_Process;
                    Queue.Dispose();
                    Queue = null;
                }
            }
        }

        #endregion

        private class LogQueueItem
        {
            public Levels Level;
            public object Object;
        }
    }
}
