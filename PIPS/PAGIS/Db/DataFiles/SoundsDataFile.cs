using System;
using PIPS.PAGIS.Db.SQLite;


namespace PIPS.PAGIS.Db.DataFiles
{
	/// <summary>
	/// Summary description for SoundsDataFile.
	/// </summary>
	public class SoundsDataFile : DataFileBase
	{
		public SoundsDataFile()
		{
			this.Tables.Add(new DataFiles.DataTables.SoundsDataTable());
		}

		public DataFiles.DataTables.SoundsDataTable Sounds {
			get { return this.Tables[0] as DataFiles.DataTables.SoundsDataTable; }
		}
		
		public override string FileName {
			get {
				return "Sounds";
			}
		}

		public override void Dispose() {
			this.Sounds.PlaySound(DataTables.SoundEvents.ShuttingDown);
			System.Threading.Thread.Sleep(1000);
			base.Dispose ();
		}

	}
}
