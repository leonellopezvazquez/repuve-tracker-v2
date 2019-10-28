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

        Queue<ControlSecondEvent> lista;

        public ControlList()
        {
            InitializeComponent();

            ControlEvent.NewEvent += new ControlEvent.NewEventHandler(addevent);

            ControlEvent testevent = new ControlEvent();

            lista = new Queue<ControlSecondEvent>();

            flowLayoutPanel1.Controls.Add(new ControlSecondEvent(null));
            CreateHandle();
        }

        private void addevent(object sender) {
            using (EventData evento = (EventData)sender) {

                ControlSecondEvent mainevent =  new ControlSecondEvent(evento);
                lista.Enqueue(mainevent);
                ControlSecondEvent queueEvent = lista.Peek();

                if (ControlSecondEvent.count>=20) {
                    lista.Dequeue();
                }

                this.Invoke((MethodInvoker)delegate ()
                {
                    flowLayoutPanel1.Controls.Add(mainevent);

                    if (ControlSecondEvent.count >= 20)
                    {
                        flowLayoutPanel1.Controls.RemoveAt(0);
                        queueEvent.Dispose();
                    }
                    
                                    

                });
                    
                //flowLayoutPanel1.Controls.Remove(firstEvent);
                //this.Refresh();
            }

            ///
        }
    }
}
