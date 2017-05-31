using System;
using System.IO;

namespace Swsu.BattleFieldMonitor.Protocol
{
    public class ProtocolHandlerStub
    {
        #region Constants
        public const uint MaximumPayloadSize = 1 << 30;
        #endregion

        #region Fields
        private readonly IProtocolHandler _handler;

        private readonly byte[] _payload = new byte[1 << 10];
        #endregion

        #region Constructors
        public ProtocolHandlerStub(IProtocolHandler handler)
        {
            _handler = handler;
        }
        #endregion

        #region Methods
        public void Process(Stream requestStream, Stream responseStream)
        {
            for (RequestHeader header; RequestHeader.TryRead(requestStream, out header);)
            {
                if (header.Version != ProtocolVersion.V1)
                {
                    throw new ProtocolException(); // May be we need to send an appropriate error response.
                }

                var payloadSize = header.PayloadSize;

                if (payloadSize > MaximumPayloadSize)
                {
                    throw new PayloadToLargeException();
                }

                try
                {
                    Process(header.Type, (int)payloadSize, requestStream, responseStream);
                }
                catch (WrongRequestTypeException)
                {
                    new ResponseHeader(ProtocolVersion.V1, ResponseStatus.WrongRequestType).Write(responseStream);
                }
                catch (MalformedPayloadException)
                {
                    new ResponseHeader(ProtocolVersion.V1, ResponseStatus.Malformed).Write(responseStream);
                }
                catch (Exception)
                {
                    new ResponseHeader(ProtocolVersion.V1, ResponseStatus.UnknownError).Write(responseStream);
                }
            }
        }

        private static ResponseHeader EncodeResponse(int payloadSize)
        {
            return new ResponseHeader(ProtocolVersion.V1, ResponseStatus.OK, (uint)payloadSize);
        }

        private void Process(RequestType requestType, int payloadSize, Stream requestStream, Stream responseStream)
        {
            switch (requestType)
            {
                case RequestType.GetReturnPoint:
                    ProcessGetReturnPoint(payloadSize, requestStream, responseStream);
                    break;

                case RequestType.GetUgvTelemetry:
                    ProcessGetUgvTelemetry(payloadSize, requestStream, responseStream);
                    break;

                default:
                    throw new WrongRequestTypeException();
            }
        }

        private unsafe void ProcessGetReturnPoint(int payloadSize, Stream requestStream, Stream responseStream)
        {
            if (payloadSize != 0)
            {
                throw new MalformedPayloadException();
            }

            var result = _handler.GetReturnPoint();
            EncodeResponse(SizeOf.Coordinates3D).Write(responseStream);
            result.Write(responseStream);
        }

        private unsafe void ProcessGetUgvTelemetry(int payloadSize, Stream requestStream, Stream responseStream)
        {
            if (payloadSize != 0)
            {
                throw new MalformedPayloadException();
            }

            var result = _handler.GetUgvTelemetry();
            EncodeResponse(SizeOf.VehicleTelemetry).Write(responseStream);
            result.Write(responseStream);
        }
        #endregion
    }
}