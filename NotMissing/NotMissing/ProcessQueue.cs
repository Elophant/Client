using System.Diagnostics;
using System.Threading;

namespace System.Collections.Generic
{
    public class ProcessQueueEventArgs<T> : EventArgs
    {
        public ProcessQueue<T> Owner { get; set; }
        public T Item { get; set; }
    }

    public class ProcessQueue<T> : Queue<T>, IDisposable
    {
        protected object Sync = new object();
        protected Thread Processor;
        public event EventHandler<ProcessQueueEventArgs<T>> Process;

        public ProcessQueue()
        {
            Processor = new Thread(ProcessLoop) {IsBackground = true};
            Processor.Start();
        }

        protected virtual new T Dequeue()
        {
            throw new NotSupportedException("Cannot dequeue");
        }

        protected virtual bool TryDequeue(out T item)
        {
            lock (Sync)
            {
                bool hasitems = Count > 0;
                item = hasitems ? base.Dequeue() : default(T);
                return hasitems;
            }
        }

        public virtual new void Enqueue(T item)
        {
            lock (Sync)
            {
                base.Enqueue(item);
                Monitor.PulseAll(Sync);
            }
        }

		public virtual new int Count 
		{
			get 
			{
				lock (Sync)
				{
					return base.Count;
				}
			}
		}

    	protected virtual void ProcessLoop()
        {
            while (Processor != null)
            {
                T obj;
                if (TryDequeue(out obj))
                {
                    OnProcess(obj);
                }
                else
                {
                    lock (Sync)
                    {
                        if (Count < 1)
                            Monitor.Wait(Sync);
                    }
                }
            }
        }

        protected virtual void OnProcess(T item)
        {
            if (Process != null)
                Process(this, new ProcessQueueEventArgs<T> {Item = item, Owner = this});
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~ProcessQueue()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Process = null;
                if (Processor != null)
                {
                    Processor = null;
                    //Get the processing thread out of waiting if it is.
                    lock (Sync)
                    {
                        Monitor.PulseAll(Sync);
                    }
                }
            }
        }
    }

    /*
    public class Tester
    {
        public static void Test()
        {
            var outputstrings = new ProcessQueue<string>();
            outputstrings.Process += outputstrings_Process;


            outputstrings.Enqueue("test");
     
            for(;;)
            {
                Thread.Sleep(100);
            }
        }
        public static void outputstrings_Process(object sender, ProcessQueueEventArgs<string> e)
        {
            Debug.WriteLine(e.Item);
        }
    }
     */
}
