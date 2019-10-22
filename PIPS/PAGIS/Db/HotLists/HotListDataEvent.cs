using PIPS.PAGIS.Db.SQLite;

namespace PIPS.PAGIS.Db.HotLists
{
    public class HotListDataEvent : DataEvent {
        public HotListDataEvent(HotListDataTable table) : base(table) {}

        public string VRM {
            get {
                return (string)this[0];
            }
            set {
                this[0] = value.ToUpper();
            }
        }

        public string Field1 {
            get {
                return (string)this[1];
            }
            set {
                this[1] = value;
            }
        }
        public string Field2 {
            get {
                return (string)this[2];
            }
            set {
                this[2] = value;
            }
        }
        public string Field3 {
            get {
                return (string)this[3];
            }
            set {
                this[3] = value;
            }
        }
        public string Field4 {
            get {
                return (string)this[4];
            }
            set {
                this[4] = value;
            }
        }

        public string Field5 {
            get {
                return (string)this[5];
            }
            set {
                this[5] = value;
            }
        }

        public string PNCID {
            get {
                return (string)this[6];
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                    this[6] = value.ToUpper();
                else this[6] = "";
            }
        }

        public string Information {
            get {
                return (string)this[7];
            }
            set {
                this[7] = value;
            }
        }

        public long BossID
        {
            get { return (long)this[8]; }
            set { this[8] = value; }
        }

        public bool Alerting
        {
            get { return (bool)this[11]; }
            set { this[11] = value; }
        }
    }
}