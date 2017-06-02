using System;
using System.IO;

namespace Swsu.BattleFieldMonitor.Protocol
{
    public class ProtocolHandlerProxy : ProtocolHandlerProxyBase, IProtocolHandler
    {
        #region Constructors
        public ProtocolHandlerProxy(Stream requestStream, Stream responseStream) : base(requestStream, responseStream)
        {
        }
        #endregion

        #region Methods
        public VehicleTelemetry GetControlCenterTelemetry()
        {
            throw new NotImplementedException();
        }

        public Coordinates3D GetReturnPoint()
        {
            throw new NotImplementedException();
        }

        public Trajectory GetTrajectory()
        {
            WriteRequestHeader(RequestType.GetTrajectory);

            int payloadSize;
            ReadResponseHeader(out payloadSize);

            var result = new Trajectory();
            result.Read(ResponseStream);
            return result;
        }

        public VehicleTelemetry GetUgvTelemetry()
        {
            WriteRequestHeader(RequestType.GetUgvTelemetry);

            int payloadSize;
            ReadResponseHeader(out payloadSize);

            if (payloadSize != SizeOf.VehicleTelemetry)
            {
                throw new InvalidPayloadSizeException(payloadSize);
            }

            return VehicleTelemetry.Read(ResponseStream);
        }
        #endregion
    }
}