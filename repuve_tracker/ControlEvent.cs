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
using System.Xml.Serialization;
using System.IO;

namespace repuve_tracker
{
    public partial class ControlEvent : UserControl
    {
        public static EventHandler evConected;
        public static EventHandler evDisconected;

        public  delegate void NewEventHandler(Object sender);
        public static event NewEventHandler NewEvent;

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
        bool IsConected = false;
        ConfigReader configuracion;

        public delegate void paintData(ReadTag4000 tag);
        public paintData myDelegate1;

        public delegate void paintData6204(ReadTag tag);
        public paintData6204 myDelegate2;
        DateTime dateTime;
        public ControlEvent()
        {
            InitializeComponent();
            InitializeDataTable();
            ControlBar.Conecting += new EventHandler(conecting);
            ControlBar.Disconecting +=new EventHandler(disconecting);
            Form2.ForceDisconectreader += new EventHandler(forceDisconectreader);
            configuracion = new ConfigReader();
            myDelegate1 = new paintData(paintDataMethod4000);
            myDelegate2 = new paintData6204(paintDataMethod6204);

            InitializeHotlist();

            CreateHandle();
    }

        private void InitializeHotlist()
        {
            hls = new Hotlistsearch();
            hls.Initialize();
        }

        private int readConFigFile()
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(ConfigReader));
                using (FileStream fileStream = new FileStream("ConfigReader.xml", FileMode.Open))
                {
                    configuracion = (ConfigReader)serializer.Deserialize(fileStream);
                }

                return 0;

            }
            catch (Exception ex)
            {
                //log.Error(ex);
                return 1;
            }
        }

        private void conecting(object sender, EventArgs e) {

            if (readConFigFile() != 0)
            {
         
                return;
            }

            if (configuracion.ACTUAL.Equals("6204")) {

                if (!IsConected)
                {
                    if (Connectar6204() == 0)
                    {
                        evConected(1, null);
                    }
                    else
                    {
                        evDisconected(1, null);
                    }
                }
                else
                {
                    Desconectar6204();
                    evDisconected(1, null);
                }
            }
            else {
                if (!IsConected)
                {
                    if (ConnectarID4000() == 0)
                    {
                        evConected(1, null);
                    }
                    else
                    {
                        evDisconected(1, null);
                    }
                }
                else
                {
                    DesconectarID4000();
                    evDisconected(1, null);
                }

            }

            ////read configuration file, choose the reader to connect, send configuration data
            

            

        }

        private void disconecting(object sender, EventArgs e){

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

        private int Connectar6204()
        {

            string strIP = "192.168.31.225";
            
            OldReader = new SiritReader(configuracion.READER6204.IPADDRESS, configuracion.READER6204.ANTENNA1, configuracion.READER6204.ANTENNA2, configuracion.READER6204.ANTENNA3, configuracion.READER6204.ANTENNA4, configuracion.READER6204.ATTENUATION);
                
            int result = OldReader.Connect();
            if (result != 0) {
                MessageBox.Show("Reader " + strIP + " Connection Error");
                IsConected = false;
                return 1;
            }
                
            else
            {
                OldReader.TagReceived += new SiritReader.TagReceivedEventHandler(TagReceived);
                OldReader.ReaderStatus += new SiritReader.ReaderStatusEventHandler(ReaderStatus);
                IsConected = true;
                return 0;
            }
            
           //lblReaderStatus.Text = "Readers Enable";
         
        }

        private int ConnectarID4000() {

            //  lblStatus.Text = "Connecting Reader";
           

            
                Reader = new Reader4000(configuracion.READER4000.IPADDRESS,configuracion.READER4000.ANTENNA1, configuracion.READER4000.ANTENNA2, configuracion.READER4000.ANTENNA3, configuracion.READER4000.ANTENNA4, configuracion.READER4000.ATTENUATION);
               
                int result = Reader.Connect();
                if (result != 0)
                {
                    MessageBox.Show("Reader " + configuracion.READER4000.IPADDRESS + " Connection Error");
                IsConected = false;
                return 1;
                }      
                else
                {                
                    Reader.TagReceived += new Reader4000.TagReceivedEventHandler(TagReceived4000);
                    Reader.ReaderStatus += new Reader4000.ReaderStatusEventHandler(ReaderStatus4000);
                }
                IsConected = true;
                return 0;

            // lblReaderStatus.Text = "Readers Enable";
        }

        private void Desconectar6204()
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
            IsConected = false;
            
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
                IsConected = false;
        
        }

        private void TagReceived(object sender)
        {
            var tag = (ReadTag)sender;

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

                    //string test = "JALC5B16667902891";
                    List<string> rwesult = hls.SearchVIN(tag.tagVIN);
                    //List<string> rwesult = hls.SearchVIN(test);
                    //hitLogger.Debug("Search time: " + (DateTime.Now - dt).TotalMilliseconds.ToString());

                    
                    foreach (string result in rwesult)
                    {
                        t.SetField<string>("Hit", "True");
                        string[] data = result.Split('|');
                      //  if (data.Length != 15)
                      //      continue;
                       // hitLogger.Info(result);
                        new HitForm(data).ShowDialog();
                    }

                    tableReads.Rows.Add(t);
                    //dgvReaders.FirstDisplayedScrollingRowIndex = 0;

                    dateTime = DateTime.Now;
                    this.Invoke(this.myDelegate2,
                                   new Object[] { tag });

                    using (EventData ev = new EventData())
                    {
                        ev.brand = "";
                        ev.model = Vin.GetWorldManufacturer(tag.tagVIN);
                        ev.folio = tag.tagFolio;
                        ev.VIN = tag.tagVIN;
                        ev.year = Vin.GetModelYear(tag.tagVIN).ToString();
                        ev.dateTime = dateTime.ToString();
                        NewEvent(ev);
                    }

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

        }

        private void ReaderStatus(object sender)
        {
            var reader = (Reader)sender;
            
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
                if (!reader.status)
                {
                    IsConected = false;
                }
                else
                {
                    IsConected = true;
                }
                // lblStatusTime.Text = reader.dateTime.ToString("HH:mm:ss");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            
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

                    //List<string> rwesult = hls.SearchVIN(tag.tagVIN);
                    //hitLogger.Debug("Search time: " + (DateTime.Now - dt).TotalMilliseconds.ToString());

                    /*
                    foreach (string result in rwesult)
                    {
                        t.SetField<string>("Hit", "True");
                        string[] data = result.Split('|');
                        if (data.Length != 15)
                            continue;
                        hitLogger.Info(result);
                        new HitForm(data).ShowDialog();
                    }*/

                    tableReads.Rows.Add(t);
                    //dgvReaders.FirstDisplayedScrollingRowIndex = 0;

                    dateTime = DateTime.Now;
                    this.Invoke(this.myDelegate1,
                                   new Object[] { tag });

                    using (EventData ev = new EventData())
                    {
                        ev.brand = "";
                        ev.model = Vin.GetWorldManufacturer(tag.tagVIN);
                        ev.folio = tag.tagFolio;
                        ev.VIN = tag.tagVIN;
                        ev.year = Vin.GetModelYear(tag.tagVIN).ToString();
                        ev.dateTime = dateTime.ToString();
                        NewEvent(ev);
                    }

                }
                 

                
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            
        }

        private void paintDataMethod4000(ReadTag4000 tag) {
            this.lFolio.Text = tag.tagFolio;
            this.lPais.Text = Vin.GetWorldManufacturer(tag.tagVIN);
            this.lVIN.Text = tag.tagVIN;
            this.lYear.Text = Vin.GetModelYear(tag.tagVIN).ToString();
            this.lModel.Text = Vin.GetWorldManufacturer(tag.tagVIN);
            this.lTS.Text = dateTime.ToString();
        }

        private void paintDataMethod6204(ReadTag tag)
        {
            this.lFolio.Text = tag.tagFolio;
            this.lPais.Text = Vin.GetWorldManufacturer(tag.tagVIN);
            this.lVIN.Text = tag.tagVIN;
            this.lYear.Text = Vin.GetModelYear(tag.tagVIN).ToString();
            this.lModel.Text = Vin.GetWorldManufacturer(tag.tagVIN);
            this.lTS.Text= dateTime.ToString();
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
                if (!reader.status)
                {
                    IsConected = false;
                }
                else {
                    IsConected = true;
                }
                // lblStatusTime.Text = reader.dateTime.ToString("HH:mm:ss");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            
        }

        ~ControlEvent()
        {
            if (Reader != null)
            {
                DesconectarID4000();
                Reader.Dispose();
            }

            if (OldReader != null)
            {
                Desconectar6204();
                
            }
        }

        private void forceDisconectreader(object sender, EventArgs e) {
            if (Reader != null)
            {
                DesconectarID4000();
                Reader.Dispose();
            }

            if (OldReader != null)
            {
                Desconectar6204();

            }
        }

        private void OnDisposing(object sender, EventArgs e)
        {
            
            if (Reader!=null) {
                DesconectarID4000();
                Reader.Dispose();
            }

            if (OldReader!=null) {
                Desconectar6204();
            }
            
            
        }

    }
}
