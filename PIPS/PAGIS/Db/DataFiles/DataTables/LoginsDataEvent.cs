using System;
using PIPS.PAGIS.Db.SQLite;

namespace PIPS.PAGIS.Db.DataFiles.DataTables
{
    public class LoginsDataEvent : DataEvent {
        private bool encrypt = false;

        public LoginsDataEvent(LoginsDataTable table) : base(table) {
            this.encrypt = table.Encrypt;
        }

        public string Login {
            get {
                return (string)this[0];
            }
            set {
                this[0] = value;
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

        public string Password {
            get {
                return (string)this[2];
            }
            set {
                this[2] = value;
            }
        }

        public DateTime Expiration {
            get {
                return (DateTime)this[3];
            }
            set {
                this[3] = value;
            }
        }

        public bool IsPncUser {
            get {
                return (bool)this[4];
            }
            set {
                this[4] = value;
            }
        }

        public bool IsSyncUser {
            get {
                return (bool)this[5];
            }
            set {
                this[5] = value;
            }
        }

        public bool IsAdminUser {
            get {
                return (bool)this[6];
            }
            set {
                this[6] = value;
            }
        }

        public long BossID
        {
            get { return (long) this[7]; }
            set { this[7] = value; }
        }
    }
}