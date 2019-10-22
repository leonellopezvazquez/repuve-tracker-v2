using System;
using System.IO;

using PIPS.PAGIS.Db.SQLite;

namespace PIPS.PAGIS.Db.HotLists
{
    /// <summary>
    /// Summary description for HotListDataFile.
    /// </summary>
    public class HotListDataFile : DataFileBase
    {
        private int index;
        private string dir;

        public HotListDataFile(string file)
        {
            var dir = Path.GetDirectoryName(file);
            var index = int.Parse(Path.GetFileNameWithoutExtension(file).Remove(0, 8));
            SetObjectFields(dir, index);
        }

        public HotListDataFile(string dir, int index)
        {
            SetObjectFields(dir, index);
        }

        private void SetObjectFields(string dir, int index)
        {
            this.index = index;
            this.dir = dir;
            this.Tables.Add(new PncidDataTable());
            this.Tables.Add(new HotListDataTable());
        }


        public IAsyncResult BeginInitialize()
        {
            return this.BeginInitialize(this.dir);
        }

        public void Initialize()
        {
            this.Initialize(this.dir);
        }

        public override string FileName
        {
            get
            {
                return "HotList_" + index.ToString();
            }
        }

        public static string SearchPattern
        {
            get
            {
                return "HotList_*.*";
            }
        }

        public override int SQLiteVersion
        {
            get
            {
                return 3;
            }
        }

        public PncidDataTable PNCID
        {
            get
            {
                return this.Tables[0] as PncidDataTable;
            }
        }

        public HotListDataTable HotList
        {
            get
            {
                return this.Tables[1] as HotListDataTable;
            }
        }

        public string Dir
        {
            get
            {
                return this.dir;
            }
        }

        public void Insert(HotListDataEvent ev)
        {
            if ((ev.VRM != null) && (ev.VRM != string.Empty))
                this.HotList.Save(ev);
            if ((ev.PNCID != null) && (ev.PNCID != string.Empty))
                this.PNCID.Save(ev);
        }
    }
}
