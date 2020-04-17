using System;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Text;

namespace Chat
{
    public class Program
    {
        /// <summary>
        /// Start method of the CLI-application
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {

            for (var i=0; i<3; i++)
            {
                using (Process myProcess = new Process())
                {
                    if (ServerIsRunning())
                    {
                        Client client = new Client();
                    }
                    else
                    {
                        Server server = new Server();
                    }
                }

            }
            
        }

        /// <summary>
        /// Checks if the server is already running
        /// </summary>
        /// <returns></returns>
        public static bool ServerIsRunning()
        {
            using (var tcpClient = new TcpClient())
            {
                try
                {
                    var ipAddress = IPAddress.Parse("127.0.0.1");
                    tcpClient.Connect(ipAddress, 666);
                }
                catch (SocketException)
                {
                    return false;
                }
                return true;
            }
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
            var buffer = new byte[2048];
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
        /// <summary>
        /// Starts a client which connects to the server
        /// </summary>
        public void StartClient()
        {
            var client = new TcpClient();
            client.Connect(IPAddress.Loopback, 666);
        }
        /// <summary>
        /// Send message via a socket
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="message"></param>
        private void SendMessage(Socket socket, string message)
        {
            var byteData = Encoding.UTF8.GetBytes(message);
            socket.Send(byteData);
        }
    }
}
