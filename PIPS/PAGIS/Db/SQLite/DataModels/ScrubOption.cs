using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIPS.PAGIS.Db.SQLite.DataModels
{
    public class ScrubOption
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public int Minutes { get; set; }
    }
}
