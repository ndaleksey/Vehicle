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
        public void Process(Stream inputStream, Stream outputStream)
        {
            for (RequestHeader header; RequestHeader.TryRead(inputStream, out header);)
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
                    Process(header.Type, (int)payloadSize, inputStream, outputStream);
                }
                catch (WrongRequestTypeException)
                {
                    new ResponseHeader(ProtocolVersion.V1, ResponseStatus.WrongRequestType).Write(inputStream);
                }
                catch (MalformedPayloadException)
                {
                    new ResponseHeader(ProtocolVersion.V1, ResponseStatus.Malformed).Write(inputStream);
                }
                catch (Exception)
                {
                    new ResponseHeader(ProtocolVersion.V1, ResponseStatus.UnknownError).Write(inputStream);
                }
            }
        }

        private static ResponseHeader EncodeResponse(int payloadSize)
        {
            return new ResponseHeader(ProtocolVersion.V1, ResponseStatus.OK, (uint)payloadSize);
        }

        private void Process(RequestType requestType, int payloadSize, Stream inputStream, Stream outputStream)
        {
            switch (requestType)
            {
                case RequestType.GetReturnPoint:
                    ProcessGetReturnPoint(payloadSize, inputStream, outputStream);
                    break;

                case RequestType.GetUgvTelemetry:
                    ProcessGetUgvTelemetry(payloadSize, inputStream, outputStream);
                    break;

                default:
                    throw new WrongRequestTypeException();
            }
        }

        private unsafe void ProcessGetReturnPoint(int payloadSize, Stream inputStream, Stream outputStream)
        {
            if (payloadSize != 0)
            {
                throw new MalformedPayloadException();
            }

            var result = _handler.GetReturnPoint();
            EncodeResponse(SizeOf.Coordinates3D).Write(outputStream);
            result.Write(outputStream);
        }

        private unsafe void ProcessGetUgvTelemetry(int payloadSize, Stream inputStream, Stream outputStream)
        {
            if (payloadSize != 0)
            {
                throw new MalformedPayloadException();
            }

            var result = _handler.GetUgvTelemetry();
            EncodeResponse(SizeOf.VehicleTelemetry).Write(outputStream);
            result.Write(outputStream);
        }
        #endregion
    }
}