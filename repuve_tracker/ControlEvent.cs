using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Sirit6204Connector;
using Reader4000Conector;
using HotListSearch;
using vin_decoder;
using log4net;

namespace repuve_tracker
{
    public partial class ControlEvent : UserControl
    {
        public static EventHandler evConected;
        public static EventHandler evDisconected;


        private static ILog hitLogger;
        String _lastVIN = "";
        List<SiritReader> readerList = new List<SiritReader>();
        List<String> readerIPList = new List<String>();
        DataTable tableReads = new DataTable();
        int iCountReads = 0;
        Dictionary<string, string[]> hits = new Dictionary<string, string[]>();
        Hotlistsearch hls;
        Reader4000 Reader;
        SiritReader OldReader;
        bool CurrentStatus = false;

        bool is6204 = true;

        public ControlEvent()
        {
            InitializeComponent();
            InitializeDataTable();
            ControlBar.Conecting += new EventHandler(conecting);
            ControlBar.Disconecting +=new EventHandler(disconecting);
            ControlOptions.SelectingReader += new EventHandler(ReaderSelected);
        }

        

        private void conecting(object sender, EventArgs e) {

            if (ConnectarID4000() == 0)
            {
                evConected(1,null);
            }
            else
            {
                evDisconected(1, null);
            }

        }

        private void disconecting(object sender, EventArgs e){

        }

        private void ReaderSelected(object sender, EventArgs e) {
            string stringreader = (string)sender;
            if (stringreader.Equals("6204"))
            {
                is6204 = true;
            }
            else
            {
                is6204 = false;
            }
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
           // dgvReaders.DataSource = tableReads;
           // this.dgvReaders.DefaultCellStyle.Font = new Font("Tahoma", 14);
           // dgvReaders.Sort(dgvReaders.Columns[4], System.ComponentModel.ListSortDirection.Descending);
        }

        private void Connectar()
        {

            string strIP = "192.168.31.225";
            
            OldReader = new SiritReader(strIP);
                
            int result = OldReader.Connect();
            if (result != 0)
                MessageBox.Show("Reader " + strIP + " Connection Error");
            else
            {
                OldReader.TagReceived += new SiritReader.TagReceivedEventHandler(TagReceived);
                OldReader.ReaderStatus += new SiritReader.ReaderStatusEventHandler(ReaderStatus);
            }
            
           //lblReaderStatus.Text = "Readers Enable";
         
        }

        private int ConnectarID4000() {

            //  lblStatus.Text = "Connecting Reader";

            string strIP = "192.168.31.230";
                Reader = new Reader4000(strIP);
               
                int result = Reader.Connect();
                if (result != 0)
                {
                    MessageBox.Show("Reader " + strIP + " Connection Error");
                    return 1;
                }
                       
                else
                {
                    
                    Reader.TagReceived += new Reader4000.TagReceivedEventHandler(TagReceived4000);
                    Reader.ReaderStatus += new Reader4000.ReaderStatusEventHandler(ReaderStatus4000);
                }
                return 0;

            // lblReaderStatus.Text = "Readers Enable";
        }

        private void Desconectar()
        {
            //#if DEBUG
            //#else
           
            try
            {
                OldReader.TagReceived -= new SiritReader.TagReceivedEventHandler(TagReceived);
                OldReader.Disconnect();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
            
          //  lblReaderStatus.Text = "Reader Disable";
            //#endif
        }

        private void DesconectarID4000()
        {
           
           
                try
                {
                    Reader.TagReceived -= new Reader4000.TagReceivedEventHandler(TagReceived);
                    Reader.Disconnect();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            
        
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
                       // lblReadTags.Text = iCountReads.ToString();
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
                        //dgvReaders.FirstDisplayedScrollingRowIndex = 0;
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            });
        }

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
                        //lblReaderStatus.Text = "Disconnected";
                        if (reader.status)
                        {
                            int a = 1;
                        }
                            //lblReaderStatus.Text = "Connected";
                    }
                   // lblStatusTime.Text = reader.dateTime.ToString("HH:mm:ss");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            });
        }

        private void TagReceived4000(object sender) {
            var tag = (ReadTag4000)sender;
            
            try
            {
                tag.tagVIN = tag.tagVIN.Trim();
                tag.tagVIN = tag.tagVIN.Replace("\0", "");
                if (tag.tagVIN != "" &
                    tag.tagVIN != _lastVIN)
                {
                    iCountReads++;
                    // lblReadTags.Text = iCountReads.ToString();
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
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);

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
                    //dgvReaders.FirstDisplayedScrollingRowIndex = 0;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            
        }

        private void ReaderStatus4000(object sender) {
            var reader = (NewReader4000)sender;
            
            try
            {
                if (reader.status != CurrentStatus)
                {
                    CurrentStatus = reader.status;
                    //lblReaderStatus.Text = "Disconnected";
                    if (reader.status)
                    {
                        int a = 1;
                    }
                    //lblReaderStatus.Text = "Connected";
                }
                // lblStatusTime.Text = reader.dateTime.ToString("HH:mm:ss");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            
        }
    }
}
