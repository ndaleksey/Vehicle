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
            var request = new MemoryStream(GetGetReturnPointRequest());
            var response = new MemoryStream();

            stub.Process(request, response);

            handlerMock.Verify(h => h.GetReturnPoint(), Times.Once());
            CollectionAssert.AreEqual(GetGetReturnPointResponse(), response.ToArray());
        }

        private static byte[] GetGetReturnPointRequest()
        {
            using (var stream = new MemoryStream())
            {
                stream.WriteByte((byte)ProtocolVersion.V1);
                stream.WriteByte((byte)RequestType.GetReturnPoint);
                stream.Write(BigEndian.GetBytes((uint)0));
                return stream.ToArray();
            }
        }

        private static byte[] GetGetReturnPointResponse()
        {
            using (var stream = new MemoryStream())
            {
                stream.WriteByte((byte)ProtocolVersion.V1);
                stream.WriteByte((byte)ResponseStatus.OK);
                stream.Write(BigEndian.GetBytes((uint)SizeOf.Coordinates3D));
                stream.Write(BigEndian.GetBytes(30.0));
                stream.Write(BigEndian.GetBytes(50.0));
                stream.Write(BigEndian.GetBytes(10.0));
                return stream.ToArray();
            }
        }
        #endregion
    }
}
