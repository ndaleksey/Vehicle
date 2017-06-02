using Swsu.BattleFieldMonitor.Protocol;
using System;
using System.Net.Sockets;
using System.Threading;

namespace Swsu.BattleFieldMonitor.DatabaseUpdater
{
    internal class Program
    {
        #region Methods
        private static void Main(string[] args)
        {
            var thread = new Thread(SendOutgoingRequests);
            thread.IsBackground = true;
            thread.Start();

            Console.WriteLine("Press any key to terminate.");
            Console.ReadKey();
        }

        private static void SendOutgoingRequests()
        {
            Again:
            try
            {
                using (var client = new TcpClient("localhost", DefaultPorts.ClientDriven))
                {
                    var stream = client.GetStream();
                    var proxy = new ProtocolHandlerProxy(stream, stream);

                    for (;;)
                    {
                        UpdateDatabase(proxy.GetUgvTelemetry());
                        Thread.Sleep(200);
                    }
                }
            }
            catch (SocketException)
            {
                goto Again;
            }
        }

        private static void UpdateDatabase(VehicleTelemetry telemetry)
        {
            // TODO: Update the database somehow...
        }
        #endregion
    }
}