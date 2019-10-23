using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.IO;

namespace repuve_tracker
{
    public partial class FormOptions : Form
    {
        ConfigReader configuration;
        public FormOptions()
        {
            InitializeComponent();
            configuration = new ConfigReader();
            int testread = readConFigFile();
            configuration.READER4000.ATTENUATION = "100";
            int reswrite = WriteConfigFile(configuration);
        }

        private void btOK_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private int readConFigFile()
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(ConfigReader));
                using (FileStream fileStream = new FileStream("ConfigReader.xml", FileMode.Open)) {
                    configuration = (ConfigReader)serializer.Deserialize(fileStream);
                }

                return 0;
              
            }
            catch (Exception ex)
            {
                //log.Error(ex);
                return 4;
            }
        }

        private int WriteConfigFile(ConfigReader config) {

            
            var subReq = new ConfigReader();
            //var xml = "";

            try {
                XmlSerializer writer = new XmlSerializer(typeof(ConfigReader));
                System.IO.FileStream file = System.IO.File.Create("ConfigReader.xml");
                writer.Serialize(file,config);
                file.Close();
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }
            

            return 0;


        }

        private void tbAttenuation6204_Scroll(object sender, EventArgs e)
        {

        }

        private void tbAttenuation4000_Scroll(object sender, EventArgs e)
        {

        }
    }
}
