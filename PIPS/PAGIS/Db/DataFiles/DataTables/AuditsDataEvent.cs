using System;
using PIPS.PAGIS.Db.SQLite;

namespace PIPS.PAGIS.Db.DataFiles.DataTables
{
    public class AuditsDataEvent : DataEvent {
        public AuditsDataEvent(AuditsDataTable table) : base(table) {}
	
        public DateTime Timestamp {
            get {
                return (DateTime)this[0];
            }
            set {
                this[0] = value;
            }
        }

        public string Location {
            get {
                return (string)this[1];
            }
            set {
                this[1] = value;
            }
        }

        public double Latitude {
            get {
                return (double)this[2];
            }
            set {
                this[2] = value;
            }
        }

        public double Longitude {
            get {
                return (double)this[3];
            }
            set {
                this[3] = value;
            }
        }

        public string Information {
            get {
                return (string)this[4];
            }
            set {
                this[4] = value;
            }
        }

        public System.Drawing.Image CreatePicture() {
            return DataEvent.CreateImage(this.Picture);
        }

        public byte[] Picture {
            get {
                return (byte[])this[5];
            }
            set {
                this[5] = value;
            }
        }

        public bool IsSynced {
            get {
                return (bool)this[6];
            }
            set {
                this[6] = value;
            }
        }

        public string Login {
            get {
                return (string)this[7];
            }
            set {
                this[7] = value;
            }
        }
    }
}