using System;
using System.Drawing;
using System.Collections.Generic;
using PIPS.PAGIS.Db.SQLite;


namespace PIPS.PAGIS.Db.DataFiles.DataTables
{
	/// <summary>
	/// Summary description for HitsDataTable.
	/// </summary>
	public class HitsDataTable : DataTableBase {

		public HitsDataTable() {
			this.Columns.Add(new LongDataColumn("read_id"));
			this.Columns.Add(new IntDataColumn("priority"));
			this.Columns.Add(new StringDataColumn("hotlist"));
			this.Columns.Add(new ColorDataColumn("color"));
			this.Columns.Add(new BooleanDataColumn("covert"));
			this.Columns.Add(new StringDataColumn("alarm"));
			this.Columns.Add(new StringDataColumn("vrm"));
			this.Columns.Add(new StringDataColumn("field1"));
			this.Columns.Add(new StringDataColumn("field2"));
			this.Columns.Add(new StringDataColumn("field3"));
			this.Columns.Add(new StringDataColumn("field4"));
			this.Columns.Add(new StringDataColumn("field5"));
			this.Columns.Add(new StringDataColumn("pncid"));
			this.Columns.Add(new StringDataColumn("information"));
		    this.Columns.Add(new BooleanDataColumn("alerting"));
		}

        /// <summary>
        /// Gets count of all hits which haven't been set to misread
        /// </summary>
        public override long Count
        {
            get { return base.GetCountCustom(@" JOIN t_reads on  t_reads.id = t_hits.read_id 
                                                WHERE t_reads.misread == 'False'"); 
                }
        }

        /// <summary>
        /// Gets count of all hits with missing dispositions which haven't been set to misread
        /// </summary>
	    public long MissingDispositionCount
	    {
            get
            {
                return base.GetCountCustom(@" JOIN t_reads on  t_reads.id = t_hits.read_id 
                                          LEFT JOIN t_read_dispositions AS dispositions ON dispositions.read_id = t_hits.read_id 
                                          WHERE t_reads.misread == 'False'
                                                AND
                                                dispositions.read_id isnull
                                                AND
                                                t_hits.covert == 'False'"); 
            }
	    }



		public override string Name {
			get {
				return "t_hits";
			}
		}

		public override int SortColumn {
			get {
				return 1;
			}
		}

		public override int IndexColumn {
			get {
				return 0;
			}
		}

        public HitsDataEvent Save(ReadsDataEvent read, HitsDataEvent hit, string information)
        {
            if (this.DataFile.DataRepository is SystemRepository)
            {
                var start = DateTime.Now;
                hit.Information = information;
                this.Save(hit);
                //PIPS.Logger.WriteLine("HitsDataTable: new hit save = {0} ms", (DateTime.Now - start).TotalMilliseconds);
                // this read already exists so we need to resynce it with boss
                var indy = read.Hits.FindIndex(
                    h => h.HotList == hit.HotList);
                if (indy !=-1) read.Hits[indy].Information = information;
                read.SaveAndQueueForSync();
                return hit;
            }
            return null;
        }

	    public HitsDataEvent Save(long read_id, string hotlist, string information) {
			if(this.DataFile.DataRepository is SystemRepository) {
				var start = DateTime.Now;
				ReadsDataEvent read = (this.DataFile.DataRepository as SystemRepository).Events.Reads.SelectByID(read_id) as ReadsDataEvent;
				HitsDataEvent hit = this.CreateHitsDataEvent();
				hit.Alarm = "MED";
				hit.DisplayColor = Color.Blue;
				hit.Field1 = hit.Field2 = hit.Field3 = hit.Field4 = hit.Field5 = string.Empty;
				hit.HotList = hotlist;
				hit.IsCovert = false;
				hit.PNCID = string.Empty;
				hit.Priority = 500;
				hit.ReadID = read.ID;
				hit.VRM = read.VRM;
			    var result = Save(read, hit, information);
				//PIPS.Logger.WriteLine("HitsDataTable: new hit save = {0} ms", (DateTime.Now - start).TotalMilliseconds);
				return result;
			}
			return null;
		}

		protected override void OnInserted(DataEvent ev) {
			HitsDataEvent hit = ev as HitsDataEvent;
			base.OnInserted (ev);
		}

		public HitsDataEvent CreateHitsDataEvent() {
			return new HitsDataEvent(this);
		}

		public override DataEvent CreateDataEvent() {
			return this.CreateHitsDataEvent();
		}
	}
}
