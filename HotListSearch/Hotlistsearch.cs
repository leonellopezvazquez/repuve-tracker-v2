using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PIPS.PAGIS.Db;
using PIPS.PAGIS.Db.DataFiles.DataTables;
using PIPS.PAGIS.Db.SQLite;

namespace HotListSearch
{
    public class Hotlistsearch
    {

        SystemRepository sr;
        public void Initialize()
        {
            sr = new SystemRepository();
            //List<string> result = sr.HotLists.HotLists.ProcessRead_IPS("3AM68513350018449");
            //List<string> result2 = sr.HotLists.HotLists.ProcessRead("335BU4");
        }

        public List<string> SearchVIN(string vin) { 
           return sr.HotLists.HotLists.ProcessRead_IPS(vin);
        }
    }
}
