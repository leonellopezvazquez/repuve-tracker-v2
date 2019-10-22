using System;
using System.Collections;

using PIPS.PAGIS.Db.SQLite;

namespace PIPS.PAGIS.Db.DataFiles
{
	/// <summary>
	/// Summary description for LogsDataTable.
	/// </summary>
	public class AuditsDataFile : DataFileBase
	{
		public AuditsDataFile()
		{
            this.Tables.Add(new DataFiles.DataTables.AuditsDataTable());
		}

		public override string FileName {
			get {
				return "Audits";
			}
		}

		public DataFiles.DataTables.AuditsDataTable Audits {
			get {
				return this.Tables[0] as DataFiles.DataTables.AuditsDataTable;
			}
		}

	}
}
