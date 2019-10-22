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
    public partial class ControlList : UserControl
    {
        public ControlList()
        {
            InitializeComponent();

            ControlEvent testevent = new ControlEvent();

            flowLayoutPanel1.Controls.Add(new ControlEvent());
            flowLayoutPanel1.Controls.Add(new ControlEvent());
            flowLayoutPanel1.Controls.Add(new ControlEvent());
            flowLayoutPanel1.Controls.Add(new ControlEvent());
            flowLayoutPanel1.Controls.Add(new ControlEvent());
        }
    }
}
