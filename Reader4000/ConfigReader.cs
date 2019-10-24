using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Reader4000Conector
{

    [XmlRoot(ElementName = "READER6204")]
    public class ConfigREADER6204
    {
        [XmlElement(ElementName = "IPADDRESS")]
        public string IPADDRESS { get; set; }
        [XmlElement(ElementName = "ANTENNA1")]
        public string ANTENNA1 { get; set; }
        [XmlElement(ElementName = "ANTENNA2")]
        public string ANTENNA2 { get; set; }
        [XmlElement(ElementName = "ANTENNA3")]
        public string ANTENNA3 { get; set; }
        [XmlElement(ElementName = "ANTENNA4")]
        public string ANTENNA4 { get; set; }
        [XmlElement(ElementName = "ATTENUATION")]
        public string ATTENUATION { get; set; }
    }

    [XmlRoot(ElementName = "READER4000")]
    public class ConfigREADER4000
    {
        [XmlElement(ElementName = "IPADDRESS")]
        public string IPADDRESS { get; set; }
        [XmlElement(ElementName = "ANTENNA1")]
        public string ANTENNA1 { get; set; }
        [XmlElement(ElementName = "ANTENNA2")]
        public string ANTENNA2 { get; set; }
        [XmlElement(ElementName = "ANTENNA3")]
        public string ANTENNA3 { get; set; }
        [XmlElement(ElementName = "ANTENNA4")]
        public string ANTENNA4 { get; set; }
        [XmlElement(ElementName = "ATTENUATION")]
        public string ATTENUATION { get; set; }
    }

    [XmlRoot(ElementName = "CONFIGREADER")]
    public class ConfigReader
    {
        [XmlElement(ElementName = "ACTUAL")]
        public string ACTUAL { get; set; }
        [XmlElement(ElementName = "READER6204")]
        public ConfigREADER6204 READER6204 { get; set; }
        [XmlElement(ElementName = "READER4000")]
        public ConfigREADER4000 READER4000 { get; set; }
    }
}
