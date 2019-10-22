using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIPS.PAGIS.Db.SQLite.DataModels
{
    public class Hotlists
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public int Priority { get; set; }
        public DateTime Timestamp { get; set; }
        public long Color { get; set; }
        public bool Covert { get; set; }
        public string Alarm { get; set; }
        public bool Active { get; set; }
        public string File { get; set; }
        public bool WhiteList { get; set; }
        public long BossId { get; set; }
        public byte[] Sound { get; set; }
        public bool Alerting { get; set; }
    }
}
