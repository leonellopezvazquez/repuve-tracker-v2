using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Xml.Serialization;
using System.Threading;
using System.Net;
using System.IO;

namespace Reader4000Conector
{
    public class Reader4000: IDisposable
    {
        public delegate void TagReceivedEventHandler(Object sender);
        public event TagReceivedEventHandler TagReceived;
        public delegate void ReaderStatusEventHandler(Object sender);
        public event ReaderStatusEventHandler ReaderStatus;

        private static readonly ILog readLogger = LogManager.GetLogger("ReadsLogger");
        private static readonly ILog readerStatusLogger = LogManager.GetLogger("ReaderStatusLogger");
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private System.Timers.Timer timer;
        private int timerInterval = 20;
        private NewReader4000 reader;
        private string[] ReadType = { "arrive", "report", "depart" };

        private SocketTCP4000 SocketCmd;
        private string sCmdEvento = string.Empty;     
        private string prefixEventHeader = "event.tag.";
        private int prefixLength = 17;
        private string[] prefixData = { "tag_id=", "user_data=", "antenna=" };
        private string separatorData = ",";
        private Config configuration = new Config();

        ///config variables
        private string ipAddress = null;
        private IPAddress ip;
        private string antena1;
        private string antena2;
        private string antena3;
        private string antena4;
        private string atenuacion;

        private string  AntennasEnable;

        public Reader4000(string ipAddress,string antena1,string antena2,string antena3,string antena4,string atenuacion)
        {
            this.ipAddress = ipAddress;
            this.antena1 = antena1;
            this.antena2 = antena2;
            this.antena3 = antena3;
            this.antena4 = antena4;
            this.atenuacion = atenuacion;

            ipAddress = "1";
            reader = new NewReader4000();
            reader.name = ipAddress;
            reader.status = false;
            log.Info("Initializing reader " + ipAddress);
        }

        public int Connect()
        {
            if (reader.status)
                return 0;

            if (readConFigFile() == 4)
                return 4;

            if (ipAddress == null || ipAddress == "")
                return 1;
            try
            {
                ip = IPAddress.Parse(ipAddress);
            }
            catch
            {
                return 1;
            }

            SocketCmd = new SocketTCP4000(ip, 50007);
            if (!SocketCmd.TestSocket())
                return 2;

            int result = SocketCmd.StartSocket();
            if (result != 0)
                return 3;

            SocketCmd.ResponseEventCmd += new EventHandler((RecibeEventoCMD));
            SocketCmd.ResponseEventTags += new EventHandler(RecibeEventoTags);

            Thread.Sleep(100);



            SocketCmd.SendCmd("setup.protocols = ISOC\r\n");
            ProcesaEventoCmd();

            SocketCmd.SendCmd(" modem.protocol.isoc.control.user_data_length = 13\r\n");
            ProcesaEventoCmd();

            SocketCmd.SendCmd("modem.protocol.isoc.filter.1.length = 8\r\n");
            ProcesaEventoCmd();

            SocketCmd.SendCmd("modem.protocol.isoc.filter.1.offset = 48\r\n");
            ProcesaEventoCmd();

            SocketCmd.SendCmd("modem.protocol.isoc.filter.1.mask = B0\r\n");
            ProcesaEventoCmd();

            SocketCmd.SendCmd("modem.protocol.isoc.filter.1.action = assert_deassert\r\n");
            ProcesaEventoCmd();

            SocketCmd.SendCmd("modem.protocol.isoc.filter.1.mem_bank = MEMBANK_TID\r\n");
            ProcesaEventoCmd();

            SocketCmd.SendCmd("modem.protocol.isoc.filter.1.enable = 1\r\n");
            ProcesaEventoCmd();

            SocketCmd.SendCmd("modem.protocol.isoc.filtering.enable = 1\r\n");
            ProcesaEventoCmd();

            AntennasEnable = "";
            if (antena1.Equals("ON")) {
                AntennasEnable = "1";
            }
            if (antena2.Equals("ON"))
            {
                AntennasEnable += " 2";
            }
            if (antena3.Equals("ON"))
            {
                AntennasEnable += " 3";
            }
            if (antena4.Equals("ON"))
            {
                AntennasEnable += " 4";
            }
            //antenas
            SocketCmd.SendCmd("antennas.mux_sequence="+AntennasEnable +"\r\n");
            ProcesaEventoCmd();

            //atenuacion
            try
            {
                int att = int.Parse(atenuacion);
                SocketCmd.SendCmd("antennas.1.advanced.attenuation="+ atenuacion + "\r\n");
                ProcesaEventoCmd();
                SocketCmd.SendCmd("antennas.2.advanced.attenuation=" + atenuacion + "\r\n");
                ProcesaEventoCmd();
                SocketCmd.SendCmd("antennas.3.advanced.attenuation=" + atenuacion + "\r\n");
                ProcesaEventoCmd();
                SocketCmd.SendCmd("antennas.4.advanced.attenuation=" + atenuacion + "\r\n");
                ProcesaEventoCmd();
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }

            SocketCmd.SendCmd("tag.reporting." + configuration.EventTag + "_fields = tag_id antenna user_data\r\n");
            ProcesaEventoCmd();

            SocketCmd.SendCmd("reader.events.register(" + SocketCmd.IdEventTags + ",event.tag." + configuration.EventTag + ")\r\n");
            ProcesaEventoCmd();

            SocketCmd.SendCmd("setup.operating_mode = active\r\n");
            ProcesaEventoCmd();

            MonitorThread();

            changeReaderStatus();

            log.Info("Reader Connected @ " + ipAddress);
            return 0;
        }


        public int Disconnect()
        {
            try
            {

                SocketCmd.SendCmd("setup.operating_mode = standby\r\n");
                ProcesaEventoCmd();

                Thread.Sleep(50);

                SocketCmd.SendCmd("reader.events.unregister(" + SocketCmd.IdEventTags + ",event.tag." + configuration.EventTag + ")\r\n");
                ProcesaEventoCmd();

                SocketCmd.ResponseEventCmd -= new EventHandler((RecibeEventoCMD));
                SocketCmd.ResponseEventTags -= new EventHandler((RecibeEventoTags));
                SocketCmd.StopSocket();

                timer.Stop();

                if (!reader.status)
                    return 0;
                else
                    changeReaderStatus();

                log.Info("Reader Disconnected @ " + ipAddress);

                return 0;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return 1;
            }
        }

        private int readConFigFile()
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Config));
                using (FileStream fileStream = new FileStream("config.xml", FileMode.Open))
                    configuration = (Config)serializer.Deserialize(fileStream);

                timerInterval = Convert.ToInt16(configuration.HeartBeat);

                if (!ReadType.Contains(configuration.EventTag))
                    return 4;
                return 0;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return 4;
            }
        }


        private void RecibeEventoTags(object sender, EventArgs e)
        {
            try
            {
                if (!reader.status)
                    return;
                ReadTag4000 tag = ParseEvent((string)sender);
                if (tag != null)
                    TagReceived(tag);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        private void RecibeEventoCMD(object sender, EventArgs e)
        {
            sCmdEvento = sender.ToString();
            log.Info(ipAddress + " command: " + sender.ToString());
        }

        private string ProcesaEventoCmd()
        {
            int Count = 0;
            string response = string.Empty;
            while (sCmdEvento == string.Empty && Count < 150)
            {
                Count++;
                Thread.Sleep(10);
            }
            response = sCmdEvento;
            sCmdEvento = string.Empty;
            return response;
        }

        public ReadTag4000 ParseEvent(string sData)
        {
            try
            {
                if (sData.Contains(prefixEventHeader))
                    sData = sData.Substring(prefixLength);
                else
                    return null;
                sData = sData.Replace(" ", "");
                String[] splitDato = sData.Split(',');
                if (splitDato.Length != 3)
                    return null;

                ReadTag4000 tag = new ReadTag4000();
                splitDato[0] = splitDato[0].Replace("0x", "");
                tag.tagEPC = splitDato[0].Substring(prefixData[0].Length, splitDato[0].Length - prefixData[0].Length - 1);
                tag.tagFolio = getEPCValue(tag.tagEPC.Substring(0, 16));

                if (splitDato[1].Length > prefixData[1].Length)
                {
                    splitDato[1] = splitDato[1].Replace("0x", "");
                    tag.tagUSER = splitDato[1].Substring(prefixData[1].Length, splitDato[1].Length - prefixData[1].Length - 1);
                    tag.tagVIN = getEPCValue(tag.tagUSER.Substring(16)).Substring(0, 17);
                }

                tag.anntena = splitDato[2];
                tag.reader = ip.ToString();
                tag.dateTime = DateTime.Now.ToString();
                readLogger.Info(tag.ToString());
                return tag;
            }
            catch
            {
                return null;
            }
        }

        private void MonitorThread()
        {
            timer = new System.Timers.Timer(timerInterval * 1000);
            timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            timer.Start();
        }

        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            timer.Stop();

            if (!SocketCmd.Status())
            {
                if (reader.status)
                {
                    changeReaderStatus();
                    ReaderStatus(reader);
                }
                ReconnectReader();
            }
            else
            {
                reader.dateTime = DateTime.Now;
                MonitorThread();
                ReaderStatus(reader);
            }
        }

        private void changeReaderStatus()
        {
            if (reader != null)
            {
                reader.status = !reader.status;
                reader.dateTime = DateTime.Now;
                readerStatusLogger.Info(reader.ToString());
            }
        }

        private void ReconnectReader()
        {
            try
            {
                log.Error("Communication Lost with the Reader: " + ipAddress);
                log.Error("Reconnection attemp Reader @ " + ipAddress);
                Disconnect();
                if (Connect() != 0)
                    MonitorThread();
            }
            catch
            {
                MonitorThread();
            }
        }

        private static String getEPCValue(String epcCode)
        {
            byte[] ascciValues = null;
            try
            {
                ascciValues = new byte[epcCode.Length / 2];
                for (int i = 0; i < ascciValues.Length; i++)
                {
                    int p = Convert.ToInt32((epcCode.Substring((2 * i), 2)), 16);
                    ascciValues[i] = (byte)p;
                }
                return new ASCIIEncoding().GetString(ascciValues);
            }
            catch (Exception)
            {
                return "ERROR";
            }
        }

        public void Dispose()
        {
            //Disconnect();
        }



        /// <summary>
        /// Al destruirse la clase se intenta liberar los recursos del lector empleados por esta libreria
        /// </summary>
        ~Reader4000()
        {
            Disconnect();
        }
    }
}
