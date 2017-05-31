using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace Swsu.BattleFieldMonitor.Protocol
{
    [TestClass]
    public class ProtocolHandlerProxyTests
    {
        #region Methods
        [TestMethod]
        public void Should_CallGetUgvTelemetry()
        {
            var request = new MemoryStream();
            var response = new MemoryStream(GetGetUgvTelemetryResponse());
            var proxy = new ProtocolHandlerProxy(request, response);

            var telemetry = proxy.GetUgvTelemetry();

            CollectionAssert.AreEqual(GetGetUgvTelemetryRequest(), request.ToArray());
            Assert.AreEqual(30.0, telemetry.Coordinates.Latitude);
            Assert.AreEqual(50.0, telemetry.Coordinates.Longitude);
            Assert.AreEqual(10.0, telemetry.Coordinates.Altitude);
            Assert.AreEqual(5.0f, telemetry.Heading);
            Assert.AreEqual(7.0f, telemetry.Pitch);
            Assert.AreEqual(3.0f, telemetry.Roll);
            Assert.AreEqual(9.0f, telemetry.Speed);
        }

        private static byte[] GetGetUgvTelemetryRequest()
        {
            using (var stream = new MemoryStream())
            {
                stream.WriteByte((byte)ProtocolVersion.V1);
                stream.WriteByte((byte)RequestType.GetUgvTelemetry);
                stream.Write(BigEndian.GetBytes((uint)0));
                return stream.ToArray();
            }
        }

        private static byte[] GetGetUgvTelemetryResponse()
        {
            using (var stream = new MemoryStream())
            {
                stream.WriteByte((byte)ProtocolVersion.V1);
                stream.WriteByte((byte)ResponseStatus.OK);
                stream.Write(BigEndian.GetBytes((uint)SizeOf.VehicleTelemetry));
                stream.Write(BigEndian.GetBytes(30.0));
                stream.Write(BigEndian.GetBytes(50.0));
                stream.Write(BigEndian.GetBytes(10.0));
                stream.Write(BigEndian.GetBytes(5.0f));
                stream.Write(BigEndian.GetBytes(7.0f));
                stream.Write(BigEndian.GetBytes(3.0f));
                stream.Write(BigEndian.GetBytes(9.0f));
                return stream.ToArray();
            }
        }
        #endregion
    }
}