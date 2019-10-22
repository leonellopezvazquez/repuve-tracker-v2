using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Reader4000
{
    [XmlRoot(ElementName = "CONFIG")]
    class Config
    {
        [XmlElement(ElementName = "HEARTBEAT")]
        public string HeartBeat { get; set; }
        [XmlElement(ElementName = "EVENTTAG")]
        public string EventTag { get; set; }
        [XmlElement(ElementName = "FOLIO-VIN")]
        public bool FolioVIN { get; set; }
    }
}
