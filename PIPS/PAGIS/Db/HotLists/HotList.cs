using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Collections;

using PIPS.PAGIS.Db.SQLite;

namespace PIPS.PAGIS.Db.HotLists
{
	/// <summary>
	/// Summary description for Hotlist.
	/// </summary>
	public class HotList : IDisposable
	{
		private string directory;
		private DataFileCollection dataFiles;
		private bool available;
		private PIPS.Utilities.EventQueue eq;
		private HotListDataEvent insertEvent;

		public HotList(string directory)
		{
			this.available = true;
			DateTime start = DateTime.Now;

			this.dataFiles = new DataFileCollection();
			this.directory = directory;
			if(Directory.Exists(this.directory)) {
				string[] files = Directory.GetFiles(this.directory, HotListDataFile.SearchPattern);
				if(files != null) {
					foreach(string file in files) {
						this.dataFiles.Add(new HotListDataFile(file));
					}
					ArrayList results = new ArrayList();
					foreach(HotListDataFile hl in this.dataFiles)
						results.Add(hl.BeginInitialize());
					for(int i = 0; i < results.Count; i++)
						this.dataFiles[i].EndInitialize(results[i] as IAsyncResult);
				}
			}

			this.eq = new PIPS.Utilities.EventQueue();
			this.eq.EventOccurred += new PIPS.Utilities.EventQueueHandler(FindCallback);

			insertEvent = this.GetLastDataFile().HotList.CreateHotListDataEvent();
			//PIPS.Logger.WriteLine(false, "Hotlist.Initialize({0}, {1})", (DateTime.Now - start).TotalMilliseconds, directory);
		}

		#region Import / Insert / Remove

		public void Import(string pipscsv, StringHandler msgCallback) {
            if ((pipscsv != null) && (pipscsv != string.Empty))
            {

                try
                {
                    available = false;
                    CSV csv = new CSV(PIPS.Utilities.FileEncrypter.IsEncrypted(pipscsv) ? PIPS.Utilities.FileEncrypter.GetStreamReader(pipscsv) : new StreamReader(pipscsv));
                    try
                    {
                        //PIPS.Logger.WriteLine("HotList.Import({0})", pipscsv);
                        string[] fields;
                        int count = 0;
                        HotListDataFile hlfile = this.GetLastDataFile();
                        hlfile.BeginTransaction();
                        DateTime start = DateTime.Now;
                        while ((fields = csv.ReadLine()) != null)
                        {

                            if ((count % 10000) == 0)
                                //PIPS.Logger.WriteLine("HotList.Import({0}, {1})", count, (DateTime.Now - start).TotalMilliseconds);

                            if ((msgCallback != null) && ((count % 1000) == 0))
                            {
                                msgCallback(string.Format("Importing '{0}' at line {1}...", Path.GetFileNameWithoutExtension(pipscsv), count));
                            }

                            count++;

                            Int64 bossId;

                            if (fields.Length<9 || !Int64.TryParse(fields[8], out bossId))
                                bossId = -1;
                            this.Save(fields[0], fields[1], fields[2], fields[3], fields[4], fields[5], fields[6], fields[7], bossId);
                        }
                        hlfile.CommitTransaction();
                    }
                    finally
                    {
                        csv.Close();
                    }
                }
                finally
                {
                    available = true;
                    GC.WaitForPendingFinalizers();
                    GC.Collect(0, GCCollectionMode.Forced);
                    GC.Collect(1, GCCollectionMode.Forced);
                    GC.Collect(2, GCCollectionMode.Forced);
                }

            }
		}

		public void ImportDeltas(string pipscsv, StringHandler msgCallback) {
			if((pipscsv != null) && (pipscsv != string.Empty)) {
				try {
					available = false;
					string inserts = Path.ChangeExtension(pipscsv, "inserts");
					string deletes = Path.ChangeExtension(pipscsv, "deletes");
					if (File.Exists(inserts) || File.Exists(deletes)) {
						ImportDeltaDeletes(pipscsv, deletes, msgCallback);
						ImportDeltaInserts(pipscsv, inserts, msgCallback);
					}
					foreach (HotListDataFile df in this.dataFiles) {
						df.HotList.Reindex();
					}
				} finally {
					available = true;
				}
            }
            GC.WaitForPendingFinalizers();
            GC.Collect(0, GCCollectionMode.Forced);
            GC.Collect(1, GCCollectionMode.Forced);
            GC.Collect(2, GCCollectionMode.Forced);
        }

        public void ImportDeltasBof2(string pipscsv, string filename, StringHandler msgCallback)
        {
            if (string.IsNullOrEmpty(pipscsv)) return;
            try
            {

                if (pipscsv.EndsWith("_ins.pips"))
                {
                    ImportDeltaInserts(filename, pipscsv, msgCallback);
                }

                else if (pipscsv.EndsWith("_del.pips"))
                {
                    ImportDeltaDeletes(filename, pipscsv, msgCallback);
                }

                foreach (HotListDataFile df in this.dataFiles)
                {
                    df.HotList.Reindex();
                }
            }
            catch (Exception ex)
            {
                //PIPS.Logger.Exception(ex);
            }
        }

        private void ImportDeltaInserts(string pipscsv, string filename, StringHandler msgCallback)
        {
			CSV csv = new CSV(PIPS.Utilities.FileEncrypter.IsEncrypted(filename) ? PIPS.Utilities.FileEncrypter.GetStreamReader(filename) : new StreamReader(filename));
			try {
				//PIPS.Logger.WriteLine("HotList.DeltaInserts({0})", filename);
				string[] fields;
				int count = 1;
				int dataFileIndex = 0;
                Int64 bossId;
				HotListDataFile df = ((HotListDataFile) this.dataFiles[dataFileIndex]);
				df.BeginTransaction();
				while((fields = csv.ReadLine()) != null) {
					if((++count % 1000) == 0) {
						if (null != msgCallback) {
							msgCallback(string.Format("Inserting '{0}' at line {1}...", Path.GetFileNameWithoutExtension(filename), count));
						}
						df.CommitTransaction();
						dataFileIndex = (dataFileIndex + 1) % this.dataFiles.Count;
						df.BeginTransaction();
					}
					HotListDataEvent de = df.HotList.CreateDataEvent() as HotListDataEvent;
					de.ID = -1;
					de.VRM = fields[0];
					de.Field1 = fields[1];
					de.Field2 = fields[2];
					de.Field3 = fields[3];
					de.Field4 = fields[4];
					de.Field5 = fields[5];
					de.PNCID = fields[6];
					de.Information = fields[7];

                    //  Only BOSS provides a valid BOSS id
                    if (fields.Length < 9 || !Int64.TryParse(fields[8], out bossId))
                        bossId = -1;
				    de.BossID = bossId;
                    de.Save();
				}
				df.CommitTransaction();
			} finally {
				csv.Close();
			}
            GC.WaitForPendingFinalizers();
            GC.Collect(0, GCCollectionMode.Forced);
            GC.Collect(1, GCCollectionMode.Forced);
            GC.Collect(2, GCCollectionMode.Forced);
		}

		private void ImportDeltaDeletes(string pipscsv, string filename, StringHandler msgCallback) {
			CSV csv = new CSV(PIPS.Utilities.FileEncrypter.IsEncrypted(filename) ? PIPS.Utilities.FileEncrypter.GetStreamReader(filename) : new StreamReader(filename));
			try {
				//PIPS.Logger.WriteLine("HotList.DeltaDeletes({0})", filename);
				string[] fields;
				int count = 0;
				while((fields = csv.ReadLine()) != null) {
					if((msgCallback != null) && ((count % 1000) == 0))
						msgCallback(string.Format("Deleting '{0}' at line {1}...", Path.GetFileNameWithoutExtension(filename), count));

					foreach (HotListDataFile df in this.dataFiles) {
						df.BeginTransaction();
					    long[] IDs = df.HotList.SelectIDsByVRM(fields[0]);

						foreach (long ID in IDs) {
							HotListDataEvent de = df.HotList.SelectByID(ID) as HotListDataEvent;
							if (null != de 
								&& string.Equals(de.VRM,fields[0],StringComparison.OrdinalIgnoreCase)) 
                            {
								de.Delete();
							}
						}
						df.CommitTransaction();
					}
					count++;
				}
			} finally {
				csv.Close();
			}
		}

		public HotListDataFile AddNewDataFile() {
			if(!Directory.Exists(this.directory))
				Directory.CreateDirectory(this.directory);
			HotListDataFile hl = new HotListDataFile(this.directory, this.dataFiles.Count);
			this.dataFiles.Add(hl);
			hl.Initialize();
			return hl;
		}

		public HotListDataFile GetLastDataFile() {
			if(this.dataFiles.Count == 0)
				return this.AddNewDataFile();
			return this.dataFiles[this.dataFiles.Count - 1] as HotListDataFile;
		}

		private void SaveInternal(HotListDataFile hlfile, string vrm, string field1, string field2, string field3, string field4, string field5, string pncid, string information, long bossID) {
			if((vrm != null) && (vrm != string.Empty)) {
				this.insertEvent.ID = -1;
				this.insertEvent.VRM = vrm;
				this.insertEvent.Field1 = field1;
				this.insertEvent.Field2 = field2;
				this.insertEvent.Field3 = field3;
				this.insertEvent.Field4 = field4;
				this.insertEvent.Field5 = field5;
				this.insertEvent.PNCID = pncid;
				this.insertEvent.Information = information;
			    this.insertEvent.BossID = bossID;
				hlfile.Insert(this.insertEvent);
			}
		}
		public void Save(string vrm, string field1, string field2, string field3, string field4, string field5, string pncid, string information, long bossID) {
			this.SaveInternal(this.GetLastDataFile(), vrm.Trim().ToUpper(System.Globalization.CultureInfo.InvariantCulture),
				field1, field2, field3, field4, field5, pncid, information, bossID);
		}

		
		public void Clear() {
			this.Close();
			if(System.IO.Directory.Exists(this.directory))
				System.IO.Directory.Delete(this.directory, true);
			this.available = true;
		}

		#endregion

		#region Find Vrm / Pncid

        /// <summary>
        /// Determines whether the "pncid"/target is used by a hotplate in this hotlist.
        /// </summary>
        /// <param name="pncid">The pncid to be checked.</param>
        /// <returns>True, if the pncid is used by a hotplate.</returns>
        public bool IsPncidUsed(string pncid)
        {
            foreach (HotListDataFile file in dataFiles)
            {
                var hotplateIds = file.HotList.SelectIDs();
                if (hotplateIds == null) continue;

                if (hotplateIds.Select(hotplateId => file.HotList.SelectByID(hotplateId))
                       .OfType<HotListDataEvent>()
                       .Any(hotplateRecord => string.Compare(pncid, hotplateRecord.PNCID, true) == 0))
                {
                    return true;
                }
            }
            return false;
        }

	    public IAsyncResult BeginFindPncid(string pncid) {
			FindPncidData find = new FindPncidData(this, pncid);
			this.eq.AddEvent(find);
			return find;
		}

		public DataEventCollection EndFindPncid(IAsyncResult data) {
			if(data is FindCallbackData) {
				FindCallbackData find = data as FindCallbackData;
				find.AsyncWaitHandle.WaitOne();
				return find.AsyncState as DataEventCollection;
			}
			return new DataEventCollection();
		}

		public IAsyncResult BeginFindVrm(string vrm) {
			FindVrmData find = new FindVrmData(this, vrm);
			this.eq.AddEvent(find);
			return find;
		}

		public DataEventCollection EndFindVrm(IAsyncResult data) {
			if(data is FindCallbackData) {
				FindCallbackData find = data as FindCallbackData;
				find.AsyncWaitHandle.WaitOne();
				return find.AsyncState as DataEventCollection;
			}
			return new DataEventCollection();
		}

        public IAsyncResult BeginFindField1(string field1) {
            FindField1Data find = new FindField1Data(this, field1);
            this.eq.AddEvent(find);
            return find;
        }

        public DataEventCollection EndFindField1(IAsyncResult data) {
            if (data is FindCallbackData) {
                FindCallbackData find = data as FindCallbackData;
                find.AsyncWaitHandle.WaitOne();
                return find.AsyncState as DataEventCollection;
            }
            return new DataEventCollection();
        }

		#region FindPncidData

		private class FindPncidData : FindCallbackData {
			private string pncid;

			public FindPncidData(HotList hotlist, string pncid) : base(hotlist) {
				this.pncid = pncid;
			}

			protected override void FindInternal() {
				foreach(HotListDataFile file in this.HotList.dataFiles) {
					long[] ids = file.PNCID.SelectIDsByIndexColumn(pncid);
					if(ids != null) {
						foreach(long id in ids) {
							this.Results.Add(file.PNCID.SelectByID(id));
						}
					}
				}
			}
		}

		#endregion

		#region FindVrmData

		private class FindVrmData : FindCallbackData {
			private string vrm;

			public FindVrmData(HotList hotlist, string vrm) : base(hotlist) {
				this.vrm = vrm;
			}

			protected override void FindInternal() {
				foreach(HotListDataFile file in this.HotList.dataFiles) {
					long[] ids = file.HotList.SelectIDsByIndexColumn(vrm);
					if(ids != null) {
						foreach(long id in ids) {
							this.Results.Add(file.HotList.SelectByID(id));
						}
					}
				}
			}
		}

        #endregion

        #region

        private class FindField1Data : FindCallbackData {
            private string field1;

            public FindField1Data(HotList hotlist, string field1) : base(hotlist) {
                this.field1 = field1;
            }

            protected override void FindInternal() {
                foreach (HotListDataFile file in this.HotList.dataFiles) {
                    long[] ids = file.HotList.SelectIDsByIndexColumn(field1);
                    if (ids != null) {
                        foreach (long id in ids) {
                            this.Results.Add(file.HotList.SelectByID(id));
                        }
                    }
                }
            }
        }

        #endregion


        #region FindCallbackData

        private abstract class FindCallbackData : IAsyncResult {
			private ManualResetEvent trigger;
			private string vrm;
			private DataEventCollection results;
			private HotList hotlist;
			
			public void Find() {
				try {
					if(this.HotList.available) {
						this.FindInternal();
					}
				} finally {
					this.trigger.Set();
				}
			}

			protected abstract void FindInternal();

			public FindCallbackData(HotList hotlist) {
				this.trigger = new ManualResetEvent(false);
				this.results = new DataEventCollection();
				this.hotlist = hotlist;
			}

			protected HotList HotList {
				get {
					return this.hotlist;
				}
			}

			protected DataEventCollection Results {
				get {
					return this.results;
				}
			}

			#region IAsyncResult Members

			public object AsyncState {
				get {
					return this.Results;
				}
			}

			public bool CompletedSynchronously {
				get {
					return false;
				}
			}

			public System.Threading.WaitHandle AsyncWaitHandle {
				get {
					return this.trigger;
				}
			}

			public bool IsCompleted {
				get {
					return this.trigger.WaitOne(1, false);
				}
			}

			#endregion

		}

		#endregion

		private void FindCallback(object eventdata) {
			FindCallbackData find = eventdata as FindCallbackData;
			find.Find();
		}

		#endregion

		#region IDisposable Members

		public void Dispose() {
			if(this.eq != null) {
				try {
					this.eq.Dispose();
				} catch {}
				this.eq = null;
			}
			if(this.dataFiles != null) {
				try {
					this.Close();
				} catch {}
				this.dataFiles = null;
			}
		}

		#endregion

		private void Close() {
			this.available = false;
			foreach(DataFileBase file in this.dataFiles) {
				file.Dispose();
			}
			this.dataFiles.Clear();
		}
    }
}
