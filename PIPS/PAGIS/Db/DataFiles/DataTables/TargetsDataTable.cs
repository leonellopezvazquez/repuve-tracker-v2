using System.Data;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using PIPS.PAGIS.Db.SQLite;

namespace PIPS.PAGIS.Db.DataFiles.DataTables
{
	/// <summary>
	/// Summary description for TargetsDataTable.
	/// </summary>
	public class TargetsDataTable : DataTableBase
	{
		public TargetsDataTable()
		{
			this.Columns.Add(new StringDataColumn("pncid"));
			this.Columns.Add(new StringDataColumn("name"));
			this.Columns.Add(new StringDataColumn("address"));
			this.Columns.Add(new DateTimeDataColumn("dob"));
			this.Columns.Add(new StringDataColumn("birthplace"));
			this.Columns.Add(new StringDataColumn("ethnicity"));
			this.Columns.Add(new StringDataColumn("category"));
			this.Columns.Add(new StringDataColumn("warning"));
			this.Columns.Add(new StringDataColumn("information"));
			this.Columns.Add(new StringDataColumn("login"));
			this.Columns.Add(new ByteArrayDataColumn("image"));
		    this.Columns.Add(new LongDataColumn("bossid"));
		}

		public override string Name {
			get {
				return "t_targets";
			}
		}

		public override int IndexColumn {
			get {
				return 0;
			}
		}

		public TargetsDataEvent CreateTargetsDataEvent() {
			return new TargetsDataEvent(this);
		}

		public override DataEvent CreateDataEvent() {
			return this.CreateTargetsDataEvent();
		}
        public List<TargetsDataEvent> SelectTargets()
        {
            var ids = base.SelectIDs();

            var list = ids.Select(id => SelectByID(id) as TargetsDataEvent).ToList();

            return list;
        }


	}
}
