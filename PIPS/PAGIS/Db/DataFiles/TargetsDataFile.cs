using System;
using System.Collections;
using PIPS.PAGIS.Db.SQLite;


namespace PIPS.PAGIS.Db.DataFiles {
	/// <summary>
	/// Summary description for TargetsDataTable.
	/// </summary>
	public class TargetsDataFile : DataFileBase {
		public TargetsDataFile() {
			this.Tables.Add(new DataFiles.DataTables.TargetsDataTable());
		}

		public override string FileName {
			get {
				return "Targets";
			}
		}

		public DataFiles.DataTables.TargetsDataTable Targets {
			get {
				return this.Tables[0] as DataFiles.DataTables.TargetsDataTable;
			}
		}
	}
}
