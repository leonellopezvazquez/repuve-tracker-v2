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
using System.Net;

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
            fillingConfig();
           
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


                if (configuration.READER6204.ANTENNA3.Equals("ON"))
                {
                    cbant36204.Checked = true;
                }
                else
                {
                    cbant36204.Checked = false;
                }

                if (configuration.READER6204.ANTENNA4.Equals("ON"))
                {
                    cbant46204.Checked = true;
                }
                else
                {
                    cbant46204.Checked = false;
                }


                if (configuration.READER4000.ANTENNA1.Equals("ON"))
                {
                    cbant14000.Checked = true;
                }
                else
                {
                    cbant44000.Checked = false;
                }

                if (configuration.READER4000.ANTENNA2.Equals("ON"))
                {
                    cbant24000.Checked = true;
                }
                else
                {
                    cbant24000.Checked = false;
                }

                if (configuration.READER4000.ANTENNA3.Equals("ON"))
                {
                    cbant34000.Checked = true;
                }
                else
                {
                    cbant34000.Checked = false;
                }

                if (configuration.READER4000.ANTENNA4.Equals("ON"))
                {
                    cbant44000.Checked = true;
                }
                else
                {
                    cbant44000.Checked = false;
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

            if (Sel6204.Checked)
            {
                configuration.ACTUAL = "6204";
            }
            else {
                configuration.ACTUAL = "4000";
            }

            ///////ip address

            if (tbIP6204.Text.Equals(tbIP4000.Text)) {
                MessageBox.Show(this, "Los equipos no deben tener la misma direccion IP", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return 1;
            }

            
            string addrString6204 = tbIP6204.Text;
            IPAddress address;
            if (IPAddress.TryParse(addrString6204, out address))
            {      
                configuration.READER6204.IPADDRESS = addrString6204;
            }
            else
            {
                //Invalid IP
                MessageBox.Show(this, "Direccion IP de lector 6204 invalida", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 1;
            }

            string addrString4000 = tbIP4000.Text;
            IPAddress address2;
            if (IPAddress.TryParse(addrString4000, out address2))
            {             
                configuration.READER4000.IPADDRESS = addrString4000;
            }
            else
            {
                //Invalid IP
                MessageBox.Show(this, "Direccion IP de lector ID4000 invalida", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 1;
            }


            if (cbant16204.Checked)
            {
                configuration.READER6204.ANTENNA1 = "ON";
            }
            else {
                configuration.READER6204.ANTENNA1 = "OFF";
            }


            if (cbant26204.Checked)
            {
                configuration.READER6204.ANTENNA2 = "ON";
            }
            else
            {
                configuration.READER6204.ANTENNA2 = "OFF";
            }


            if (cbant36204.Checked)
            {
                configuration.READER6204.ANTENNA3 = "ON";
            }
            else
            {
                configuration.READER6204.ANTENNA3 = "OFF";
            }


            if (cbant46204.Checked)
            {
                configuration.READER6204.ANTENNA4 = "ON";
            }
            else
            {
                configuration.READER6204.ANTENNA4 = "OFF";
            }


            if (cbant14000.Checked)
            {
                configuration.READER4000.ANTENNA1 = "ON";
            }
            else
            {
                configuration.READER4000.ANTENNA1 = "OFF";
            }


            if (cbant24000.Checked)
            {
                configuration.READER4000.ANTENNA2 = "ON";
            }
            else
            {
                configuration.READER4000.ANTENNA2 = "OFF";
            }

            if (cbant34000.Checked)
            {
                configuration.READER4000.ANTENNA3 = "ON";
            }
            else
            {
                configuration.READER4000.ANTENNA3 = "OFF";
            }

            if (cbant44000.Checked)
            {
                configuration.READER4000.ANTENNA4 = "ON";
            }
            else
            {
                configuration.READER4000.ANTENNA4 = "OFF";
            }

            /////attenuations

            configuration.READER6204.ATTENUATION = (tbAttenuation6204.Value * 10).ToString();
            configuration.READER4000.ATTENUATION = (tbAttenuation4000.Value * 10).ToString();

           int res = WriteConfigFile(configuration);

            if (res == 0)
            {
                this.Close();
            }
            else {
                MessageBox.Show(this, "Error en guardado de configuracion", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


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
            this.Close();
        }
    }
}
