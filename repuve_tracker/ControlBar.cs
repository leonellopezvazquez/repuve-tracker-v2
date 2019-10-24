using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace repuve_tracker
{
    public partial class ControlBar : UserControl
    {

        public static EventHandler Hidding;
        public static EventHandler Showing;
        public static EventHandler Conecting;
        public static EventHandler Disconecting;


        public ControlBar()
        {
            InitializeComponent();
            ControlOptions.Hidding += hidding;
            ControlEvent.evConected += new EventHandler(readerConected);
            ControlEvent.evDisconected += new EventHandler(readerDisconected);
        }

        private void readerConected(object sender, EventArgs e)
        {
            lbconected.Text = "Conected";
            this.imdispStatus.Image = Properties.Resources.Sem_Verde;
        }

        private void readerDisconected(object sender, EventArgs e)
        {
            lbconected.Text = "disconected";
            this.imdispStatus.Image = Properties.Resources.Sem_Rojo;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Hidding(1, null);
        }


        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Conecting(1,null);
        }

    
        private void hidding(object sender, EventArgs e)
        {
            this.Show();
        }


        private void pictureBox2_Click_1(object sender, EventArgs e)
        {
            //this.Hide();
            //Hidding(1, null);
        }

       
        private void pictureBox1_Click(object sender, MouseEventArgs e)
        {
            Conecting(1, null);
        }



        private void pictureBox1_Click_1(object sender, EventArgs e)
        {

        }

        private void btOptions_Click(object sender, EventArgs e)
        {
            FormOptions opciones = new FormOptions();
            opciones.Show();
            Hidding(1, null);
        }

        private void btConnect_Click(object sender, EventArgs e)
        {
            Conecting(1, null);
        }
    }
}
