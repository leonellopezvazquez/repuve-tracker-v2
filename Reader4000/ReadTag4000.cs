using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reader4000Conector
{
    public class ReadTag4000
    {
        public string reader { get; set; }
        public string anntena { get; set; }
        public string dateTime { get; set; }
        public string tagEPC { get; set; }
        public string tagUSER { get; set; }
        public string tagFolio { get; set; }
        public string tagVIN { get; set; }

        public override string ToString()
        {
            return reader + "|" + anntena + "|" + tagEPC + "|" + tagUSER + "|" + tagFolio + "|" + tagVIN + "|" + dateTime;
        }
    }
}
