using System;

namespace Sirit6204Connector
{
    public class Reader
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
