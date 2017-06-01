using Swsu.BattleFieldMonitor.Protocol;
using Swsu.Common;
using Swsu.Geo;
using Swsu.Mathematics.LinearAlgebra;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Swsu.BattleFieldMonitor.VehicleSimulator
{
    internal class Program
    {
        #region Constants
        private const double Radius = 1000; // 1 км.

        private const double Speed = 10; // 36 км/ч.
        #endregion

        #region Fields
        private readonly static GeographicCoordinates Center = new GeographicCoordinates(30, 50);

        private readonly static DateTime StartTime = DateTime.Now;
        #endregion

        #region Methods
        private static void GetVehicleInfo(double time, out double latitude, out double longitude, out double heading, out double speed)
        {
            var ellipsoid = Ellipsoid.Wgs84;
            var r = Matrices.EastNorthUpToEarthCentered(Center.Latitude, Center.Longitude);
            var center = ellipsoid.ConvertToGeocentric(Center.Latitude, Center.Longitude, 0);
            var phi = Speed / Radius * time;
            var x = Radius * Math.Cos(phi);
            var y = Radius * Math.Sin(phi);
            var v = new Vector3(center.X, center.Y, center.Z) + r * new Vector3(x, y, 0);
            var l = Ellipsoid.Wgs84.ConvertToGeographic3D(new GeocentricCoordinates(v.X, v.Y, v.Z));
            latitude = l.Latitude;
            longitude = l.Longitude;
            heading = (90 - MathHelpers.RadiansToDegrees(phi + Math.PI / 2)) % 360;

            if (heading < 0)
            {
                heading += 360;
            }

            speed = Speed;
        }

        private static void ListenForIncomingRequests()
        {
            var listener = new TcpListener(IPAddress.Any, DefaultPorts.ClientDriven);
            listener.Start();

            for (;;)
            {
                var client = listener.AcceptTcpClient();
                var clientThread = new Thread(ProcessIncomingRequests);
                clientThread.IsBackground = true;
                clientThread.Start(client);
            }
        }

        private static void Main(string[] args)
        {
            var listenerThread = new Thread(ListenForIncomingRequests);
            listenerThread.IsBackground = true;
            listenerThread.Start();

            Console.WriteLine("Press any key to terminate.");
            Console.ReadKey();
        }

        private static void ProcessIncomingRequests(object parameter)
        {
            using (var client = (TcpClient)parameter)
            {
                var stream = client.GetStream();
                var stub = new ProtocolHandlerStub(Handler.Instance);
                stub.Process(stream, stream);
            }
        }
        #endregion

        #region Nested Types
        private class Handler : ProtocolHandler
        {
            #region Fields
            public static readonly Handler Instance = new Handler();
            #endregion

            #region Constructors
            private Handler()
            {
            }
            #endregion

            #region Methods
            protected override VehicleTelemetry GetUgvTelemetry()
            {
                double latitude, longitude, heading, speed;
                GetVehicleInfo((DateTime.Now - StartTime).TotalSeconds, out latitude, out longitude, out heading, out speed);

                return new VehicleTelemetry
                {
                    Coordinates = new Coordinates3D(latitude, longitude, 0),
                    Heading = (float)heading,
                    Speed = (float)(speed * 3.6)
                };
            }
            #endregion
        }
        #endregion
    }
}