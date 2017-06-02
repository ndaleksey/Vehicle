using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.IO;

namespace Swsu.BattleFieldMonitor.Protocol
{
    [TestClass]
    public class ProtocolHandlerStubTests
    {
        #region Methods
        [TestMethod]
        public void Should_ProcessGetReturnPoint()
        {
            var handlerMock = new Mock<IProtocolHandler>(MockBehavior.Strict);
            handlerMock.Setup(h => h.GetReturnPoint()).Returns(new Coordinates3D(30, 50, 10));
            var stub = new ProtocolHandlerStub(handlerMock.Object);
            var request = new MemoryStream(ReferenceData.GetReturnPoint_Request);
            var response = new MemoryStream();

            stub.Process(request, response);

            handlerMock.Verify(h => h.GetReturnPoint(), Times.Once());
            CollectionAssert.AreEqual(ReferenceData.GetReturnPoint_Response, response.ToArray());
        }

        [TestMethod]
        public void Should_ProcessGetTrajectory()
        {
            var handlerMock = new Mock<IProtocolHandler>(MockBehavior.Strict);
            var waypoint1 = new Coordinates3D(30, 50, 10);
            var waypoint2 = new Coordinates3D(32, 52, 11);
            var waypoint3 = new Coordinates3D(35, 55, 12);
            handlerMock.Setup(h => h.GetTrajectory()).Returns(new Trajectory { Timestamp = 100500, Waypoints = { waypoint1, waypoint2, waypoint3 } });
            var stub = new ProtocolHandlerStub(handlerMock.Object);
            var request = new MemoryStream(ReferenceData.GetTrajectory_Request);
            var response = new MemoryStream();

            stub.Process(request, response);

            handlerMock.Verify(h => h.GetTrajectory(), Times.Once());
            CollectionAssert.AreEqual(ReferenceData.GetTrajectory_Response, response.ToArray());
        }

        [TestMethod]
        public void Should_ProcessGetUgvTelemetry()
        {
            var handlerMock = new Mock<IProtocolHandler>(MockBehavior.Strict);
            var coordinates = new Coordinates3D(30, 50, 10);
            handlerMock.Setup(h => h.GetUgvTelemetry()).Returns(new VehicleTelemetry { Coordinates = coordinates, Heading = 5, Pitch = 7, Roll = 3, Speed = 9 });
            var stub = new ProtocolHandlerStub(handlerMock.Object);
            var request = new MemoryStream(ReferenceData.GetUgvTelemetry_Request);
            var response = new MemoryStream();

            stub.Process(request, response);

            handlerMock.Verify(h => h.GetUgvTelemetry(), Times.Once());
            CollectionAssert.AreEqual(ReferenceData.GetUgvTelemetry_Response, response.ToArray());
        }
        #endregion
    }
}