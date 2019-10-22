using System;
using System.Data;
using System.IO;
using System.Threading;
using System.Collections;

using PIPS.PAGIS.Db.SQLite;

namespace PIPS.PAGIS.Db.HotLists
{
	/// <summary>
	/// Summary description for HotListDataTable.
	/// </summary>
	public class HotListDataTable : DataTableBase
	{
		private int index;

		protected override bool IsCountInitialized {
			get {
				return false;
			}
		}

		public HotListDataTable()
		{
			this.Columns.Add(new StringDataColumn("vrm"));
			this.Columns.Add(new StringDataColumn("field1"));
			this.Columns.Add(new StringDataColumn("field2"));
			this.Columns.Add(new StringDataColumn("field3"));
			this.Columns.Add(new StringDataColumn("field4"));
			this.Columns.Add(new StringDataColumn("field5"));
			this.Columns.Add(new StringDataColumn("pncid"));
			this.Columns.Add(new StringDataColumn("information"));
            this.Columns.Add(new LongDataColumn("bossID"));
		}

		public override int IndexColumn {
			get {
				return 1; //IPS
			}
		}

		public HotListDataEvent CreateHotListDataEvent() {
			return new HotListDataEvent(this);
		}

		public override DataEvent CreateDataEvent() {
			return this.CreateHotListDataEvent();
		}

		public override string Name {
			get {
				return "t_hotlist";
			}
		}

	    public long[] SelectIDsByVRM(string vrm)
	    {
            var ids =  this.SelectIDsWhere(string.Format("vrm LIKE '{0}'", vrm));
	        return ids;
	    }
	}
}
