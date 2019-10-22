using System;
using System.Collections;
using System.IO;
using System.Threading;
using System.Data;

using System.Data.SQLite;

namespace PIPS.PAGIS.Db.SQLite
{
	/// <summary>
	/// Summary description for DataFile.
	/// </summary>
	public abstract class DataFileBase : IDisposable
	{
		private bool isOpen;
		private SQLiteConnection connection;
		private SQLiteCommand command;
		private PIPS.Utilities.EventQueue commandQueue;
		private DataRepositoryBase repository;
		private DataTableCollection tables;

	    public DataTableCollection Tables {
			get { return this.tables; }
		}

		public DataRepositoryBase DataRepository {
			get {
				return this.repository;
			}
		}

	    public SQLiteTransaction Transaction { get; set; }

	    public DataFileBase()
		{
			this.isOpen = false;
			this.tables = new DataTableCollection();
			this.commandQueue = new PIPS.Utilities.EventQueue();
			this.commandQueue.EventOccurred += new PIPS.Utilities.EventQueueHandler(CommandOccurred);
		}

		public virtual int SQLiteVersion {
			get {
				return 3;
			}
		}

		public void BeginTransaction() {
			this.AddCommand(new TransactionCommand(this.connection, TransactionCommand.TransactionCommands.Begin));
		}

		public void CommitTransaction() {
			this.AddCommand(new TransactionCommand(this.connection, TransactionCommand.TransactionCommands.Commit));
		}

		public void RollbackTransaction() {
			this.AddCommand(new TransactionCommand(this.connection, TransactionCommand.TransactionCommands.Rollback));
		}

		private class TransactionCommand : DataCommandBase {
			private SQLiteConnection conn;
			private TransactionCommands cmd;

			public enum TransactionCommands {
				Begin,
				Commit,
				Rollback
			}

			public TransactionCommand(SQLiteConnection conn, TransactionCommands command) {
				this.conn = conn;
				this.cmd = command;
			}

			protected override void InternalExecute(DataFileBase datafile) 
			{
                switch (this.cmd)
                {
                    case TransactionCommands.Begin:
                        datafile.Transaction = this.conn.BeginTransaction();
                        break;
                    case TransactionCommands.Commit:
                        if (datafile.Transaction != null)
                        {
                            datafile.Transaction.Commit();
                            datafile.Transaction = null;
                        }
                        break;
                    case TransactionCommands.Rollback:
                        if (datafile.Transaction != null)
                        {
                            datafile.Transaction.Rollback();
                            datafile.Transaction = null;
                        }
                        break;
                }
            }

			public override object Result {
				get {
					return null;
				}
			}

		}


		public void Vacuum() {
			/*VacuumCommand vc = new VacuumCommand(this.conn);
			this.AddCommand(vc);
			vc.Wait();*/
		}

		#region Initialize

		public IAsyncResult BeginInitialize(string dir) {
			if(!this.isOpen) {
				InitializeCommand ic = new InitializeCommand(dir);
				this.AddCommand(ic);
				return ic;
			}
			return null;
		}

		public void EndInitialize(IAsyncResult result) {
			if((result != null) && (result is InitializeCommand)) {
				result.AsyncWaitHandle.WaitOne();
				if(result.AsyncState != null) {
					this.connection = result.AsyncState as SQLiteConnection;
					this.command = this.connection.CreateCommand();

					foreach(DataTableBase table in this.tables)
						table.Initialize(this);

					this.isOpen = true;
				} else {
					throw new DataException("Could not connect to the database '" + this.FileName + "'!");
				}
			}
		}

		public void Initialize(string dir) {
			this.EndInitialize(this.BeginInitialize(dir));
		}

		public void Initialize(DataRepositoryBase repository) {
			this.repository = repository;
			this.Initialize(this.repository.DataDirectory);
		}

		#endregion

		#region Connect

		/*private class ConnectCommand : DataCommandBase {
			private SQLiteConnection conn;

			private string file;
			private int version;

			public ConnectCommand(string file, int version) {
				this.file = file;
				this.version = version;
			}

			protected override void InternalExecute(DataFileBase datafile) {
				bool isNew = !File.Exists(file);
				SQLiteConnection c = new SQLiteConnection(string.Format("Data Source={0};New={1};Compress=False;Synchronous=Off;Version={2};AutoVacuum=False", file, isNew, version));
				try {
					c.Open();
					if (c.State != ConnectionState.Open) {
						try {
							c.Close();
						} catch {}
						c = null;
					}
					this.conn = c;
				} catch {
					try {
						c.Close();
					} catch {}
					throw;
				}
			}

			public override object Result {
				get {
					return this.conn;
				}
			}

		}

		private SQLiteConnection Connect(string dir) {
			string file = string.Format("{0}\\{1}.db", dir, this.FileName);
			try {
				using(DataCommandBase dcb = new ConnectCommand(file, this.SQLiteVersion)) {
					this.AddCommand(dcb);
					dcb.Wait();
					if(dcb.Result != null)
						return dcb.Result as SQLiteConnection;
					else
						throw new DataException("Connection not open!");
				}
			} catch {
				Thread.Sleep(250);
				try {
					if(File.Exists(file))
						File.Delete(file);
				} catch(Exception ex) {
					PIPS.Logger.Exception(ex);
				}
				throw;
			}
		}*/

		#endregion

		public abstract string FileName { get; }

		/*public SQLiteCommand CreateCommand() {
			if(this.conn == null)
				return null;
			return this.conn.CreateCommand();
		}*/

		public void AddCommand(DataCommandBase cmd) {
			this.commandQueue.AddEvent(cmd);
		}

		internal SQLiteCommand GetCommand(string cmdText, SQLiteParameter[] parameters) {
            //foreach (SQLiteParameter param in this.command.Parameters)
            //    param.Dispose();
		    this.command.Parameters.Clear();
			this.command.CommandText = cmdText;
			if(parameters != null) {
				foreach(SQLiteParameter parameter in parameters)
					this.command.Parameters.Add(parameter);
			}
			return this.command;
		}

		private void CommandOccurred(object eventdata) {
			DataCommandBase dcb = eventdata as DataCommandBase;
			try {
				dcb.Execute(this);
			} catch(ThreadAbortException) {
			} catch(Exception ex) {
				while(ex != null) {
					//PIPS.Logger.Exception(ex);
					ex = ex.InnerException;
				}
			}
		}

		#region IDisposable Members

		public virtual void Dispose() {
			if(this.commandQueue != null) {
				try {
					this.commandQueue.Dispose();
				} catch {}
				this.commandQueue = null;
			}
			if(this.tables != null) {
				try {
					foreach(DataTableBase table in this.tables) {
						try {
							table.Dispose();
						} catch {}
					}
					this.tables.Clear();
				} catch {}
				this.tables = null;
			}
			if(this.command != null) {
				try {
					this.command.Dispose();
				} catch {}
				this.command = null;
			}
			if(this.connection != null) {
				try {
					this.connection.Dispose();
				} catch {}
				this.connection = null;
			}
		}

		#endregion

	}
}
