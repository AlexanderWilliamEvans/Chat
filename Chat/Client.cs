using System;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.IO;

namespace Chat
{
    public class Client
    {
        public Client()
        {
            StartClient();

        }
        /// <summary>
        /// Starts a client which connects to the server
        /// </summary>
        public void StartClient()
        {
            try{
                TcpClient tcpClient = new TcpClient("127.0.0.1", 5000);
                Console.WriteLine("Connected to server.");

                Thread thread = new Thread(HandleMessages);
                thread.Start(tcpClient);

                StreamWriter sWriter = new StreamWriter(tcpClient.GetStream());

                while (true)
                {
                    if (tcpClient.Connected)
                    {
                        string input = Console.ReadLine();
                        sWriter.WriteLine(input);
                        sWriter.Flush();
                    }
                }

            }
            catch (Exception e)
            {
                Console.Write(e.Message);
            }

            Console.ReadKey();

        }
        /// <summary>
        /// Send message via a socket
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="message"></param>
        static void HandleMessages(object obj)
        {
            TcpClient tcpClient = (TcpClient)obj;
            StreamReader sReader = new StreamReader(tcpClient.GetStream());

            while (true)
            {
                try
                {
                    string message = sReader.ReadLine();
                    Console.WriteLine(message);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    break;
                }
            }
        }
    }
}
