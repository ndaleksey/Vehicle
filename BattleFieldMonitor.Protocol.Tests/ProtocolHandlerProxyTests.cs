using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace Swsu.BattleFieldMonitor.Protocol
{
    [TestClass]
    public class ProtocolHandlerProxyTests
    {
        #region Methods
        [TestMethod]
        public void Should_CallGetTrajectory()
        {
            var request = new MemoryStream();
            var response = new MemoryStream(ReferenceData.GetTrajectory_Response);
            var proxy = new ProtocolHandlerProxy(request, response);

            var result = proxy.GetTrajectory();

            CollectionAssert.AreEqual(ReferenceData.GetTrajectory_Request, request.ToArray());
            Assert.AreEqual((ulong)100500, result.Timestamp);
            Assert.AreEqual(3, result.Waypoints.Count);
            Assert.AreEqual(30.0, result.Waypoints[0].Latitude);
            Assert.AreEqual(50.0, result.Waypoints[0].Longitude);
            Assert.AreEqual(10.0, result.Waypoints[0].Altitude);
            Assert.AreEqual(32.0, result.Waypoints[1].Latitude);
            Assert.AreEqual(52.0, result.Waypoints[1].Longitude);
            Assert.AreEqual(11.0, result.Waypoints[1].Altitude);
            Assert.AreEqual(35.0, result.Waypoints[2].Latitude);
            Assert.AreEqual(55.0, result.Waypoints[2].Longitude);
            Assert.AreEqual(12.0, result.Waypoints[2].Altitude);
        }

        [TestMethod]
        public void Should_CallGetUgvTelemetry()
        {
            var request = new MemoryStream();
            var response = new MemoryStream(ReferenceData.GetUgvTelemetry_Response);
            var proxy = new ProtocolHandlerProxy(request, response);

            var telemetry = proxy.GetUgvTelemetry();

            CollectionAssert.AreEqual(ReferenceData.GetUgvTelemetry_Request, request.ToArray());
            Assert.AreEqual(30.0, telemetry.Coordinates.Latitude);
            Assert.AreEqual(50.0, telemetry.Coordinates.Longitude);
            Assert.AreEqual(10.0, telemetry.Coordinates.Altitude);
            Assert.AreEqual(5.0f, telemetry.Heading);
            Assert.AreEqual(7.0f, telemetry.Pitch);
            Assert.AreEqual(3.0f, telemetry.Roll);
            Assert.AreEqual(9.0f, telemetry.Speed);
        }
        #endregion
    }
}