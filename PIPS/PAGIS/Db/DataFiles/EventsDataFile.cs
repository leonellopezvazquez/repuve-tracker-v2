using System;
using PIPS.PAGIS.Db.SQLite;


namespace PIPS.PAGIS.Db.DataFiles {
	/// <summary>
	/// Summary description for ReadsDataTable.
	/// </summary>
	public class EventsDataFile : DataFileBase {
		public DataFiles.DataTables.ReadsDataTable Reads {
			get { return this.Tables[0] as DataFiles.DataTables.ReadsDataTable; }
		}

		public EventsDataFile() {
			this.Tables.Add(new DataFiles.DataTables.ReadsDataTable());
		}

		public override string FileName {
			get {
				return "Events";
			}
		}
	}
}