using System;
using System.Text;
using System.Xml;

namespace PIPS.XmlPackets
{
    public class HitTrigger
    {
        public const string RootDocumentName = "pips-datastream-hittrigger";

        public DateTime Timestamp { get; set; }
        public string VRM { get; set; }
        public string HotlistName { get; set; }
        public string[] Fields = new string[ReadHit.HitInfo.FieldCount];

        //Constructor
        public HitTrigger()
        {
        }

        //Constructor
        public HitTrigger(string xml)
        {
            if (!LoadXml(xml))
            {
                throw new XmlException("Xml did not load or parse.");
            }
        }

        /// <summary>
        /// Loads XML
        /// </summary>
        public bool LoadXml(string xml)
        {
            return ParseXmlToFields(xml);
        }

        /// <summary>
        /// Gets Xml
        /// </summary>
        public string Xml()
        {
            return BuildXmlFromFields();
        }

        /// <summary>
        /// Parses Xml into fields
        /// </summary>
        private bool ParseXmlToFields(string xml)
        {
            var success = false;
            try
            {
                var doc = new XmlDocument();
                doc.LoadXml(xml);

                var root = doc.DocumentElement;
                if ((root != null) && (root.Name.ToLower() == RootDocumentName))
                {
                    Timestamp = DateTime.MinValue;
                    VRM = "";
                    HotlistName = "";

                    for (var i = 0; i < ReadHit.HitInfo.FieldCount; i++)
                    {
                        Fields[i] = string.Empty;
                    }

                    foreach (XmlElement element in root.ChildNodes)
                    {
                        switch (element.Name.ToLower())
                        {
                            case "timestamp":
                                DateTime datetime;
                                DateTime.TryParse(element.InnerText, out datetime);
                                if (datetime != DateTime.MinValue)
                                {
                                    Timestamp = datetime;
                                }
                                break;
                            case "vrm":
                                VRM = element.InnerText;
                                break;
                            case "hotlist":
                                if (element.HasAttributes)
                                {
                                    foreach (XmlAttribute attribute in element.Attributes)
                                    {
                                        if (attribute.Name.ToLower() == "name")
                                        {
                                            HotlistName = attribute.InnerText;
                                        }
                                    }

                                    for (var i = 0; i < ReadHit.HitInfo.FieldCount; i++)
                                    {
                                        var elements = element.GetElementsByTagName("field" + (i + 1));
                                        if (elements.Count > 0)
                                        {
                                            Fields[i] = elements[0].InnerText;
                                        }
                                    }
                                }
                                break;
                        }
                    }
                }
                success = true;
            }
            catch
            {
                //swallow
            }

            return success;
        }

        /// <summary>
        /// Builds the Xml
        /// </summary>
        /// <returns>Xml</returns>
        private string BuildXmlFromFields()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            sb.Append(string.Format("<{0}>", RootDocumentName)); 
            sb.Append(string.Format("  <timestamp>{0}</timestamp>", Timestamp.ToString("s")));
            sb.Append(string.Format("  <vrm>{0}</vrm>", VRM));
            sb.Append(string.Format("  <hotlist name=\"{0}\">", HotlistName));
            for (var i = 0; i < ReadHit.HitInfo.FieldCount; i++)
            {
                sb.Append(string.Format("    <field{0}>{1}</field{0}>", i + 1, Fields[i]));
            }
            sb.Append("  </hotlist>");
            sb.Append(string.Format("</{0}>", RootDocumentName)); 
            
            return sb.ToString();
        }
    }
}
