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
        private readonly Stream _requestStream;

        private readonly Stream _responseStream;
        #endregion

        #region Constructors
        public ProtocolHandlerProxy(Stream requestStream, Stream responseStream)
        {
            _requestStream = requestStream;
            _responseStream = responseStream;
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

        public unsafe VehicleTelemetry GetUgvTelemetry()
        {
            new RequestHeader(ProtocolVersion.V1, RequestType.GetUgvTelemetry).Write(_requestStream);

            var size = DecodeResponse(ResponseHeader.Read(_responseStream));

            if (size != sizeof(VehicleTelemetry))
            {
                throw new InvalidPayloadSizeException(size);
            }

            return VehicleTelemetry.Read(_responseStream);
        }

        private static void CheckStatus(ResponseStatus status)
        {
            switch (status)
            {
                case ResponseStatus.OK:
                    break;

                case ResponseStatus.Malformed:
                    throw new MalformedPayloadException();

                case ResponseStatus.WrongRequestType:
                    throw new WrongRequestTypeException();

                default:
                    throw new ProtocolException();
            }
        }

        private static int DecodeResponse(ResponseHeader header)
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