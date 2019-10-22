using System;
using System.Text;
using System.Xml;

namespace PIPS.XmlPackets
{
    public class CurrentImage
    {
        public const string RootDocumentName = "pips-datastream-currentimage";

        public DateTime Timestamp { get; set; }
        public byte[] AnprImage;

        //Constructor
        public CurrentImage()
        {
            Timestamp = DateTime.MinValue;
        }
        
        //Constructor
        public CurrentImage(string xml)
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
                    var elements = root.GetElementsByTagName("timestamp");
                    if (elements.Count > 0)
                    {
                        Timestamp = DateTime.Parse(elements[0].InnerText);
                    }

                    elements = root.GetElementsByTagName("data");
                    if (elements.Count > 0)
                    {
                        AnprImage = Convert.FromBase64String(elements[0].InnerText);
                    }

                    success = true;
                }
            }
            catch (Exception ex)
            {
                //swallow
                Console.WriteLine(ex);
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
            sb.Append(string.Format("  <timestamp>{0}</timestamp>", Timestamp.ToString("s")));
            sb.Append(string.Format("  <data>{0}</data>", Convert.ToBase64String(AnprImage)));
            sb.Append(string.Format("</{0}>", RootDocumentName));

            return sb.ToString();
        }
    }
}
