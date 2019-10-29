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
        ControlBar conbar;
        ControlEvent conevent;
        ControlList conlist;
        ControlOptions conOpts;

        public static EventHandler ForceDisconectreader;

        public Form2()
        {
            InitializeComponent();
            conbar = new ControlBar();
            conevent = new ControlEvent();
            conlist = new ControlList();
            conOpts = new ControlOptions();
            panel1.Controls.Add(conbar);
            panel1.Controls.Add(conOpts);
            panel2.Controls.Add(conevent);
            panel3.Controls.Add(conlist);
            ControlBar.Hidding += new EventHandler(hidding);
            FormOptions.Showing += new EventHandler(showing);
            
        }

        private void hidding(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void showing(object sender, EventArgs e)
        {
            this.Show();
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }


        private void form2_close(object sender, EventArgs e)
        {
            ///try to disconect reader
            this.Hide();
            ForceDisconectreader(1,null);

            conevent.Dispose();
            conbar.Dispose();
            conOpts.Dispose();
            conlist.Dispose();
        }
    }
}
