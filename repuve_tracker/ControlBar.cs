﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace repuve_tracker
{
    public  partial  class ControlBar : UserControl
    {

        public static EventHandler Hidding;
        public ControlBar()
        {
            InitializeComponent();
            ControlOptions.Hidding += hidding;
        }

        

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Hidding(1,null);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void hidding(object sender, EventArgs e)
        {
            this.Show();
        }
    }
}
