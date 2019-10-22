using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIPS.PAGIS.Db.SQLite.DataModels
{
    public class HotPlate
    {

        public HotPlate()
        {
        }
        public long BossID { get; set; }

        public string VRM { get; set; }

        public string Field1 { get; set; }

        public string Field2 { get; set; }

        public string Field3 { get; set; }

        public string Field4 { get; set; }

        public string Field5 { get; set; }

        public string PNCID { get; set; }

        public string Information { get; set; }

        public string FilePath { get; set; }
    }
}
