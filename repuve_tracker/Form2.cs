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
            ControlEvent conevent = new ControlEvent();
            ControlList conlist = new ControlList();
            ControlOptions conOpts = new ControlOptions();
            panel1.Controls.Add(conbar);
            panel1.Controls.Add(conOpts);
            panel2.Controls.Add(conevent);
            panel3.Controls.Add(conlist);
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }
    }
}
