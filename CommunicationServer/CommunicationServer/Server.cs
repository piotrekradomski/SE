﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using MessageProject;

namespace CommunicationServer
{
    class Server
    {
        static Dictionary<Type, int> typeDict = new Dictionary<Type, int>
        {
            {typeof(ConfirmGameRegistration),0},
            {typeof(ConfirmJoiningGame),1},
            {typeof(RejectJoiningGame),2},
            {typeof(JoinGame),3},
            {typeof(GameMessage),4},
            {typeof(RegisteredGames),5},
            {typeof(GetGamesList),6},
            {typeof(RegisterGame),7}
        };

        static Socket listenerSocket;
        static List<ClientData> clients;

        static void Main(string[] args)
        {
            Console.WriteLine("Starting server on " + GetIP4Address());
            listenerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            clients = new List<ClientData>();

            IPEndPoint ip = new IPEndPoint(IPAddress.Parse(GetIP4Address()), 4242);
            listenerSocket.Bind(ip);

            Thread listenThread = new Thread(ListenThread);
            listenThread.Start();
        }

        static void ListenThread()
        {
            while (true)
            {
                listenerSocket.Listen(0);
                clients.Add(new ClientData(listenerSocket.Accept()));
            }
        }

        public static void DataIn(object clientData)
        {
            ClientData client = (ClientData)clientData;
            Socket clientSocket = client.clientSocket;

            byte[] buffer;
            int readBytes;

            while (true)
            {
                try
                {
                    buffer = new byte[clientSocket.SendBufferSize];
                    readBytes = clientSocket.Receive(buffer);

                    if (readBytes > 0)
                    {
                        string xml = Encoding.ASCII.GetString(buffer);
                        DataManager(xml, client);
                    }
                }

                catch
                {

                }
                
            }
        }

        public static string GetIP4Address()
        {
            IPAddress[] ips = Dns.GetHostAddresses(Dns.GetHostName());

            foreach (IPAddress i in ips)
            {
                if (i.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork) return i.ToString();
            }

            return "";
        }

        public static void DataManager(string xml, ClientData client)
        {
            Message temp = Message.xmlIntoMessage(xml);
            dynamic msg = Convert.ChangeType(temp, temp.GetType());

            switch (typeDict[msg.GetType()])
            {
                case 0:
                    
                    break;
                case 1:
                    
                    break;
                case 2:
                    
                    break;
                default:
                    break;
            }

        }
       
    }

    class ClientData
    {
        public Socket clientSocket;
        public Thread clientThread;
        public string id;

        public ClientData()
        {
            id = Guid.NewGuid().ToString();
            clientThread = new Thread(Server.DataIn);
            clientThread.Start(this);
        }
        public ClientData(Socket clientSocket)
        {
            this.clientSocket = clientSocket;
            id = Guid.NewGuid().ToString();
            clientThread = new Thread(Server.DataIn);
            clientThread.Start(this);
        }


    }
}
