using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;

namespace Chat
{
    public class Server
    {
        IPAddress ipAddress;
        int port;
        Socket listenerSocket;
        TcpListener server;
        TcpClient client;
        List<TcpClient> clients;
        NetworkStream stream;

        public Server(IPAddress ipAddress,int port)
        {
            this.ipAddress = ipAddress;
            this.port = port;
            StartServer();
        }
        /// <summary>
        /// Setup a server socket which the clients can connect to
        /// </summary>
        private void StartServer()
        {
            //listenerSocket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            //listenerSocket.Bind(new IPEndPoint(ipAddress, port));
            //listenerSocket.Listen(100);

            //listenerSocket.BeginAccept(OnClientConnected, null);
            server = new TcpListener(ipAddress, port);
            clients = new List<TcpClient>();
            server.Start();
            Console.WriteLine("Server is connected!");

            while (true)
            {
                client = server.AcceptTcpClient();
                AddClient(client);
                Console.WriteLine("New client has connected!");
                Console.WriteLine("Current number of clients connected: " + clients.Count);
                stream = client.GetStream();
                Byte[] bytes = new Byte[1048];
                int k = stream.Read(bytes, 0, bytes.Length);
                string data = null;
                int i;
                // Loop to receive all the data sent by the client.
                while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                {
                    data = Encoding.ASCII.GetString(bytes, 0, i);
                    Console.WriteLine("Received: {0}", data);
                    byte[] msg = Encoding.ASCII.GetBytes(data);
                    stream.Write(msg, 0, msg.Length);
                    Console.WriteLine("Sent: {0}", data);
                }

            }
        }

        private List <TcpClient> AddClient(TcpClient client)
        {
            clients.Add(client);
            return clients;
        }

        /// <summary>
        /// When a client connects to the server, the socket used by the client is connected on the server side
        /// </summary>
        /// <param name="asyncResult"></param>
        private void OnClientConnected(IAsyncResult asyncResult)
        {
            var listenerSocket = (Socket)asyncResult.AsyncState;
            var clientSocket = listenerSocket.EndAccept(asyncResult);
        }
        /// <summary>
        /// Common method for how to start receiving messages
        /// </summary>
        /// <param name="socket"></param>
        private void ReceiveMessage(Socket socket)
        {
            byte [] buffer = new byte[2048];
            socket.BeginReceive(buffer, 0, 2048, 0, OnMessageReceived, socket);
        }
        /// <summary>
        /// When a new message is received, this method is triggered to extract the message
        /// </summary>
        /// <param name="result"></param>
        private void OnMessageReceived(IAsyncResult result)
        {
            var socket = (Socket)result.AsyncState;
            var messageLength = socket.EndReceive(result);
            var buffer = new byte[2048];
            var message = Encoding.UTF8.GetString(buffer, 0, messageLength);
        }
    }
}
