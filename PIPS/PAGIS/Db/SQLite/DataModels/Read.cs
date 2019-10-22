using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIPS.PAGIS.Db.SQLite.DataModels
{
    public class Read
    {
        public long Id { get; set; }
        public string Vrm { get; set; }
        public string Login { get; set; }
        public DateTime Timestamp { get; set; }
        public string Location { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Camera { get; set; }
        public string PatchLocation { get; set; }
        public string OverviewLocation { get; set; }
        public byte[] Patch { get; set; }
        public byte[] Overview { get; set; }
        public int Confidence { get; set; }
        public bool Misread { get; set; }
        public bool Synced { get; set; }
        public long BossId { get; set; }
        public int CameraId { get; set; }
    }
}
