using System;
using System.Threading;

namespace PIPS.PAGIS.Db.SQLite
{
    /// <summary>
    /// Summary description for DataCommand.
    /// </summary>
    public abstract class DataCommandBase : IDisposable, IAsyncResult
    {
        private ManualResetEvent trigger;
        private bool dispose;

        public DataCommandBase(bool dispose)
        {
            this.trigger = new ManualResetEvent(false);
            this.dispose = dispose;
        }

        public DataCommandBase() : this(false) { }

        public bool Wait()
        {
            return this.trigger.WaitOne();
        }

        public void Execute(DataFileBase datafile)
        {
            try
            {
                this.InternalExecute(datafile);
            }
            finally
            {
                if (this.trigger != null)
                    this.trigger.Set();
                if (this.dispose)
                    this.Dispose();
            }
        }
        protected abstract void InternalExecute(DataFileBase datafile);

        public abstract object Result { get; }

        #region IDisposable Members

        public virtual void Dispose()
        {
            if (this.trigger != null)
            {
                try
                {
                    this.trigger.Close();
                }
                catch { }
                this.trigger = null;
            }
        }

        #endregion

        #region IAsyncResult Members

        public object AsyncState
        {
            get
            {
                return this.Result;
            }
        }

        public bool CompletedSynchronously
        {
            get
            {
                return false;
            }
        }

        public WaitHandle AsyncWaitHandle
        {
            get
            {
                return this.trigger;
            }
        }

        public bool IsCompleted
        {
            get
            {
                return this.trigger.WaitOne(1, false);
            }
        }

        #endregion
    }
}