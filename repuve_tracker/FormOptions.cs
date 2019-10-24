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
            
           
            if (readConFigFile() == 0) {
                //paint settings
                
                paintsettings();
            }

            else {
                Console.WriteLine("error de lectura de archivo de configuracion");
            }
        }

        private void btOK_Click(object sender, EventArgs e)
        {

            //this.Hide();
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
       
            try {
                XmlSerializer writer = new XmlSerializer(typeof(ConfigReader));
                System.IO.FileStream file = System.IO.File.Create("ConfigReader.xml");
                writer.Serialize(file,config);
                file.Close();
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
                return 1;
            }
            
            return 0;
        }


        private void paintsettings() {
            if (configuration != null) {

                if (configuration.ACTUAL.Equals("6204"))
                {
                    Sel6204.Select();
                }
                else {
                    SelID4000.Select();
                }

                tbIP6204.Text = configuration.READER6204.IPADDRESS;

                tbIP4000.Text = configuration.READER4000.IPADDRESS;

                if (configuration.READER6204.ANTENNA1.Equals("ON"))
                {
                    cbant16204.Checked = true;
                }
                else
                {
                    cbant16204.Checked = false;
                }


                if (configuration.READER6204.ANTENNA2.Equals("ON"))
                {
                    cbant26204.Checked = true;
                }
                else
                {
                    cbant26204.Checked = false;
                }


                if (configuration.READER6204.ANTENNA1.Equals("ON"))
                {
                    cbant36204.Checked = true;
                }
                else
                {
                    cbant36204.Checked = false;
                }

                if (configuration.READER6204.ANTENNA1.Equals("ON"))
                {
                    cbant416204.Checked = true;
                }
                else
                {
                    cbant416204.Checked = false;
                }

                if (configuration.READER6204.ANTENNA1.Equals("ON"))
                {
                    cbant16204.Checked = true;
                }
                else
                {
                    cbant416204.Checked = false;
                }

                if (configuration.READER6204.ANTENNA1.Equals("ON"))
                {
                    cbant16204.Checked = true;
                }
                else
                {
                    cbant416204.Checked = false;
                }

                if (configuration.READER6204.ANTENNA1.Equals("ON"))
                {
                    cbant16204.Checked = true;
                }
                else
                {
                    cbant416204.Checked = false;
                }

                if (configuration.READER6204.ANTENNA1.Equals("ON"))
                {
                    cbant16204.Checked = true;
                }
                else
                {
                    cbant416204.Checked = false;
                }


                try {
                    int atenuacion6204 = int.Parse(configuration.READER6204.ATTENUATION);
                    tbAttenuation6204.Value = atenuacion6204 / 10;
                    lbatt6204.Text = (tbAttenuation6204.Value*10).ToString();

                    int atenuacion4000 = int.Parse(configuration.READER4000.ATTENUATION);
                    tbAttenuation4000.Value = atenuacion4000 / 10;
                    lbatt4000.Text = (tbAttenuation4000.Value * 10).ToString();

                } catch (Exception ex) {
                    Console.WriteLine(ex.Message);
                }

                
               
            }
        }

        private int fillingConfig() {


            return 0;
        }


        private void tbAttenuation6204_Scroll(object sender, EventArgs e)
        {
            int valor = tbAttenuation6204.Value;

            lbatt6204.Text = (valor * 10).ToString();
        }

        private void tbAttenuation4000_Scroll(object sender, EventArgs e)
        {
            int valor = tbAttenuation4000.Value;

            lbatt4000.Text = (valor * 10).ToString();
        }

        private void Sel6204_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void SelID4000_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void btCancel_Click(object sender, EventArgs e)
        {

        }
    }
}
