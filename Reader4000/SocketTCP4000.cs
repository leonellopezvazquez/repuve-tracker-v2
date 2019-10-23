using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.Net.NetworkInformation;
using System.IO;
using System.Net.Sockets;
using System.Net;

namespace Reader4000Conector
{
    public class SocketTCP4000
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public event EventHandler ResponseEventCmd, ResponseEventTags;
        public int IdEventTags = 0;
        private IPAddress IpServer;
        private IPEndPoint IPEndPointCmd, IPEndPointTag;
        private NetworkStream NetStreamTag, NetStreamSendCmd;
        private StreamReader streamReaderTag;
        private Socket ClientsocketCmd, ClientsocketTag;
        private Thread ThreadTag;
        private bool EnableSocket = false;
        private Ping ping = new Ping();
        private PingReply pingresult;

        public SocketTCP4000(IPAddress IP, Int32 Port) {
            try
            {
                IpServer = IP;

                IPEndPointCmd = new IPEndPoint(IpServer, 50007);
                ClientsocketCmd = new Socket(IPEndPointCmd.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                IPEndPointTag = new IPEndPoint(IpServer, 50008);
                ClientsocketTag = new Socket(IPEndPointTag.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        public bool TestSocket()
        {
            try
            {
                pingresult = ping.Send(IpServer);
                if (pingresult.Status.ToString() == "Success")
                    return true;
                return false;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return false;
            }
        }

        public int StartSocket()
        {
            try
            {
                ClientsocketTag.Connect(IPEndPointTag);
                //ClientsocketCmd.Connect(IPEndPointCmd);

                NetStreamTag = new NetworkStream(ClientsocketTag);
                streamReaderTag = new StreamReader(NetStreamTag);

                ThreadTag = new Thread(RecibeEvtTags);
                ThreadTag.Start();
                EnableSocket = true;
                return 0;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return 1;
            }
        }


        private bool StartSocketCMD()
        {
            try
            {
                if (!ClientsocketCmd.Connected)
                {
                    ClientsocketCmd = new Socket(IPEndPointCmd.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                    ClientsocketCmd.Connect(IPEndPointCmd);
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }


        private bool StopSocketCMD()
        {
            try
            {
                if (ClientsocketCmd.Connected)
                    ClientsocketCmd.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }



        public bool StopSocket()
        {
            try
            {
                EnableSocket = false;
                Thread.Sleep(100);

                if (ClientsocketTag.Connected)
                    ClientsocketTag.Close();
                return true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return false;
            }
        }


        public int SendCmd(string Dato)
        {
            try
            {
                if (!StartSocketCMD())
                    return 1;

                log.Info(Dato);

                NetStreamSendCmd = new NetworkStream(ClientsocketCmd);
                StreamWriter streamWriter = new StreamWriter(NetStreamSendCmd);
                StreamReader streamReader = new StreamReader(NetStreamSendCmd);
                streamWriter.AutoFlush = true;
                streamWriter.WriteLine(Dato);
                ResponseEventCmd(streamReader.ReadLine(), null);
                NetStreamSendCmd.Close();

                StopSocketCMD();

                return 0;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return 1;
            }
        }


        public void RecibeEvtTags()
        {
            try
            {
                while (EnableSocket)
                {
                    string responseValue = streamReaderTag.ReadLine();
                    if (responseValue != "")
                    {
                        ResponseEventTags(responseValue, null);
                        NetStreamTag.Flush();
                        if (responseValue.Contains("event.connection id ="))
                            IdEventTags = Convert.ToInt16(responseValue.Substring(21));
                    }
                }
                ClientsocketTag.Close();
            }
            catch (Exception ex)
            {
                if (EnableSocket)
                    log.Error(ex);
            }
        }

        public bool Status()
        {
            try
            {
                return TestSocket() && ClientsocketTag.Connected;
            }
            catch
            {
                return false;
            }
        }


    }
}
