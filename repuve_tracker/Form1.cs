using log4net;
using HotListSearch;
using Sirit6204Connector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using vin_decoder;

namespace repuve_tracker
{
    public partial class Form1 : Form
    {

        private static ILog hitLogger;

//#if DEBUG
//        ReadTag rt = new ReadTag();
//        ReadTag rt2 = new ReadTag();
//        ReadTag rt3 = new ReadTag();
//        ReadTag rt4 = new ReadTag();
//#endif

        String _lastVIN = "";
        List<SiritReader> readerList = new List<SiritReader>();
        List<String> readerIPList = new List<String>();
        DataTable tableReads = new DataTable();
        int iCountReads = 0;
        Dictionary<string, string[]> hits = new Dictionary<string, string[]>();

        Hotlistsearch hls;


        public Form1()
        {
            //log4net.Config.XmlConfigurator.Configure();
            hitLogger = LogManager.GetLogger("HitsLogger");
            InitializeComponent();
            InitializeDataTable();

            InitializeHotlist();

//#if DEBUG
//            button_test.Visible = false;
//            button_test2.Visible = false;
//            button_test3.Visible = false;
//            button_test4.Visible = false;
//            rt.anntena = "antenna=1";
//            rt.dateTime = DateTime.Now.ToString();
//            rt.reader = "127.0.0.1";
//            rt.tagEPC = "3030303031323334";
//            rt.tagFolio = "00001234";
//            rt.tagUSER = "0115B65E0000000039425741423035553445503130303134372";
//            rt.tagVIN = "1HGCM71633A004351";

//            rt2.anntena = "antenna=2";
//            rt2.dateTime = DateTime.Now.ToString();
//            rt2.reader = "127.0.0.1";
//            rt2.tagEPC = "3030303031323334";
//            rt2.tagFolio = "00002222";
//            rt2.tagUSER = "002962C1000000004A4D31435732424C3743303132383637342";
//            rt2.tagVIN = "JTDBU4EE9B9157873";

//            rt3.anntena = "antenna=1";
//            rt3.dateTime = DateTime.Now.ToString();
//            rt3.reader = "127.0.0.1";
//            rt3.tagEPC = "3030303031323334";
//            rt3.tagFolio = "00003333";
//            rt3.tagUSER = "013F4FA0000000005653534243324E483046313030363534302";
//            rt3.tagVIN = "KM8JU3AC8DU727335";

//            rt4.anntena = "antenna=2";
//            rt4.dateTime = DateTime.Now.ToString();
//            rt4.reader = "127.0.0.1";
//            rt4.tagEPC = "3030303031323334";
//            rt4.tagFolio = "00004444";
//            rt4.tagUSER = "012DC9A30000000032484746473332383143483930323537322";
//            rt4.tagVIN = "KL1TD56646B599953";

//#endif
        }

        private void InitializeHotlist()
        {
            hls = new Hotlistsearch();
            hls.Initialize();
        }

        private void InitializeDataTable()
        {
            tableReads.Columns.Add("EPC", typeof(string));
            tableReads.Columns.Add("User Data", typeof(string));
            tableReads.Columns.Add("Folio", typeof(string));
            tableReads.Columns.Add("VIN", typeof(string));
            tableReads.Columns.Add("TimeStamp", typeof(string));
            tableReads.Columns.Add("Make", typeof(string));
            tableReads.Columns.Add("Model", typeof(string));
            tableReads.Columns.Add("Antena", typeof(string));
            tableReads.Columns.Add("Hit", typeof(string));
            tableReads.AcceptChanges();
            dgvReaders.DataSource = tableReads;
            this.dgvReaders.DefaultCellStyle.Font = new Font("Tahoma", 14);
            dgvReaders.Sort(dgvReaders.Columns[4], System.ComponentModel.ListSortDirection.Descending);
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {

            if (btnConnect.Text.Equals("Connect")){
                Connectar();
                btnConnect.Text = "Disconnect";
            }
            else
            {
                btnConnect.Text = "Connect";
            }

           
        }

        private void Connectar()
        {
//#if DEBUG
//            lblStatus.Text = "Reader connected - DEBUG";
//            lblReaderStatus.Text = "Reader enabled";
//#else
            lblStatus.Text = "Connecting Reader";
            readerIPList.Add("192.168.31.225");
            //readerIPList.Add("192.168.100.200");
            //readerIPList.Add("192.168.100.201");
            //readerIPList.Add("192.168.100.202");
            //readerIPList.Add("192.168.100.203");
            //readerIPList.Add("192.168.100.204");
            //readerIPList.Add("192.168.100.205");
            //readerIPList.Add("192.168.100.206");
            //readerIPList.Add("192.168.100.207");
            //readerIPList.Add("192.168.100.208");

            foreach (string strIP in readerIPList)
            {
                var Reader = new SiritReader(strIP);
                readerList.Add(Reader);
                int result = Reader.Connect();
                if (result != 0)
                    MessageBox.Show("Reader " + strIP + " Connection Error");
                else
                {
                    Reader.TagReceived += new SiritReader.TagReceivedEventHandler(TagReceived);
                    Reader.ReaderStatus += new SiritReader.ReaderStatusEventHandler(ReaderStatus);
                }
            }
            lblReaderStatus.Text = "Readers Enable";
//#endif
        }

        private void Desconectar()
        {
//#if DEBUG
//#else
            foreach (SiritReader reader in readerList)
            {
                try
                {
                    reader.TagReceived -= new SiritReader.TagReceivedEventHandler(TagReceived);
                    reader.Disconnect();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            readerIPList.Clear();
            readerList.Clear();

            lblReaderStatus.Text = "Reader Disable";
//#endif
        }

        private void TagReceived(object sender)
        {
            var tag = (ReadTag)sender;
            this.Invoke((MethodInvoker)delegate ()
            {
                try
                {
                    tag.tagVIN = tag.tagVIN.Trim();
                    tag.tagVIN = tag.tagVIN.Replace("\0", "");
                    if (tag.tagVIN != "" &
                        tag.tagVIN != _lastVIN)
                    {
                        iCountReads++;
                        lblReadTags.Text = iCountReads.ToString();
                        DataRow t = tableReads.NewRow();
                        t.ItemArray = new Object[] {
                            tag.tagEPC,
                            tag.tagUSER,
                            tag.tagFolio,
                            tag.tagVIN,
                            DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                            Vin.GetWorldManufacturer(tag.tagVIN),
                            Vin.GetModelYear(tag.tagVIN).ToString(),
                            tag.anntena.Equals("antenna=2") ? "Left" : "Right",
                            ""
                        };
                        _lastVIN = tag.tagVIN;

                        try
                        {
                            System.Media.SoundPlayer player = new System.Media.SoundPlayer(@"sounds\Beep.wav");
                            player.Play();
                        }
                        catch (Exception)
                        {

                        }

                        DateTime dt = DateTime.Now;
                        List<string> rwesult = hls.SearchVIN(tag.tagVIN);
                        hitLogger.Debug("Search time: " + (DateTime.Now - dt).TotalMilliseconds.ToString());
                        foreach (string result in rwesult)
                        {
                            t.SetField<string>("Hit", "True");
                            string[] data = result.Split('|');
                            if (data.Length != 15)
                                continue;
                            hitLogger.Info(result);
                            new HitForm(data).ShowDialog();
                        }

                        tableReads.Rows.Add(t);
                        dgvReaders.FirstDisplayedScrollingRowIndex = 0;
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            });
        }

        bool CurrentStatus = false;
        private void ReaderStatus(object sender)
        {
            var reader = (Reader)sender;
            this.Invoke((MethodInvoker)delegate ()
            {
                try
                {
                    if (reader.status != CurrentStatus)
                    {
                        CurrentStatus = reader.status;
                        lblReaderStatus.Text = "Disconnected";
                        if (reader.status)
                            lblReaderStatus.Text = "Connected";
                    }
                    lblStatusTime.Text = reader.dateTime.ToString("HH:mm:ss");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            });
        }

        private void btnShowData_Click(object sender, EventArgs e)
        {
            if (btnShowData.Text.Equals("<"))
            {
                btnShowData.Text = ">";
                dgvReaders.Columns["EPC"].Visible = false;
                dgvReaders.Columns["User Data"].Visible = false;
            }
            else
            {
                btnShowData.Text = "<";
                dgvReaders.Columns["EPC"].Visible = true;
                dgvReaders.Columns["User Data"].Visible = true;
            }

            /*
             * 
             *             tableReads.Columns.Add("EPC", typeof(string));
            tableReads.Columns.Add("User Data", typeof(string));
            tableReads.Columns.Add("Folio", typeof(string));
            tableReads.Columns.Add("VIN", typeof(string));
            tableReads.Columns.Add("TimeStamp", typeof(string));
            tableReads.Columns.Add("Make", typeof(string));
            tableReads.Columns.Add("Model", typeof(string));
            tableReads.Columns.Add("Antena", typeof(string));
            tableReads.Columns.Add("Hit", typeof(string));
             * 
             * 
             * */
        }

        private void button_test_Click(object sender, EventArgs e)
        {
//#if DEBUG
//            if(sender.Equals(button_test))
//                TagReceived(rt);
//            else if (sender.Equals(button_test2))
//                TagReceived(rt2);
//            else if (sender.Equals(button_test3))
//                TagReceived(rt3);
//            else if (sender.Equals(button_test4))
//                TagReceived(rt4);
//#endif
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
//#if DEBUG
//            switch (e.KeyChar)
//            {
//                case '1':
//                    TagReceived(rt);
//                    break;
//                case '2':
//                    TagReceived(rt2);
//                    break;
//                case '3':
//                    TagReceived(rt3);
//                    break;
//                case '4':
//                    TagReceived(rt4);
//                    break;

//            }
//            e.Handled = true;
//            lblStatusTime.Text = DateTime.Now.ToString("HH:mm:ss");
//#endif
        }

        }
    }