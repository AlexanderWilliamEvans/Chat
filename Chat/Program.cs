using System;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Text;
using System.Collections.Generic;

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
            // List<Process> instances = new List<Process>();
            var ipAddress = IPAddress.Parse("127.0.0.1");
            int port = 666;
           /* var process1 = new Process();
            var process2 = new Process();
            var process3 = new Process();
            var process4 = new Process();
            Process[] instances = { process1, process2, process3, process4};*/

            for (var i=0; i<3; i++)
            {
               // new Debugger();
                if (ServerIsRunning(ipAddress, port))
                    {
                    _ = new Client();
                }
                    else
                    {
                    _ = new Server(ipAddress, port);
                }

            }
            
        }

        /// <summary>
        /// Checks if the server is already running
        /// </summary>
        /// <returns>true or false</returns>
        public static bool ServerIsRunning(IPAddress ipAddress, int port)
        {
            using (var tcpClient = new TcpClient())
            {
                try
                {
                   
                    tcpClient.Connect(ipAddress, port);
                }
                catch (SocketException)
                {
                    return false;
                }
                return true;
            }
        } 
   
    }
}
