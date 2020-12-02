using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;

namespace Coco.Comsumer
{
    public class Comsumer
    {
        string host = null;
        public string Msg;

        public string Host { get; set; } = "127.0.0.1:9527";
        public string TopicName { get; set; }

        public event Action<string> MsgReceived;
        public Comsumer()
        {

        }
        public Comsumer(string host, string topicName)
        {
            Host = !string.IsNullOrEmpty(host) ? host : "127.0.0.1";
            TopicName = topicName;
        }
        public void Configuration(Action<Comsumer> action)
        {
            action(this);
        }

        public virtual void SubScribe(Action<string> callback)
        {
            MsgReceived += callback;
            Thread th = new Thread(new ThreadStart(Get));
            th.Start();
        }

        public virtual void SubScribe<T>(Action<T> callback) where T : class
        {
            new Thread(() => GetObject(callback)).Start();
        }

        public virtual void SubScribe(string topicName, Action<string> callback)
        {
            Thread th = new Thread(() => Get(Host, topicName, callback));
            th.Start();
        }

        public virtual void SubScribe<T>(string topicName, Action<T> callback) where T : class
        {
            new Thread(() => GetObject(Host, topicName, callback)).Start();
        }

        public virtual void SubScribe(string host, string topicName, Action<string> callback)
        {
            Thread th = new Thread(() => Get(host, topicName, callback));
            th.Start();
        }

        public virtual void SubScribe<T>(string host, string topicName, Action<T> callback) where T : class
        {
            new Thread(() => GetObject(host, topicName, callback)).Start();
        }


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

        private bool ConnectToServer(ref TcpClient tcpClient, string host)
        {
            string[] hp = host.Split(':');
            string hostIP = hp[0] ?? "127.0.0.1";

            IPAddress ipa = IPAddress.Parse(hp[0]);

            IPEndPoint ipe = new IPEndPoint(ipa, Convert.ToInt32(hp[1]));

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

        public void Get(string host, string topicName, Action<string> callback)
        {
            while (true)
            {
                try
                {
                    TcpClient tcpClient = new TcpClient();
                    if (ConnectToServer(ref tcpClient, host))
                    {
                        CommunicationBase cb = new CommunicationBase();
                        var content = $"1^^^{topicName}";
                        cb.SendMsg(content, tcpClient);
                        var msg = cb.ReceiveMsg(tcpClient);
                        if (!string.IsNullOrEmpty(msg) && msg != "Ok")
                        {
                            callback(msg);
                        }
                        tcpClient.Close();
                    }
                }
                catch (Exception ex) { Console.WriteLine(ex); }
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
                        var content = $"1^^^{TopicName}";
                        cb.SendMsg(content, tcpClient);
                        var msg = cb.ReceiveMsg(tcpClient);
                        if (!string.IsNullOrEmpty(msg) && msg != "Ok")
                        {
                            MsgReceived?.Invoke(msg);
                        }
                        tcpClient.Close();
                    }
                }
                catch (Exception ex) { Console.WriteLine(ex); }
            }
        }

        public void GetObject<T>(Action<T> callback)
        {
            while (true)
            {
                try
                {
                    TcpClient tcpClient = new TcpClient();
                    if (ConnectToServer(ref tcpClient))
                    {
                        CommunicationBase cb = new CommunicationBase();
                        var content = $"1^^^{TopicName}";
                        cb.SendMsg(content, tcpClient);
                        var msg = cb.ReceiveMsg(tcpClient);
                        if (!string.IsNullOrEmpty(msg) && msg != "Ok")
                        {
                            T t = JsonSerializer.Deserialize<T>(msg, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
                            callback(t);
                        }
                        tcpClient.Close();
                    }
                }
                catch (Exception ex) { Console.WriteLine(ex); }
            }
        }
        public void GetObject<T>(string host, string topicName, Action<T> callback)
        {
            while (true)
            {
                try
                {
                    TcpClient tcpClient = new TcpClient();
                    if (ConnectToServer(ref tcpClient, host))
                    {
                        CommunicationBase cb = new CommunicationBase();
                        var content = $"1^^^{topicName}";
                        cb.SendMsg(content, tcpClient);
                        var msg = cb.ReceiveMsg(tcpClient);
                        if (!string.IsNullOrEmpty(msg) && msg != "Ok")
                        {
                            T t = JsonSerializer.Deserialize<T>(msg, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
                            callback(t);
                        }
                        tcpClient.Close();
                    }
                }
                catch (Exception ex) { Console.WriteLine(ex); }
            }
        }
    }
}
