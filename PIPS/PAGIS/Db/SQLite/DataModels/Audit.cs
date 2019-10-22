using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace PIPS.PAGIS.Db.SQLite.DataModels
{
    public class Audit
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string Location { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Information { get; set; }
        public byte[] Picture { get; set; }
        public bool Synced { get; set; }
        public string Login { get; set; }

        public XmlDocument Serialize()
        {
            XmlDocument doc = new XmlDocument();
            var root = doc.CreateElement("t_audits");
            doc.AppendChild(root);

            var child = doc.CreateElement("timestamp");
            var cdata = doc.CreateCDataSection(Timestamp.Ticks.ToString());
            child.AppendChild(cdata);
            root.AppendChild(child);

            child = doc.CreateElement("location");
            cdata = doc.CreateCDataSection(Location);
            child.AppendChild(cdata);
            root.AppendChild(child);

            child = doc.CreateElement("latitude");
            cdata = doc.CreateCDataSection(Latitude.ToString());
            child.AppendChild(cdata);
            root.AppendChild(child);

            child = doc.CreateElement("longitude");
            cdata = doc.CreateCDataSection(Longitude.ToString());
            child.AppendChild(cdata);
            root.AppendChild(child);

            child = doc.CreateElement("information");
            cdata = doc.CreateCDataSection(Information);
            child.AppendChild(cdata);
            root.AppendChild(child);

            child = doc.CreateElement("picture");
            cdata = doc.CreateCDataSection(Convert.ToBase64String((byte[])Picture, 0, ((byte[])Picture).Length));
            child.AppendChild(cdata);
            root.AppendChild(child);

            child = doc.CreateElement("synced");
            cdata = doc.CreateCDataSection(Synced.ToString());
            child.AppendChild(cdata);
            root.AppendChild(child);

            child = doc.CreateElement("login");
            cdata = doc.CreateCDataSection(Login);
            child.AppendChild(cdata);
            root.AppendChild(child);

            return doc;
        }
    }
}
