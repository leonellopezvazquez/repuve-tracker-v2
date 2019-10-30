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
    public partial class ControlSecondEvent : UserControl 
    {
        public static int count;
        public ControlSecondEvent(EventData evento)
        {
            InitializeComponent();
            this.btExpand.Visible = true;
            paintSettings(evento);
            if (!evento.IsHit) {
                this.IcHit.Visible = false;
            }
            count++;       
        }

        private void btCut_Click(object sender, EventArgs e)
        {
            this.Size = new System.Drawing.Size(350, 80);
            this.btExpand.Visible = true;
        }

        private void btExpand_Click(object sender, EventArgs e)
        {
            this.Size = new System.Drawing.Size(350, 160);
            this.btExpand.Visible = false;
        }


        public void paintSettings(EventData evento)
        {
            this.lFolio.Text = evento.folio;
            this.lVIN.Text = evento.VIN;
            this.lYear.Text = evento.year;
            this.lModel.Text = evento.model;
            this.lTS.Text = evento.dateTime;
        }

        private void IcHit_Click(object sender, EventArgs e)
        {

        }
    }
}
