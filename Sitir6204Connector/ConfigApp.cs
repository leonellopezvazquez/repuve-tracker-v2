using System.Collections.Generic;
using System.Xml.Serialization;

namespace Sirit6204Connector
{
    [XmlRoot(ElementName = "CONFIG")]
    public class ConfigApp
    {
        [XmlElement(ElementName = "HEARTBEAT")]
        public string HeartBeat { get; set; }
        [XmlElement(ElementName = "EVENTTAG")]
        public string EventTag { get; set; }
        [XmlElement(ElementName = "FOLIO-VIN")]
        public bool FolioVIN { get; set; }
    }
}
