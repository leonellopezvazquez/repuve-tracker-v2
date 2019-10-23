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
    public partial class ControlOptions : UserControl
    {
        public static EventHandler Hidding;

        public static EventHandler SelectingReader;

        public ControlOptions()
        {
            InitializeComponent();
            ControlBar.Hidding += hidding;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Hidding(1,null);
            
        }

        private void hidding(object sender, EventArgs e)
        {
            this.Show();

        }

        private void Sel6204_CheckedChanged(object sender, EventArgs e)
        {
            SelectingReader("6204",null);         
        }


        private void SelID4000_CheckedChanged(object sender, EventArgs e)
        {
            SelectingReader("ID4000",null);
        }
    }
}
