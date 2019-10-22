using System.Text;
using System.Xml;

namespace PIPS.XmlPackets
{
    public class Subscription
    {
        public const string RootDocumentName = "pips-datastream-subscription"; 
        
        public bool Reads { get; set; }
        public bool Hits { get; set; }
        public bool HitTrigger { get; set; }
        public Camera CurrentImage = new Camera();

        //Constructor
        public Subscription()
        {
        }

        //Constructor
        public Subscription(string xml)
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
                    Reads = false;
                    Hits = false;
                    HitTrigger = false;
                    CurrentImage.IsActive = false;

                    var elements = root.GetElementsByTagName("datatype");
                    foreach (XmlNode element in elements)
                    {
                        switch (element.InnerText.ToLower())
                        {
                            case "reads":
                                Reads = true;
                                break;
                            case "hits":
                                Hits = true;
                                break;
                            case "hittrigger":
                                HitTrigger = true;
                                break;
                            case "currentimage":
                                CurrentImage.IsActive = true;
                                //Get attributes
                                if (element.Attributes != null)
                                {
                                    foreach (XmlAttribute attribute in element.Attributes)
                                    {
                                        switch (attribute.Name)
                                        {
                                            case "camera":
                                                int index;
                                                int.TryParse(attribute.InnerText, out index);
                                                CurrentImage.Index = index;
                                                break;
                                            case "milliseconds":
                                                int milliseconds;
                                                int.TryParse(attribute.InnerText, out milliseconds);
                                                CurrentImage.Milliseconds = milliseconds;
                                                break;
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
            if (Reads)
            {
                sb.Append("  <datatype>Reads</datatype>");
            }
            if (Hits)
            {
                sb.Append("  <datatype>Hits</datatype>");
            }
            if (HitTrigger)
            {
                sb.Append("  <datatype>HitTrigger</datatype>");
            }
            if (CurrentImage.IsActive)
            {
                sb.Append(string.Format("  <datatype camera=\"{0}\" milliseconds=\"{1}\">CurrentImage</datatype>", CurrentImage.Index, CurrentImage.Milliseconds));
            }
            sb.Append(string.Format("</{0}>", RootDocumentName));
            return sb.ToString();
        }

        public class Camera
        {
            public Camera()
            {
                IsActive = false;
                Index = 0; 
                Milliseconds = 1000;
            }
            /// <summary>
            /// Is the camera active
            /// </summary>
            public bool IsActive { get; set; }
            /// <summary>
            /// Index of the active camera
            /// </summary>
            public int Index { get; set; }
            /// <summary>
            /// Number of milliseconds between image captures
            /// </summary>
            public int Milliseconds { get; set; }
        }
    }
}
