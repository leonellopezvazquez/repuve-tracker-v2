using System;
using PIPS.PAGIS.Db.SQLite;


namespace PIPS.PAGIS.Db.DataFiles.DataTables
{
	/// <summary>
	/// Summary description for AuditsDataTable.
	/// </summary>
	public class AuditsDataTable : DataTableBase
	{
		public AuditsDataTable()
		{
			this.Columns.Add(new DateTimeDataColumn("timestamp"));
			this.Columns.Add(new StringDataColumn("location"));
			this.Columns.Add(new DoubleDataColumn("latitude"));
			this.Columns.Add(new DoubleDataColumn("longitude"));
			this.Columns.Add(new StringDataColumn("information"));
			this.Columns.Add(new ByteArrayDataColumn("picture"));
			this.Columns.Add(new BooleanDataColumn("synced"));
			this.Columns.Add(new StringDataColumn("login"));
		}

		public override string Name {
			get {
				return "t_audits";
			}
		}

		public AuditsDataEvent CreateAuditsDataEvent() {
			return new AuditsDataEvent(this);
		}

		public override DataEvent CreateDataEvent() {
			return this.CreateAuditsDataEvent();
		}

		public long[] SelectNonSyncedIDs() {
			return this.SelectIDsWhere("synced = 'False'");
		}

		public AuditsDataEvent Save(string information) {
			return this.Save(information, null);
		}

		public virtual AuditsDataEvent Save(string information, byte[] picture) {
			if(this.DataFile.DataRepository is SystemRepository) {
				AuditsDataEvent ev = this.CreateAuditsDataEvent();

                var info = string.Format("{0} on device: [{1}], user: [{2}]", information, Environment.MachineName, "RFID");
                ev.Information = info;
				ev.Latitude = 0.0;
				ev.Longitude = 0.0;
				ev.Timestamp = DateTime.Now;
				ev.Location = "LOCATION";
				ev.IsSynced = false;
				ev.Picture = picture;
				ev.Login = "RFID";
				this.Save(ev);
				return ev;
			}
			return null;
		}
		public long SyncedCount {
			get {
				return this.GetCount("synced = 'True'");
			}
		}
		public long[] SelectSyncedIDs() {
			return this.SelectIDsWhere("synced = 'True'");
		}
	}
}
