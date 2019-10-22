using System;
using PIPS.PAGIS.Db.SQLite;

namespace PIPS.PAGIS.Db.DataFiles.DataTables
{
    public class TargetsDataEvent : DataEvent {

        public TargetsDataEvent(TargetsDataTable table) : base(table) {}

        public string PNCID {
            get {
                return (string)this[0];
            }
            set {
                this[0] = value.ToUpper();
            }
        }

        public string Name {
            get {
                return (string)this[1];
            }
            set {
                this[1] = value;
            }
        }

        public string Address {
            get {
                return (string)this[2];
            }
            set {
                this[2] = value;
            }
        }

        public DateTime DateOfBirth {
            get {
                return (DateTime)this[3];
            }
            set {
                this[3] = value;
            }
        }

        public string Birthplace {
            get {
                return (string)this[4];
            }
            set {
                this[4] = value;
            }
        }

        public string Ethnicity {
            get {
                return (string)this[5];
            }
            set {
                this[5] = value;
            }
        }

        public string Category {
            get {
                return (string)this[6];
            }
            set {
                this[6] = value;
            }
        }

        public string Warning {
            get {
                return (string)this[7];
            }
            set {
                this[7] = value;
            }
        }

        public string Information {
            get {
                return (string)this[8];
            }
            set {
                this[8] = value;
            }
        }

        public string Login {
            get {
                return (string)this[9];
            }
            set {
                this[9] = value;
            }
        }

        public byte[] Image
        {
            get
            {
                return (byte[])this[10];
            }
            set
            {
                this[10] = value;
            }
        }

        public long BossID
        {
            get { return (long)this[11]; }
            set { this[11] = value; }
        }
        
        public System.Drawing.Image CreatePicture()
        {
            return DataEvent.CreateImage(this.Image);
        }      
    }
}