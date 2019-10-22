using System;
using System.Text;
using System.Xml;

namespace PIPS.XmlPackets
{
    public class HeartBeat
    {
        public const string RootDocumentName = "pips-datastream-heartbeat";

        public DateTime TimeStamp { get; set; }

        //Constructor
        public HeartBeat()
        {
            TimeStamp = DateTime.MinValue;
        }
        
        //Constructor
        public HeartBeat(string xml)
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
        /// Gets heartbeat xml packet for Now
        /// </summary>
        public string Now()
        {
            TimeStamp = DateTime.Now;
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
                    var elements = root.GetElementsByTagName("timestamp");
                    if (elements.Count > 0)
                    {
                        TimeStamp = DateTime.Parse(elements[0].InnerText);
                        success = true;
                    }
                }
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
            var sb = new StringBuilder();
            sb.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            sb.Append(string.Format("<{0}>", RootDocumentName));
            sb.Append(string.Format("  <timestamp>{0}</timestamp>", TimeStamp.ToString("s")));
            sb.Append(string.Format("</{0}>", RootDocumentName));

            return sb.ToString();
        }
    }
}
