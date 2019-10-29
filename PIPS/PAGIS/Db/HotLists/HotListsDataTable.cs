using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

using PIPS.PAGIS.Db.DataFiles.DataTables;
using PIPS.PAGIS.Db.SQLite;

namespace PIPS.PAGIS.Db.HotLists
{
	/// <summary>
	/// Summary description for HotListsDataTable.
	/// </summary>
	public class HotListsDataTable : DataTableBase
	{
        private PIPS.PAGIS.Db.HotLists.FuzzyLogic fuzzyFilter;
        private Hashtable hotlists;
	
		public HotListsDataTable()
		{
			this.Columns.Add(new StringDataColumn("name"));
			this.Columns.Add(new IntDataColumn("priority"));
			this.Columns.Add(new DateTimeDataColumn("timestamp"));
			this.Columns.Add(new ColorDataColumn("color"));
			this.Columns.Add(new BooleanDataColumn("covert"));
			this.Columns.Add(new StringDataColumn("alarm"));
			this.Columns.Add(new BooleanDataColumn("active"));
			this.Columns.Add(new StringDataColumn("file"));
			this.Columns.Add(new BooleanDataColumn("whitelist"));
		    this.Columns.Add(new LongDataColumn("bossid"));
		    this.Columns.Add(new ByteArrayDataColumn("sound"));
			
			this.hotlists = new Hashtable();
            this.fuzzyFilter = new PIPS.PAGIS.Db.HotLists.FuzzyLogic();
        }

        public PIPS.PAGIS.Db.HotLists.FuzzyLogic FuzzyFilter
        {
            get
            {
                return this.fuzzyFilter;
            }
        }

        public override string Name {
			get {
				return "t_hotlists";
			}
		}

		public override int IndexColumn {
			get {
				return 0;
			}
		}

		public HotListsDataEvent CreateHotListsDataEvent() {
			return new HotListsDataEvent(this);
		}

		public override DataEvent CreateDataEvent() {
			return this.CreateHotListsDataEvent();
		}

		public void ClearManualReports() {
			this.ManualReportsHotList.Clear();
		}

		public void AddManualReport(string vrm, string pncid, string information) {
			this.ManualReportsHotList.Save(vrm, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, pncid, information, -1);
		}

        public long[] SelectIDsCovertListsFiltered()
        {
            return base.SelectIDsWhere("covert == 'False'", "priority");
        }

        public List<HotListsDataEvent> SelectHotListsCovertListsFiltered()
        {
            List<HotListsDataEvent> filteredHotlists = new List<HotListsDataEvent>();
            var ids = SelectIDsCovertListsFiltered();
            foreach (var id in ids)
            {
                filteredHotlists.Add(SelectByID(id) as PIPS.PAGIS.Db.HotLists.HotListsDataEvent);
            }

            return filteredHotlists;
        }

		private PIPS.PAGIS.Db.HotLists.HotList ManualReportsHotList {
			get {
				long[] ids = this.SelectIDsByIndexColumn("Manual Reports");
				if((ids == null) || (ids.Length <= 0))
					return this.hotlists[this.CreateManualReports()] as PIPS.PAGIS.Db.HotLists.HotList;
				HotListsDataEvent mr = this.SelectByID(ids[0]) as HotListsDataEvent;
				mr.Timestamp = DateTime.Now;
				mr.Save();
				return this.hotlists[ids[0]] as PIPS.PAGIS.Db.HotLists.HotList;
			}
		}

		private HitsDataEvent CopyHotlistToHit(long hotlist_id, HotListDataEvent hotlist) {
			HitsDataEvent hit = (this.DataFile.DataRepository as SystemRepository).Events.Reads.Hits.CreateHitsDataEvent();
			HotListsDataEvent hl =this.SelectByID(hotlist_id) as HotListsDataEvent;
			hit.Alarm = hl.Alarm;
			hit.DisplayColor = hl.DisplayColor;
			hit.Field1 = hotlist.Field1;
			hit.Field2 = hotlist.Field2;
			hit.Field3 = hotlist.Field3;
			hit.Field4 = hotlist.Field4;
			hit.Field5 = hotlist.Field5;
			hit.HotList = hl.Name;
			hit.Information = hotlist.Information;
			hit.IsCovert = hl.IsCovert;
			hit.PNCID = hotlist.PNCID;
			hit.Priority = hl.Priority;
			hit.VRM = hotlist.VRM;
		    hit.Alerting = hl.Alerting;
			return hit;
		}

		private HitsDataEvent CopyWhitelistToHit(long hotlist_id, ReadsDataEvent read) {
			HitsDataEvent hit = (this.DataFile.DataRepository as SystemRepository).Events.Reads.Hits.CreateHitsDataEvent();
			HotListsDataEvent hl =this.SelectByID(hotlist_id) as HotListsDataEvent;
			hit.Alarm = hl.Alarm;
			hit.DisplayColor = hl.DisplayColor;
			hit.Field1 = string.Empty;
			hit.Field2 = string.Empty;
			hit.Field3 = string.Empty;
			hit.Field4 = string.Empty;
			hit.Field5 = string.Empty;
			hit.HotList = hl.Name;
			hit.Information = string.Empty;
			hit.IsCovert = hl.IsCovert;
			hit.PNCID = string.Empty;
			hit.Priority = hl.Priority;
			hit.VRM = read.VRM;
            hit.Alerting = hl.Alerting;
            return hit;
		}

		public DataEventCollection ProcessPncid(string pncid) {
			DataEventCollection r = new DataEventCollection();
			ArrayList results = new ArrayList();
			long[] ids = this.SelectIDsWhere("active = 'True'");
			if(ids != null) {
				for(int i = 0; i < ids.Length; i++) {
					try {
						results.Add(this.GetHotlist(ids[i]).BeginFindPncid(pncid));
					} catch (Exception ex) {
						//PIPS.Logger.Exception(ex);
						results.Add(null);
					}
				}
				for(int i = 0; i < ids.Length; i++) {
					try {
						if(results[i] != null) {
							IAsyncResult result = results[i] as IAsyncResult;
							DataEventCollection pncids = this.GetHotlist(ids[i]).EndFindPncid(result);
							foreach(PIPS.PAGIS.Db.HotLists.HotListDataEvent hotlist in pncids) {
								r.Add(this.CopyHotlistToHit(ids[i], hotlist));
							}
						}
					} catch {}
				}
			}
			return r;
		}

		private StringHandler msgCallback;
		public void Save(HotListsDataEvent hotlist, StringHandler msgCallback) {
			this.msgCallback = msgCallback;
			try {
				this.Save(hotlist);
			} finally {
				this.msgCallback = null;
			}
		}

        //IPS - HL
		public List<string> ProcessRead_IPS(string read) {
            List<string> result_text = new List<string>();

            DateTime start = DateTime.Now;
			ArrayList results = new ArrayList();
			long[] ids = this.SelectIDsWhere("active = 'True' and whitelist = 'False'", "priority", true);
#if DEBUG
            //ids = new long[] { 7 };
#endif
            if(ids != null) {
				string field1 = this.FuzzyFilter.Filter(read);
				for(int i = 0; i < ids.Length; i++) {
					try {
						results.Add(this.GetHotlist(ids[i]).BeginFindField1(field1));
					} catch (Exception ex) {
						//PIPS.Logger.Exception(ex);
						results.Add(null);
					}
				}

				for(int i = 0; i < ids.Length; i++) {
					try {
						if(results[i] != null) {
							IAsyncResult result = results[i] as IAsyncResult;
							DataEventCollection vrms = this.GetHotlist(ids[i]).EndFindField1(result);
                            foreach (PIPS.PAGIS.Db.HotLists.HotListDataEvent hotlist in vrms)
                            {
                                HotListsDataEvent hl = this.SelectByID(ids[i]) as HotListsDataEvent;
                                result_text.Add(
                                    ids[i].ToString() + "|" + //HL id
                                    hl.Alarm + "|" +
                                    hl.DisplayColor + "|" +
                                    hotlist.Field1 + "|" +
                                    hotlist.Field2 + "|" +
                                    hotlist.Field3 + "|" +
                                    hotlist.Field4 + "|" +
                                    hotlist.Field5 + "|" +
                                    hl.Name + "|" +
                                    hotlist.Information + "|" +
                                    hl.IsCovert + "|" +
                                    hotlist.PNCID + "|" +
                                    hl.Priority + "|" +
                                    hotlist.VRM + "|" +
                                    hl.Alerting);
                            }
                        }
					} catch (Exception ex) {
						//PIPS.Logger.Exception(ex);
					}
				}
			}
			//if (!read.Permit) {
			//	ProcessWhitelist(read);
			//}
			//PIPS.Logger.WriteLine("HotListsDataTable.ProcessRead({0})", (DateTime.Now - start).TotalMilliseconds);
            return result_text;
        }

        public List<string> ProcessRead(string read)
        {
            List<string> result_text = new List<string>();
            DateTime start = DateTime.Now;
            ArrayList results = new ArrayList();
            long[] ids = this.SelectIDsWhere("active = 'True' and whitelist = 'False'", "priority", true);

#if DEBUG
            ids = new long[] { 7 };
#endif

            if (ids != null)
            {
                string vrm = this.FuzzyFilter.Filter(read);
                for (int i = 0; i < ids.Length; i++) {
                    try {
                        results.Add(this.GetHotlist(ids[i]).BeginFindVrm(vrm));
                    }
                    catch (Exception ex) {
                        //PIPS.Logger.Exception(ex);
                        results.Add(null);
                    }
                }
                for (int i = 0; i < ids.Length; i++) {
                    try {
                        if (results[i] != null) {
                            IAsyncResult result = results[i] as IAsyncResult;
                            DataEventCollection vrms = this.GetHotlist(ids[i]).EndFindVrm(result);
                            foreach (PIPS.PAGIS.Db.HotLists.HotListDataEvent hotlist in vrms)
                            {

                                HotListsDataEvent hl = this.SelectByID(ids[i]) as HotListsDataEvent;
                                result_text.Add(
                                    ids[i].ToString() + "|" + //HL id
                                    hl.Alarm + "|" +
                                    hl.DisplayColor + "|" +
                                    hotlist.Field1 + "|" +
                                    hotlist.Field2 + "|" +
                                    hotlist.Field3 + "|" +
                                    hotlist.Field4 + "|" +
                                    hotlist.Field5 + "|" +
                                    hl.Name + "|" +
                                    hotlist.Information + "|" +
                                    hl.IsCovert + "|" +
                                    hotlist.PNCID + "|" +
                                    hl.Priority + "|" +
                                    hotlist.VRM + "|" +
                                    hl.Alerting);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        //PIPS.Logger.Exception(ex);
                    }
                }
            }
            //PIPS.Logger.WriteLine("HotListsDataTable.ProcessRead({0})", (DateTime.Now - start).TotalMilliseconds);
            return result_text;
        }

        public void ProcessRead(ReadsDataEvent read)
        {
            DateTime start = DateTime.Now;
            ArrayList results = new ArrayList();
            long[] ids = this.SelectIDsWhere("active = 'True' and whitelist = 'False'", "priority", true);

            if (ids != null)
            {
                string vrm = this.FuzzyFilter.Filter(read.VRM);
                for (int i = 0; i < ids.Length; i++)
                {
                    try
                    {
                        results.Add(this.GetHotlist(ids[i]).BeginFindVrm(vrm));
                    }
                    catch (Exception ex)
                    {
                        //PIPS.Logger.Exception(ex);
                        results.Add(null);
                    }
                }
                for (int i = 0; i < ids.Length; i++)
                {
                    try
                    {
                        if (results[i] != null)
                        {
                            IAsyncResult result = results[i] as IAsyncResult;
                            DataEventCollection vrms = this.GetHotlist(ids[i]).EndFindVrm(result);
                            foreach (PIPS.PAGIS.Db.HotLists.HotListDataEvent hotlist in vrms)
                            {

                                read.LinkedDataEvents(0).Add(this.CopyHotlistToHit(ids[i], hotlist));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        //PIPS.Logger.Exception(ex);
                    }
                }
            }
            if (!read.Permit)
            {
                ProcessWhitelist(read);
            }
            //PIPS.Logger.WriteLine("HotListsDataTable.ProcessRead({0})", (DateTime.Now - start).TotalMilliseconds);

        }

        public void ProcessWhitelist(ReadsDataEvent read) 
		{
			DateTime start = DateTime.Now;
			ArrayList results = new ArrayList();
			long[] ids = this.SelectIDsWhere("active = 'True' and whitelist = 'True'", "priority", true);
			if(ids != null) 
			{
				for(int i = 0; i < ids.Length; i++) 
				{
					try 
					{
						results.Add(this.GetHotlist(ids[i]).BeginFindVrm(read.VRM));
					} 
					catch (Exception ex) 
					{
						//PIPS.Logger.Exception(ex);
						results.Add(null);
					}
				}
				int strikes = 0;
				long id = -1;
				ReadsDataEvent violation = null;
				for(int i = 0; i < ids.Length; i++) 
				{
					try 
					{
						IAsyncResult result = results[i] as IAsyncResult;
						DataEventCollection vrms = this.GetHotlist(ids[i]).EndFindVrm(result);
						if(vrms.Count == 0) 
						{
							if (null == violation) 
							{
								id = ids[i];
								violation = read;
							}
							strikes++;
						} 
					} 
					catch (Exception ex) 
					{
						//PIPS.Logger.Exception(ex);
					}
				}
				if (strikes == ids.Length && null != violation) 
				{
					//PIPS.Logger.WriteLine("Whitelist violation('{0}')", violation.VRM);
					read.LinkedDataEvents(0).Add(this.CopyWhitelistToHit(id, violation));
				}
			}
			//PIPS.Logger.WriteLine("HotListsDataTable.ProcessWhitelist({0})", (DateTime.Now - start).TotalMilliseconds);
		}

		private long CreateManualReports() {
			HotListsDataEvent manualReports = this.CreateHotListsDataEvent();
			manualReports.DisplayColor = Color.Blue;
			manualReports.Alarm = "MED";
			manualReports.Name = "Manual Reports";
			manualReports.Priority = 500;
			manualReports.IsActive = true;
			manualReports.IsCovert = false;
			manualReports.SourceFile = string.Empty;
			manualReports.Timestamp = DateTime.Now;
			manualReports.IsWhitelist = false;
		    manualReports.Alerting = true;
			this.Save(manualReports);
			return manualReports.ID;
		}

		protected override void OnInitialized() {
			long[] ids = this.SelectIDs();
			if(ids != null) {
				foreach(long id in ids) {
					HotListsDataEvent ev = (HotListsDataEvent)this.SelectByID(id);
					//PIPS.Logger.WriteLine(false, "HotListsDataTable.Init({0}, {1})", ev.Name, ev.Alarm);
					this.hotlists[ev.ID] = new PIPS.PAGIS.Db.HotLists.HotList(this.DataFile.DataRepository.DataDirectory + "\\" + ev.ID.ToString());
				}
			}
			this.hotlists[(long)0] = new PIPS.PAGIS.Db.HotLists.HotList(this.DataFile.DataRepository.DataDirectory + "\\0");
			base.OnInitialized ();
		}

		public override void Dispose() {
			if(hotlists != null) {
				try {
					foreach(PIPS.PAGIS.Db.HotLists.HotList hotlist in hotlists.Values) {
						try {
							hotlist.Dispose();
						} catch {}
					}
					hotlists.Clear();
				} catch {}
				hotlists = null;
			}
			base.Dispose();
		}

		protected override void OnDeleting(DataEvent ev) {
			this.GetHotlist(ev.ID).Clear();
			this.hotlists.Remove(ev.ID);
			string source = (ev as HotListsDataEvent).SourceFile;
			if((source != null) && (source != string.Empty)) {
				if(System.IO.File.Exists(source))
					System.IO.File.Delete(source);
				if(System.IO.File.Exists(System.IO.Path.ChangeExtension(source, ".wav")))
					// Added extra exception handling for file delete since occasionally the file was already locked when deleting
					// which caused an error and did not allow hotlist processign to continue.
					// Now it will try to delete the file, wait 500 milliseconds, try again, then if still not successful it will log error and move on.
					// This shouldn't cause a file when downloading a new sound file because the new file will overwrite the existing.
					// There could also be file locking issues at that point, but that will need to be addressed at that point.
					try
					{
						System.IO.File.Delete(System.IO.Path.ChangeExtension(source, ".wav"));
					}
					catch(System.IO.IOException ioEx)
					{
						//PIPS.Logger.Exception(ioEx);
						//PIPS.Logger.WriteLine("Waiting 1 second...");
						System.Threading.Thread.Sleep(1000);
						//PIPS.Logger.WriteLine("Attempting to delete .WAV file again");
						try
						{
							System.IO.File.Delete(System.IO.Path.ChangeExtension(source, ".wav"));
						}
						catch(Exception ex)
						{
							//PIPS.Logger.WriteLine("Failed to delete .WAV file a second time.  Continuing with hotlist processing.");
							////PIPS.Logger.Exception(ex);
						}
					}
				base.OnDeleting(ev);
			}
		}

		public PIPS.PAGIS.Db.HotLists.HotList GetHotlist(long id) {
			return this.hotlists[id] as PIPS.PAGIS.Db.HotLists.HotList;
		}

		protected override void OnInserted(DataEvent ev) {
			this.hotlists[ev.ID] = new PIPS.PAGIS.Db.HotLists.HotList(this.DataFile.DataRepository.DataDirectory + "\\" + ev.ID.ToString());
			base.OnInserted(ev);
		}

		protected override void OnUpdated(DataEvent ev) {
			this.GetHotlist(ev.ID).ImportDeltas((ev as HotListsDataEvent).SourceFile, this.msgCallback);
			base.OnUpdated(ev);
		}
	}

	public class HotListsDataEvent : DataEvent {

		public HotListsDataEvent(HotListsDataTable table) : base(table) {}

		public void Save(StringHandler msgCallback) {
		if(this.Table is HotListsDataTable) {
				(this.Table as HotListsDataTable).Save(this, msgCallback);
			}
		}

		public string Name {
			get {
				return (string)this[0];
			}
			set {
				this[0] = value;
			}
		}

		public int Priority {
			get {
				return (int)this[1];
			}
			set {
				if (value < 0) {
					this[1] = value * -1;
					this[8] = true;
				} else {
					this[1] = value;
				}
			}
		}

		public DateTime Timestamp {
			get {
				return (DateTime)this[2];
			}
			set {
				this[2] = value;
			}
		}

		public Color DisplayColor {
			get {
				return (Color)this[3];
			}
			set {
				this[3] = value;
			}
		}

		public bool IsCovert {
			get {
				return (bool)this[4];
			}
			set {
				this[4] = value;
			}
		}

		public string Alarm {
			get {
				return (string)this[5];
			}
			set {
				this[5] = value;
			}
		}

		public bool IsActive {
			get {
				return (bool)this[6];
			}
			set {
				this[6] = value;
			}
		}

		public string SourceFile {
			get {
				return (string)this[7];
			}
			set {
				this[7] = value;
			}
		}

		public bool IsWhitelist {
			get {
				return (bool)this[8];
			}
			set {
				this[8] = value;
			}
		}

	    public long BossID
	    {
	        get { return (long) this[9]; }
            set
            {
                if (this[9] == null)
                    this[9] = (-1);

                this[9] = value;
            }
	    }

	    public byte[] Sound
	    {
            get { return (byte[]) this[10]; }
            set {this[10] = value;}
	    }

        public bool Alerting
        {
            get
            {
                return ExternalAlertingSettingGet(this.Name);
            }
            set
            {
                ExternalAlertingSettingSet(this.Name, value);
            }
        }

		public bool Trigger 
		{
			get
			{
				IniFile ini = new IniFile(Environment.CurrentDirectory + @"\Triggers.ini");
				return ini.ReadBoolean("Hotlists", this.Name, false);
			}
			set 
			{
				IniFile ini = new IniFile(Environment.CurrentDirectory + @"\Triggers.ini");
				ini.WriteBoolean("Hotlists", this.Name, value);
			}
		}

	    public bool ExternalAlertingSettingGet(string hotlistName)
	    {
            IniFile ini = new IniFile(Environment.CurrentDirectory + @"\Alerting.ini");
            return ini.ReadBoolean("Hotlists", hotlistName, true);
	    }
        public void ExternalAlertingSettingSet(string hotlistName, bool value)
        {
            if (string.IsNullOrEmpty(hotlistName)) return;

            IniFile ini = new IniFile(Environment.CurrentDirectory + @"\Alerting.ini");
            ini.WriteBoolean("Hotlists", hotlistName, value);
        }
    }
}
