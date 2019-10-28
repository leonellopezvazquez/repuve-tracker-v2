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
            this.btExpand.Visible = false;
            paintSettings();
            count++;
            lFolio.Text = count.ToString();
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


        public void paintSettings()
        {

        }
    }
}
