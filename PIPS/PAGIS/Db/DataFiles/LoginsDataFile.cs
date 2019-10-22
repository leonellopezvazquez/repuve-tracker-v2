using System;

using PIPS.PAGIS.Db.SQLite;

namespace PIPS.PAGIS.Db.DataFiles {
	/// <summary>
	/// Summary description for LoginsDataTable.
	/// </summary>
	public class LoginsDataFile : DataFileBase {
		public LoginsDataFile(bool encrypt) {
			this.Tables.Add(new DataFiles.DataTables.LoginsDataTable(encrypt));
		}

		public DataFiles.DataTables.LoginsDataTable Logins {
			get { return this.Tables[0] as DataFiles.DataTables.LoginsDataTable; }
		}

		public override string FileName {
			get {
				return "Logins";
			}
		}


	}

	
}
