using System;
using System.Data;
using System.IO;
using System.Threading;
using System.Collections;


namespace PIPS.PAGIS.Db.HotLists
{
	/// <summary>
	/// Summary description for HotListDataTable.
	/// </summary>
	public class PncidDataTable : HotListDataTable
	{

		public PncidDataTable() {}

		public override int IndexColumn {
			get {
				return 6;
			}
		}

		public override string Name {
			get {
				return "t_pncid";
			}
		}

	}
}
