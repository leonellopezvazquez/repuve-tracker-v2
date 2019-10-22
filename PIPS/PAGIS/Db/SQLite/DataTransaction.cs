using System;
using System.Collections;

using System.Data.SQLite;

namespace PIPS.PAGIS.Db.SQLite
{
	/// <summary>
	/// Summary description for DataTransaction.
	/// </summary>
	public class DataTransaction : IDisposable
	{
		private ArrayList transactions;

		public DataTransaction()
		{
			this.transactions = new ArrayList();
		}

		internal void AddTransaction(SQLiteTransaction trans) {
			this.transactions.Add(trans);
		}

		#region IDisposable Members

		public void Dispose() {
			if(this.transactions != null) {
				try {
					foreach(SQLiteTransaction trans in this.transactions) {
						try {
							trans.Dispose();
						} catch {}
					}
					this.transactions.Clear();
				} catch {}
				this.transactions = null;
			}
		}

		#endregion

	}
}
