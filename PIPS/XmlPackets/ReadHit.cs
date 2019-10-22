using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace PIPS.XmlPackets
{
    public class ReadHit
    {
        public const string RootDocumentName = "pips-datastream-reads";

        public string SourceName { get; set; }
        public string CameraName { get; set; }
        public int CameraID { get; set; }
        public string SoftwareVersion { get; set; }
        public string State { get; set; }
        public string VRM { get; set; }
        public double Confidence { get; set; }
        public DateTime Timestamp { get; set; }
        public string UserName { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public bool IsMisread { get; set; }
        public bool IsManual { get; set; }

        private List<ImageInfo> _imageInfos = new List<ImageInfo>();
        public List<ImageInfo> Images
        {
            get { return _imageInfos; }
            set { _imageInfos = value; }
        }

        private readonly List<HitInfo> _hitInfos = new List<HitInfo>();
        public List<HitInfo> HitInfos
        {
            get { return _hitInfos; }
        }

        public class HitInfo
        {
            public const int FieldCount = 5;
            public string[] Fields = new string[FieldCount];
            public string HotlistName;
        }

        public class ImageInfo
        {
            public string ImageName;
            public string Type;
            public byte[] AnprImage;
        }

        //Constructor
        public ReadHit()
        {
        }

        //Constructor
        public ReadHit(string xml)
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
            Clear();

            var success = false;
            try
            {
                var doc = new XmlDocument();
                doc.LoadXml(xml);

                var root = doc.DocumentElement;
                if ((root != null) && (root.Name.ToLower() == RootDocumentName))
                {
                    var sourceNodes = root.GetElementsByTagName("source");

                    if (sourceNodes.Count > 0)
                    {
                        var sourceNode = sourceNodes[0];
                        foreach (XmlElement sourceChildNode in sourceNode.ChildNodes)
                        {
                            switch (sourceChildNode.Name.ToLower())
                            {
                                case "name":
                                    SourceName = sourceChildNode.InnerText;
                                    break;
                                case "software":
                                    var softwareVersionAttribute = sourceChildNode.GetAttributeNode("version");
                                    if (softwareVersionAttribute != null)
                                    {
                                        SoftwareVersion = softwareVersionAttribute.InnerText;
                                    }
                                    break;
                                case "camera":
                                    var cameraNameAttribute = sourceChildNode.GetAttributeNode("name");
                                    if (cameraNameAttribute != null)
                                    {
                                        CameraName = cameraNameAttribute.InnerText;
                                    }
                                    ParseRead(sourceChildNode);
                                    break;
                            }
                        }
                    }
                }
                success = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                //swallow
            }

            return success;
        }

        /// <summary>
        /// Parse the "read" node
        /// </summary>
        private void ParseRead(XmlElement sourceChildNode)
        {
            var readsNodes = sourceChildNode.GetElementsByTagName("read");

            if (readsNodes.Count <= 0) return;

            foreach (XmlElement readChildNode in readsNodes[0].ChildNodes)
            {
                switch (readChildNode.Name.ToLower())
                {
                    case "state":
                        State = readChildNode.InnerText;
                        break;
                    case "vrm":
                        VRM = readChildNode.InnerText;
                        break;
                    case "confidence":
                        float confidence;
                        float.TryParse(readChildNode.InnerText, out confidence);
                        Confidence = confidence;
                        break;
                    case "timestamp":
                        DateTime timestamp;
                        DateTime.TryParse(readChildNode.InnerText, out timestamp);
                        Timestamp = timestamp;
                        break;
                    case "username":
                        UserName = readChildNode.InnerText;
                        break;
                    case "gps":
                        ParseGps(readChildNode);
                        break;
                    case "misread":
                        if (!string.IsNullOrEmpty(readChildNode.InnerText))
                        {
                            IsMisread = (readChildNode.InnerText != "0");
                        }
                        break;
                    case "manual":
                        if (!string.IsNullOrEmpty(readChildNode.InnerText))
                        {
                            IsManual = (readChildNode.InnerText != "0");
                        }
                        break;
                    case "images":
                        ParseImages(readChildNode);
                        break;
                    case "hits":
                        ParseHits(readChildNode);
                        break;
                }
            }
        }

        /// <summary>
        /// Parse the "gps" node
        /// </summary>
        private void ParseGps(XmlElement readChildNode)
        {
            foreach (XmlElement gpsNode in readChildNode.ChildNodes)
            {
                switch (gpsNode.Name.ToLower())
                {
                    case "latitude":
                        float latitude;
                        float.TryParse(gpsNode.InnerText, out latitude);
                        Latitude = latitude;
                        break;
                    case "longitude":
                        float longitude;
                        float.TryParse(gpsNode.InnerText, out longitude);
                        Longitude = longitude;
                        break;
                }
            }
        }

        /// <summary>
        /// Parse the "hits" node
        /// </summary>
        private void ParseHits(XmlElement readChildNode)
        {
            const string field = "field";

            //Parse Hits
            foreach (XmlElement hitNode in readChildNode.ChildNodes)
            {
                var hitInfo = new HitInfo();

                var hotlistName = hitNode.GetAttributeNode("name");
                if (hotlistName != null)
                {
                    hitInfo.HotlistName = hotlistName.InnerText;
                }


                //Parse fields (field1, field2, etc)
                foreach (XmlElement hitChildNode in hitNode.ChildNodes)
                {
                    var fieldName = hitChildNode.Name.ToLower();
                    if (fieldName.StartsWith(field))
                    {
                        var lastPart = fieldName.Substring(field.Length);
                        int index;
                        int.TryParse(lastPart, out index);
                        index--;
                        if (index >= 0 && index < HitInfo.FieldCount)
                        {
                            hitInfo.Fields[index] = hitChildNode.InnerText;
                        }
                    }
                }
                HitInfos.Add(hitInfo);
            }
        }

        /// <summary>
        /// Parse the "images" node
        /// </summary>
        private void ParseImages(XmlElement readChildNode)
        {
            //Parse Images
            foreach (XmlElement imageNode in readChildNode.ChildNodes)
            {
                ImageInfo image = new ImageInfo();

                var imageName = imageNode.GetAttributeNode("name");
                if (imageName != null)
                {
                    image.ImageName = imageName.InnerText;
                }


                //Parse image node children
                foreach (XmlElement imageChildNode in imageNode.ChildNodes)
                {
                    switch (imageChildNode.Name.ToLower())
                    {
                        case "type":
                            image.Type = imageChildNode.InnerText;
                            break;
                        case "data":
                            image.AnprImage = Convert.FromBase64String(imageChildNode.InnerText);
                            break;
                    }
                }
                Images.Add(image);
            }
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
            sb.Append("<reads>");
            sb.Append("<source>");
            sb.Append(AddNode("name", SourceName));
            sb.Append(AddNodeAndAttribute("software name=\"PAGIS\"", "version", SoftwareVersion));
            sb.Append("</software>");
            sb.Append(AddNodeAndAttribute("camera", "name", CameraName));
            sb.Append("<read>");
            sb.Append(AddNode("state", State));
            sb.Append(AddNode("vrm", VRM));

            sb.Append(AddNode("confidence", Confidence));
            sb.Append(AddNode("timestamp", Timestamp.ToString("s")));
            sb.Append(AddNode("username", UserName));
            sb.Append("<gps>");
            sb.Append(AddNode("latitude", Latitude));
            sb.Append(AddNode("longitude", Longitude));
            sb.Append("</gps>");

            if (Images.Count > 0)
            {
                sb.Append("<images>");
                foreach (var image in Images)
                {
                    sb.Append(AddNodeAndAttribute("image", "name", image.ImageName));
                    sb.Append(AddNode("type", IsMisread));

                    sb.Append(AddNode("data", Convert.ToBase64String(image.AnprImage)));

                    sb.Append("</image>");
                }
                sb.Append("</images>");
            }

            sb.Append(AddNode("misread", IsMisread));
            sb.Append(AddNode("manual", IsManual));

            if (HitInfos.Count > 0)
            {
                sb.Append("<hits>");
                foreach (var hit in HitInfos)
                {
                    sb.Append(AddNodeAndAttribute("hotlist", "name", hit.HotlistName));
                    for (var i = 0; i < HitInfo.FieldCount; i++)
                    {
                        sb.Append(AddNode(string.Format("field{0}", i + 1), hit.Fields[i]));
                    }
                    sb.Append("</hotlist>");
                }
                sb.Append("</hits>");
            }

            sb.Append("</read>");
            sb.Append("</camera>");
            sb.Append("</source>");
            sb.Append("</reads>");
            sb.Append(string.Format("</{0}>", RootDocumentName)); 

            return sb.ToString();
            //return FormatXml(sb.ToString());
        }

        ///// <summary>
        ///// Formats the provided Xml so it is indented and human readable.
        ///// </summary>
        ///// <param name="xml">The input Xml to format.</param>
        //public static string FormatXml(string xml)
        //{
        //    var doc = new XmlDocument();
        //    doc.Load(xml);
        //    //doc.Load(new StringReader(xml));
        //    var sb = new StringBuilder();
        //    using (var writer = new XmlTextWriter(new StringWriter(sb)))
        //    {
        //        writer.Formatting = Formatting.Indented;
        //        doc.Save(writer);
        //    }
        //    return sb.ToString();
        //}

        public void Clear()
        {
            SourceName = string.Empty;
            CameraName = string.Empty;
            SoftwareVersion = string.Empty;
            State = string.Empty;
            VRM = string.Empty;
            Confidence = 0.0f;
            Timestamp = DateTime.MinValue;
            UserName = string.Empty;
            Latitude = 0.0f;
            Longitude = 0.0f;
            IsMisread = false;
            IsManual = false;
            Images.Clear();
            HitInfos.Clear();
        }

        public string AddNode(string nodeName, object nodeValue)
        {
            return string.Format("<{0}>{1}</{0}>", nodeName, nodeValue);
        }
        public string AddNodeAndAttribute(string nodeName, string attributeName, string attributeValue)
        {
            return string.Format("<{0} {1}=\"{2}\">", nodeName, attributeName, attributeValue);
        }
    }
}
