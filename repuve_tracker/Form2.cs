using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace repuve_tracker
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            ControlBar conbar = new ControlBar();
            panel1.Controls.Add(conbar);
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }
    }
}
