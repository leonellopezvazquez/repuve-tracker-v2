using System;
using System.Threading;
using System.Collections;

namespace PIPS.Utilities
{
	public delegate void EventQueueHandler(object eventdata);

	public class EventQueue : IDisposable {
		private Queue asyncQueue;
		private Queue syncQueue;
		private Thread runner;
		private AutoResetEvent trigger = new AutoResetEvent(false);
		private bool disposing = false;

		public event EventQueueHandler EventOccurred;

		public EventQueue() : this(ThreadPriority.Normal) {}
		public EventQueue(ThreadPriority priority) {
			asyncQueue = new Queue();
			syncQueue = Queue.Synchronized(asyncQueue);
			runner = new Thread(new ThreadStart(Run));
			runner.IsBackground = true;
			runner.Priority = priority;
			runner.Name = "EventQueue Thread";
			runner.Start();
		}

		public void AddEvent(object data) {
			if(disposing) return;
			syncQueue.Enqueue(data);
			trigger.Set();
		}

		private void Run() {
			while (true) {
				trigger.WaitOne(3000, true); //Wait for 3 seconds then poll
				while (syncQueue.Count > 0) {
					try {
						object data = syncQueue.Dequeue();
						if(this.EventOccurred != null)
							this.EventOccurred(data);
					} catch(Exception ex) {
						//Logger.Exception(ex);
					}
				}
			}
		}

		public void Dispose() {
			try {
				disposing = true;
				if (null != runner) {
					runner.Abort();
					runner = null;
				}
			} catch {}
		}
	}

}
