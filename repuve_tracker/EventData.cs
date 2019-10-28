using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace repuve_tracker
{
    public class EventData:IDisposable
    {
        public string model { get; set; }
        public string brand { get; set; }
        public string country { get; set; }
        public string folio { get; set; }
        public string VIN { get; set; }
        public string TS { get; set; }
        public string year { get; set; }
        public bool status { get; set; }
        public bool IsHit { get; set; }
        public DateTime dateTime { get; set; }

        public void Dispose()
        {
           // throw new NotImplementedException();
        }

        public override string ToString()
        {
            return model + "|" +brand+ "|" + country + "|" + folio + "|" + VIN +"|"+ TS + "|" + year + "|" + status+"|"+IsHit+"|"+ dateTime;
        }

    }
}
