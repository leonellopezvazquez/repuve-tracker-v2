using System;
using System.Collections.Generic;
using System.Drawing;
using PIPS.PAGIS.Db.SQLite;

namespace PIPS.PAGIS.Db.DataFiles.DataTables
{

    public class ReadsDataEvent : DataEvent, IReadsDataEvent
    {
        public ReadsDataEvent(IReadsDataTable table,SystemRepository systemRepository) : base(table as DataTableBase)
        {
            SystemRepository = systemRepository;
        }
        #region Field indexes
        private const int FieldIxVRM = 0;
        private const int FieldIxBossID = 13;
        private const int FieldIxLogin = 1;
        private const int FieldIxTimestamp = 2;
        private const int FieldIxLocation = 3;
        private const int FieldIxLatitude = 4;
        private const int FieldIxLongitude = 5;
        private const int FieldIxCamera = 6;
        private const int FieldIxPatchLocation = 7;
        private const int FieldIxOverviewLocation = 8;
        private const int FieldIxConfidence = 9;
        private const int FieldIxIsMisread = 10;
        private const int FieldIxIsManual = 11;
        private const int FieldIxIsSynced = 12;
        private const int FieldIxCameraID = 14;
        private const int FieldIxSyntax = 15;
        #endregion

        public string VRM {
            get {
                return (string)this[FieldIxVRM];
            }
            set {
                this[FieldIxVRM] = value;
            }
        }
        public int BossID
        {
            get { return (int)this[FieldIxBossID]; }
            set { this[FieldIxBossID] = value; }
        }
        public string Login {
            get {
                return (string)this[FieldIxLogin];
            }
            set {
                this[FieldIxLogin] = value;
            }
        }
        public DateTime Timestamp {
            get {
                return (DateTime)this[FieldIxTimestamp];
            }
            set {
                this[FieldIxTimestamp] = value;
            }
        }
        public string Location {
            get {
                return (string)this[FieldIxLocation];
            }
            set {
                this[FieldIxLocation] = value;
            }
        }
        public double Latitude {
            get {
                return (double)this[FieldIxLatitude];
            }
            set {
                this[FieldIxLatitude] = value;
            }
        }

        public double Longitude {
            get {
                return (double)this[FieldIxLongitude];
            }
            set {
                this[FieldIxLongitude] = value;
            }
        }
        public string Camera {
            get {
                return (string)this[FieldIxCamera];
            }
            set {
                this[FieldIxCamera] = value;
            }
        }
        public string Syntax
        {
            get { return (string)this[FieldIxSyntax]; }
            set { this[FieldIxSyntax] = value; }
        }
		
        public int Confidence {
            get {
                return (int)this[FieldIxConfidence];
            }
            set {
                this[FieldIxConfidence] = value;
            }
        }
        public bool IsMisread {
            get {
                return (bool)this[FieldIxIsMisread];
            }
            set {
                this[FieldIxIsMisread] = value;
            }
        }
        public bool IsManual {
            get {
                return (bool)this[FieldIxIsManual];
            }
            set {
                this[FieldIxIsManual] = value;
            }
        }
        public bool IsSynced {
            get {
                return (bool)this[FieldIxIsSynced];
            }
            set {
                this[FieldIxIsSynced] = value;
            }
        }

        public int CameraID{
            get {
                return (int)this[FieldIxCameraID];
            }
            set {
                this[FieldIxCameraID] = value;
            }
        }
        
        /// <summary>
        /// This member added to enable resync of data that's already been synced.
        /// If this is set, LinkedDataTables will not be deleted when the read is
        /// updated.
        /// </summary>
        private bool resync = false;
        public bool Resync {
            get {
                return resync;
            }
            set {
                resync = value;
            }
        }
        private bool permit = false;

        public bool Permit {
            get {
                return permit;
            } 
            set {
                permit = value;
            }
        }


        // linked data properties
        public List<HitsDataEvent> Hits
        {
            get
            {
                List<HitsDataEvent> _hits = new List<HitsDataEvent>();
                foreach( HitsDataEvent hit in this.LinkedDataEvents(0))
                {
                    _hits.Add(hit);
                }
                return _hits;
            }
        }

        public List<HitsDataEvent> HitsFiltered
        {
            get
            {
                List<HitsDataEvent> _hits = new List<HitsDataEvent>();
                foreach (HitsDataEvent hit in this.LinkedDataEvents(0))
                {
                    if (hit.IsCovert == false) _hits.Add(hit);
                }
                return _hits;
            }
        }
        /// <summary>
        ///  This method should be called post synchronization
        /// </summary>
        public override void Save()
        {

            base.Save();
        }
        /// <summary>
        /// This method should be called when we need to queue for sync-ing
        /// </summary>
        public void SaveAndQueueForSync()
        {

            this.IsSynced = false;
            this.Resync = true;
            base.Save();
        }

        public Image CreateOverview()
        {
            throw new NotImplementedException();
        }

        public Image CreatePatch()
        {
            throw new NotImplementedException();
        }

        public SystemRepository SystemRepository { get; set; }
        public byte[] Patch { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public byte[] Overview { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string PatchLocation { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string OverviewLocation { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}