using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Coco.Comsumer
{
    public abstract class Comsumer
    {
        string host = null;
        public string Msg;

        public string Host { get; set; }
        public string TopicName { get; private set; }

        public event Action<string> MsgReceived;
        public Comsumer(string host, string topicName)
        {
            Host = !string.IsNullOrEmpty(host) ? host : "127.0.0.1";
            TopicName = topicName;
        }

        public virtual void Start()
        {
            MsgReceived += SubScribe;
            Thread th = new Thread(new ThreadStart(Get));
            th.Start();
        }

        protected abstract void SubScribe(string msg);

        private bool ConnectToServer(ref TcpClient tcpClient)
        {
            string hostIP = string.IsNullOrEmpty(Host) ? "127.0.0.1" : Host;

            IPAddress ipa = IPAddress.Parse(hostIP);

            IPEndPoint ipe = new IPEndPoint(ipa, 9527);

            try
            {
                tcpClient.Connect(ipe);
                return tcpClient.Connected;
            }
            catch (Exception ex)
            {
                tcpClient.Close();
                Console.WriteLine(ex);
                return false;
            }
        }

        public void Get()
        {
            while (true)
            {
                try
                {
                    TcpClient tcpClient = new TcpClient();
                    if (ConnectToServer(ref tcpClient))
                    {
                        CommunicationBase cb = new CommunicationBase();
                        cb.SendMsg(1.ToString(), tcpClient);
                        cb.ReceiveMsg(tcpClient);
                        cb.SendMsg(TopicName, tcpClient);
                        var msg = cb.ReceiveMsg(tcpClient);
                        if (!string.IsNullOrEmpty(msg))
                        {
                            Msg = msg;
                            MsgReceived?.Invoke(msg);
                        }
                        tcpClient.Close();
                    }
                }
                catch (Exception ex) { Console.WriteLine(ex); }
            }
        }
    }
}
