using System;
using System.IO;

namespace Swsu.BattleFieldMonitor.Protocol
{
    public class ProtocolHandlerProxy : IProtocolHandler
    {
        #region Constants
        public const int MaximumPayloadSize = 1 << 30; // 1 GB
        #endregion

        #region Fields
        private readonly Stream _stream;
        #endregion

        #region Constructors
        public ProtocolHandlerProxy(Stream stream)
        {
            _stream = stream;
        }
        #endregion

        #region Methods
        public VehicleTelemetry GetControlCenterTelemetry()
        {
            throw new NotImplementedException();
        }

        public unsafe VehicleTelemetry GetUgvTelemetry()
        {
            new RequestHeader(ProtocolVersion.V1, RequestType.GetUgvTelemetry).Write(_stream);

            var size = DecodeAndGetPayloadSize(ResponseHeader.Read(_stream));

            if (size != sizeof(VehicleTelemetry))
            {
                throw new InvalidPayloadSizeException(size);
            }

            return VehicleTelemetry.Read(_stream);
        }

        private static void CheckStatus(ResponseStatus status)
        {
            switch (status)
            {
                case ResponseStatus.OK:
                    break;

                default:
                    throw new ProtocolException();
            }
        }

        private static int DecodeAndGetPayloadSize(ResponseHeader header)
        {
            if (header.Version != ProtocolVersion.V1)
            {
                throw new ProtocolException();
            }

            CheckStatus(header.Status);

            var size = (int)header.PayloadSize;

            if (size > MaximumPayloadSize)
            {
                throw new ProtocolException();
            }

            return size;
        }
        #endregion
    }
}