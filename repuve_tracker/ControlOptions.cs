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
    }
}
