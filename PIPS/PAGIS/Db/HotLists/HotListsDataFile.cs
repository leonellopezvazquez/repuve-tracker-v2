using System;
using System.Drawing;
using System.Collections;
using System.Threading;
using System.Text;

using PIPS.PAGIS.Db.SQLite;

namespace PIPS.PAGIS.Db.HotLists
{
	/// <summary>
	/// Summary description for HotListsDataTable.
	/// </summary>
	public class HotListsDataFile : DataFileBase
	{

		public HotListsDataFile()
		{
			this.Tables.Add(new HotListsDataTable());
		}

		public override string FileName {
			get {
				return "HotLists";
			}
		}

		public HotListsDataTable HotLists {
			get {
				return this.Tables[0] as HotListsDataTable;
			}
		}
	}

	
}
