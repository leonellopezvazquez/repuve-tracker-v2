using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reader4000Conector
{
    public class NewReader4000
    {
        public string name { get; set; }
        public bool status { get; set; }
        public DateTime dateTime { get; set; }

        public override string ToString()
        {
            return name + "|" + status + "|" + dateTime;
        }
    }
}
