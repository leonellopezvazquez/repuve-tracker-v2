using PIPS.PAGIS.Db.SQLite;


namespace PIPS.PAGIS.Db.DataFiles.DataTables
{
	/// <summary>
	/// Summary description for LoginsDataTable.
	/// </summary>
	public class LoginsDataTable : DataTableBase
	{
		private bool encrypt = false;

		public LoginsDataTable(bool encrypt)
		{
			this.encrypt = encrypt;
			this.Columns.Add(new StringDataColumn("login"));
			this.Columns.Add(new StringDataColumn("name"));
			this.Columns.Add(new StringDataColumn("password"));
			this.Columns.Add(new DateTimeDataColumn("expiration"));
			this.Columns.Add(new BooleanDataColumn("pncaccess"));
			this.Columns.Add(new BooleanDataColumn("syncaccess"));
			this.Columns.Add(new BooleanDataColumn("adminaccess"));
		    this.Columns.Add(new LongDataColumn("bossid"));
		}
		public override string Name {
			get {
				return "t_logins";
			}
		}

		public override int IndexColumn {
			get {
				return 0;
			}
		}

		public LoginsDataEvent CreateLoginsDataEvent() {
			return new LoginsDataEvent(this);
		}

		public override DataEvent CreateDataEvent() {
			return this.CreateLoginsDataEvent();
		}

		public bool Encrypt {
			get {
				return encrypt;
			}
		}

	}
}
