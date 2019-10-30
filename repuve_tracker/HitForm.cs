using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using vin_decoder;

namespace repuve_tracker
{
    public partial class HitForm : Form
    {
        public HitForm(string[] hit_info)
        {
            InitializeComponent();
            int[] colors = new int[4] { 255, 128, 128, 128 };
            string tmp = "";
            try
            {
                tmp = hit_info[2].Replace("Color [", "").Replace("]","");
                string[] colors_temp = tmp.Split(',');
                colors[0] = int.Parse(colors_temp[0].Replace("A=", "").Replace(",", ""));
                colors[1] = int.Parse(colors_temp[1].Replace("R=", "").Replace(",", ""));
                colors[2] = int.Parse(colors_temp[2].Replace("G=", "").Replace(",", ""));
                colors[3] = int.Parse(colors_temp[3].Replace("B=", "").Replace(",", ""));
            }
            catch (Exception ex)
            {

            }

            label_database.BackColor = Color.FromArgb(colors[0], colors[1], colors[2], colors[3]);
            label_database.Text = hit_info[8];
            label_vrm.Text = hit_info[13];
            label_field1.Text = hit_info[3];
            label_field2.Text = hit_info[4];
            label_field3.Text = hit_info[5];
            label_field4.Text = hit_info[6];
            label_field5.Text = hit_info[7];
            label_make.Text = Vin.GetWorldManufacturer(hit_info[3]);
            label_model.Text = Vin.GetModelYear(hit_info[3]).ToString();
            label_information.Text = hit_info[8];

            try
            {
                System.Media.SoundPlayer player = new System.Media.SoundPlayer(@"sounds\Siren.wav");
                player.Play();
            }
            catch (Exception)
            {

            }
        }

        private void button_ok_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
