using System;
using System.Configuration;
using System.IO;
using PIPS.PAGIS.Db.SQLite;

namespace PIPS.PAGIS.Db.DataFiles.DataTables
{
    /// <summary>
    /// Summary description for ReadsDataTable.
    /// </summary>
    public class ReadsDataTable : DataTableBase, IReadsDataTable
    {

        public override DataEvent SelectByID(long id)
        {
            ReadsDataEvent readsDataEvent = base.SelectByID(id) as ReadsDataEvent;
            readsDataEvent.SystemRepository = this.DataFile.DataRepository as SystemRepository;
            return readsDataEvent;
        }
        public ReadsDataTable()
        {

            this.Columns.Add(new StringDataColumn("vrm"));
            this.Columns.Add(new StringDataColumn("login"));
            this.Columns.Add(new DateTimeDataColumn("timestamp"));
            this.Columns.Add(new StringDataColumn("location"));
            this.Columns.Add(new DoubleDataColumn("latitude"));
            this.Columns.Add(new DoubleDataColumn("longitude"));
            this.Columns.Add(new StringDataColumn("camera"));

            //this.Columns.Add(new ImageRefDataColumn(new ImageRefHandler(this.imageStorage.Open), "patch"));
            //this.Columns.Add(new ImageRefDataColumn(new ImageRefHandler(this.imageStorage.Open), "overview"));

            this.Columns.Add(new IntDataColumn("confidence"));
            this.Columns.Add(new BooleanDataColumn("misread"));
            this.Columns.Add(new BooleanDataColumn("manual"));
            this.Columns.Add(new BooleanDataColumn("synced"));
            this.Columns.Add(new IntDataColumn("bossid"));
            this.Columns.Add(new IntDataColumn("cameraid"));
            this.Columns.Add(new StringDataColumn("syntax"));


            this.LinkedTables.Add(new HitsDataTable());
            //this.LinkedTables.Add(new ReadDispositionsDataTable());
        }

        public HitsDataTable Hits
        {
            get
            {
                return this.LinkedTables[0] as HitsDataTable;
            }
        }

        public int FilteredHitsCount
        {
            get { return this.SelectIDsWithFilteredHits().Length; }
        }
        public int FilteredReadsCount
        {
            get { return this.SelectIDsWithFilteredReads().Length; }
        }
        public int FilteredMisReadsCount
        {
            get { return this.SelectIDsWithFilteredMisReads().Length; }
        }

        public override string Name
        {
            get
            {
                return "t_reads";
            }
        }

        public override int IndexColumn
        {
            get
            {
                return 0;
            }
        }

        public void Save(XmlPackets.ReadHit readPacket, SystemRepository systemRepository)
        {
            ReadsDataEvent read = this.CreateReadsDataEvent(systemRepository);
            read.VRM = readPacket.VRM;
            read.Login = "RFID";
            read.Timestamp = readPacket.Timestamp;

            //
            // Set lat/lon from the SDK if available, otherwise it will use local GPS
            //
            //PIPS.Logger.WriteLine("xml packet read lat/lon {0},{1}", readPacket.Latitude, readPacket.Longitude);
            read.Location = "0";
            read.Latitude = 0.0;
            read.Longitude = 0.0;
            read.Camera = readPacket.CameraName;
            read.CameraID = readPacket.CameraID;
            if (readPacket.Images[0].ImageName == "Patch")
            {
                read.Patch = readPacket.Images[0].AnprImage;
            }
            else // it is overview pic
            {
                read.Overview = readPacket.Images[0].AnprImage;
            }

            if (readPacket.Images[1].ImageName == "Patch")
            {
                read.Patch = readPacket.Images[1].AnprImage;
            }
            else // it is overview pic
            {
                read.Overview = readPacket.Images[1].AnprImage;
            }

            read.Confidence = (int)readPacket.Confidence;
            read.IsManual = false;
            read.IsMisread = false;
            read.IsSynced = false;
            read.BossID = -1;
            this.Save(read);
        }


    //    protected void Save(PIPS.ANPR.AnprResult result)
    //    {
    //        if (this.DataFile.DataRepository is SystemRepository)
    //        {
    //            DateTime start = DateTime.Now;
    //            string camera = "RFID";

    //            byte[] ov = result.NumOverviews > 0 ? result.Overview(0) : result.Result;
    //            ReadsDataEvent read = this.CreateReadsDataEvent(this.DataFile.DataRepository as SystemRepository);
    //            read.VRM = result.PlateText;
    //            read.Login = "RFID";
    //            read.Timestamp = result.Timestamp;

    //            //
    //            // Set lat/lon from the SDK if available, otherwise it will use local GPS
    //            //
    //            read.Location = "Location";
    //            read.Latitude = 0.0;
    //            read.Longitude = 0.0;
    //            read.Camera = camera;
    //            read.CameraID = result.CameraID;
				//read.Patch = result.Patch;
				//read.Overview = ov;
				//read.Confidence = result.ConfidenceFactor;
    //            //  Syntax only available in LIVE mode
    //            try
    //            {
    //                string dt = (string)(new System.Configuration.AppSettingsReader().GetValue("CaptureReadSyntax", typeof(string)));
    //                read.Syntax = (dt.ToUpper() == "TRUE") ? result.SyntaxName : string.Empty;
    //            }
    //            catch (Exception e)
    //            {
    //                read.Syntax = string.Empty;
    //            }

    //            read.IsManual = false;
				//read.IsMisread = false;
    //            read.IsSynced = false;
    //            read.BossID = -1;

    //            this.Save(read);
    //            if (result.LaneID < 1 || result.LaneID > 4)
    //            {
    //                PIPS.Logger.WriteLine("*** LANEID {0}", result.LaneID);
    //            }

    //            cameraStatistics[result.LaneID - 1]++;
    //            long rc = System.Threading.Interlocked.Increment(ref readCount);
    //            if ((rc % 100) == 0)
    //            {
    //                SaveCameraStatistics();
    //            }
    //            PIPS.Logger.WriteLine("ReadsDataTable.Save({0})", (DateTime.Now - start).TotalMilliseconds);
    //        }
    //        //PIPS.Logger.WriteLine("AnprResult.Saved()");
    //    }

        private static int[] cameraStatistics = new int[4];
        private long readCount = 0;

        private void SaveCameraStatistics()
        {
#if DEBUG
            using (StreamWriter sw = new StreamWriter(string.Format(@"{0}\{1}", System.Windows.Forms.Application.StartupPath, "camerastats.log"), true))
            {
                sw.WriteLine("\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\"",
                    DateTime.Now.ToString(),
                    cameraStatistics[0],
                    cameraStatistics[1],
                    cameraStatistics[2],
                    cameraStatistics[3]
                    );
                sw.Flush();
            }
#endif
        }

        public ReadsDataEvent Save(string vrm)
        {
            return Save(vrm, false);
        }

        public ReadsDataEvent Save(string vrm, bool permit)
        {
            if (this.DataFile.DataRepository is SystemRepository)
            {
                ReadsDataEvent read = this.CreateReadsDataEvent(this.DataFile.DataRepository as SystemRepository);
				read.VRM = vrm;
				read.BossID = -1;
				read.Timestamp = DateTime.Now;
				read.Patch = null;
				read.Overview = null;
                read.Login = "RFID";
                read.Location = "0";
                read.Latitude = 0.0;
                read.Longitude = 0.0;
				read.IsSynced = false;
				read.IsManual = true;
				read.IsMisread = false;
                read.Syntax = string.Empty;
                read.Confidence = 100;
				read.Camera = string.Empty;
				read.CameraID = 0;
				read.Permit = permit;
				this.Save(read);
				if(read.LinkedDataEvents(this.Hits).Count == 0) {
					SoundsDataFile sounds = this.DataFile.DataRepository.DataFiles[typeof(SoundsDataFile)] as SoundsDataFile;
					if(sounds != null) {
						sounds.Sounds.PlaySound(SoundEvents.NoMatchInDatabase);
					}
				}

                return read;
            }
            return null;
        }

        public override void Dispose()
        {
            try
            {
            }
            catch { }
            base.Dispose();
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
        }

        protected override void OnDeleted(DataEvent ev)
        {
            ReadsDataEvent read = ev as ReadsDataEvent;
            base.OnDeleted(ev);
        }

        protected override void OnInserting(DataEvent ev)
        {
            //PIPS.Logger.WriteLine("AnprResult.OnInserting()");
            //DateTime start = DateTime.Now;
            ReadsDataEvent read = ev as ReadsDataEvent;
            if (this.DataFile.DataRepository is SystemRepository)
            {
                (this.DataFile.DataRepository as SystemRepository).HotLists.HotLists.ProcessRead(read);
            }
            //PIPS.Logger.WriteLine("ReadsDataTable.ProcessRead({0})", (DateTime.Now - start).TotalMilliseconds);

            //read.OverviewLocation = this.ImageStorage.Save(this.GetLocation(read, "O"), read.Overview);
            //read.PatchLocation = this.ImageStorage.Save(this.GetLocation(read, "P"), read.Patch);

            //PIPS.Logger.WriteLine("Assigned '{0}' = '{1}'", read.VRM, id);
            //PIPS.Logger.WriteLine("ReadsDataTable.SavedImages({0})", (DateTime.Now - start).TotalMilliseconds);
            base.OnInserting(ev);
        }

        private string GetLocation(ReadsDataEvent read, string desc)
        {
            string cam = string.Empty;
            for (int i = 0; i < read.Camera.Length; i++)
            {
                if (char.IsLetterOrDigit(read.Camera, i))
                    cam = cam + read.Camera[i];
            }
            return string.Format("{0}_{1}_{2}_{3}_{4}.img", read.Timestamp.Ticks, cam, read.Confidence, read.VRM, desc);
        }

        private static long lastPing = DateTime.Now.Ticks;

        protected override void OnInserted(DataEvent ev)
        {

            //PIPS.Logger.WriteLine("AnprResult.OnInserted()");
            //PIPS.Logger.WriteLine("Reads.OnInserted()");
            DateTime start = DateTime.Now;
            SoundsDataFile sounds = this.DataFile.DataRepository.DataFiles[typeof(SoundsDataFile)] as SoundsDataFile;
            if (sounds != null)
            {
                if (lastPing < (DateTime.Now.Ticks - TimeSpan.FromMilliseconds(500).Ticks))
                {
                    lastPing = DateTime.Now.Ticks;
                    sounds.Sounds.PlaySound(SoundEvents.CameraDetection);
                }
                ReadsDataEvent read = ev as ReadsDataEvent;
                if (read.LinkedDataEvents(this.Hits).Count > 0)
                {
                    foreach (HitsDataEvent hit in read.LinkedDataEvents(Hits))
                    {
                        if (hit.Alerting) 
                        {
                                if (hit.Alarm.ToUpper() == "LOW")
                                    sounds.Sounds.PlaySound(SoundEvents.AlarmLow);
                                else if (hit.Alarm.ToUpper() == "HIGH")
                                    sounds.Sounds.PlaySound(SoundEvents.AlarmHigh);
                                else if ((hit.Alarm.ToUpper() == "MED") || (hit.Alarm.ToUpper() == "MEDIUM"))
                                    sounds.Sounds.PlaySound(SoundEvents.AlarmMedium);
                                else
                                    sounds.Sounds.PlaySound(hit.Alarm);
                            break;
                        }
                    }
                }
            }
            //PIPS.Logger.WriteLine("Reads.OnInserted({0}).PlayedSound", (DateTime.Now - start).TotalMilliseconds);
            base.OnInserted(ev);
            //PIPS.Logger.WriteLine("Reads.OnInserted().Complete()");
        }


        protected override void OnUpdated(DataEvent ev)
        {
            //ReadsDataEvent read = ev as ReadsDataEvent;
            //if(!read.IsSynced) {
            base.OnUpdated(ev);
            //}
        }

        protected override void OnUpdating(DataEvent ev)
        {
            ReadsDataEvent read = ev as ReadsDataEvent;
            //
            // IsSynced keeps track of initial sync, Resync keeps track of resync.
            // This convoluted logic is because of retrofitting PAGIS to allow
            // already synced reads to be resent via Wi-Fi.
            //
            // Also don't delete our linked tables if this is a scrubbed read because we can't recreate the hit
            // DataEvents are like the coolest thing ever

            if (!read.IsSynced && !read.Resync && read.VRM != "LPN_SCRUB")
            {
                foreach (DataTableBase linkedTable in this.LinkedTables)
                {
                    foreach (DataEvent linkedEvent in ev.LinkedDataEvents(linkedTable))
                    {
                        linkedTable.Delete(linkedEvent);
                    }
                    ev.LinkedDataEvents(linkedTable).Clear();
                }
                if (this.DataFile.DataRepository is SystemRepository)
                    (this.DataFile.DataRepository as SystemRepository).HotLists.HotLists.ProcessRead(read);
            }
            base.OnUpdating(ev);
        }


        public ReadsDataEvent CreateReadsDataEvent(SystemRepository systemRepository)
        {
            return new ReadsDataEvent(this, systemRepository);
        }

        public override DataEvent CreateDataEvent()
        {
            return this.CreateReadsDataEvent(null);
        }

        public long SyncedCount
        {
            get
            {
                return this.GetCount("synced = 'True'");
            }
        }

        public long MisreadsCount
        {
            get
            {
                return this.GetCount("misread = 'True'");
            }
        }

        public long ManaulsCount
        {
            get
            {
                return this.GetCount("manual = 'True'");
            }
        }

        public long[] SelectManualIDs()
        {
            return this.SelectIDsWhere("manual = 'True'");
        }

        public long[] SelectNonSyncedIDs()
        {
            return this.SelectIDsWhere("synced = 'False'", "timestamp", false);
        }

        public long[] SelectSyncedIDs()
        {
            return this.SelectIDsWhere("synced = 'True'");
        }

        public long[] SelectMisreadIDs()
        {
            return this.SelectIDsWhere("misread = 'True' and not vrm =" + ScrubPlate);
        }

        /// <summary>
        /// Gets a list of read id's which are not misreads
        /// and have associated hits
        /// </summary>
        /// <returns></returns>
        public virtual long[] SelectIDsWithFilteredHits()
        {
            return SelectIDsCustom(@"
                                    SELECT t_reads.id from t_reads 
                                    JOIN t_hits on t_hits.read_id = t_reads.id
                                    WHERE t_reads.misread == 'False'
                                    AND t_hits.covert == 'False' and not t_reads.vrm =" + ScrubPlate
                   );
        }

        public virtual long[] SelectIDsWithFilteredReads()
        {
            return SelectIDsCustom(@"
                                    SELECT t_reads.id from t_reads 
                                    WHERE not t_reads.vrm =" + ScrubPlate
                   );
        }
        public virtual long[] SelectIDsWithFilteredMisReads()
        {
            return SelectIDsCustom(@"
                                    SELECT t_reads.id from t_reads 
                                    WHERE t_reads.misread == 'True' and not t_reads.vrm =" + ScrubPlate
                   );
        }
        private const string ScrubPlate = "'LPN_SCRUB'";
    }
}
