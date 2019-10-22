using System.Drawing;
using PIPS.PAGIS.Db.SQLite;

namespace PIPS.PAGIS.Db.DataFiles.DataTables
{
    public class HitsDataEvent : DataEvent
    {
        public HitsDataEvent(HitsDataTable table) : base(table)
        {
            //best practice don't return null
            this.Information = string.Empty;
        }

        public long ReadID
        {
            get { return (long) this[0]; }
            set { this[0] = value; }
        }

        public int Priority
        {
            get { return (int) this[1]; }
            set { this[1] = value; }
        }

        public string HotList
        {
            get { return (string) this[2]; }
            set { this[2] = value; }
        }

        public Color DisplayColor
        {
            get { return (Color) this[3]; }
            set { this[3] = value; }
        }

        public bool IsCovert
        {
            get { return (bool) this[4]; }
            set { this[4] = value; }
        }

        public string Alarm
        {
            get { return (string) this[5]; }
            set { this[5] = value; }
        }

        public string VRM
        {
            get { return (string) this[6]; }
            set { this[6] = value; }
        }

        public string Field1
        {
            get { return (string) this[7]; }
            set { this[7] = value; }
        }

        public string Field2
        {
            get { return (string) this[8]; }
            set { this[8] = value; }
        }

        public string Field3
        {
            get { return (string) this[9]; }
            set { this[9] = value; }
        }

        public string Field4
        {
            get { return (string) this[10]; }
            set { this[10] = value; }
        }

        public string Field5
        {
            get { return (string) this[11]; }
            set { this[11] = value; }
        }

        public string PNCID
        {
            get { return (string) this[12]; }
            set { this[12] = value; }
        }

        public string Information
        {
            get { return (string) this[13]; }
            set { this[13] = value; }
        }

        public bool Alerting
        {
            get { return (bool) this[14]; }
            set { this[14] = value; }
        }

    }
}