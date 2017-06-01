using System;
using System.IO;

namespace Swsu.BattleFieldMonitor.Protocol
{
    public class ProtocolHandlerStub : ProtocolHandlerStubBase
    {
        #region Fields
        private readonly IProtocolHandler _handler;
        #endregion

        #region Constructors
        public ProtocolHandlerStub(IProtocolHandler handler)
        {
            _handler = handler;
        }
        #endregion

        #region Methods        
        protected override void Process(Stream requestStream, Stream responseStream, RequestType requestType, int payloadSize)
        {
            switch (requestType)
            {
                case RequestType.GetReturnPoint:
                    ProcessGetReturnPoint(requestStream, responseStream, payloadSize);
                    break;

                case RequestType.GetTrajectory:
                    ProcessGetTrajectory(requestStream, responseStream, payloadSize);
                    break;

                case RequestType.GetUgvTelemetry:
                    ProcessGetUgvTelemetry(requestStream, responseStream, payloadSize);
                    break;

                default:
                    throw new WrongRequestTypeException();
            }
        }        

        private void ProcessGetReturnPoint(Stream requestStream, Stream responseStream, int payloadSize)
        {
            if (payloadSize != 0)
            {
                throw new MalformedPayloadException();
            }

            var result = _handler.GetReturnPoint();
            WriteResponseHeader(responseStream, SizeOf.Coordinates3D);
            result.Write(responseStream);
        }

        private void ProcessGetTrajectory(Stream requestStream, Stream responseStream, int payloadSize)
        {
            if (payloadSize != 0)
            {
                throw new MalformedPayloadException();
            }

            var result = _handler.GetTrajectory();
            WriteResponseHeader(responseStream, result.SizeInBytes);
            result.Write(responseStream);
        }

        private void ProcessGetUgvTelemetry(Stream requestStream, Stream responseStream, int payloadSize)
        {
            if (payloadSize != 0)
            {
                throw new MalformedPayloadException();
            }

            var result = _handler.GetUgvTelemetry();
            WriteResponseHeader(responseStream, SizeOf.VehicleTelemetry);
            result.Write(responseStream);
        }
        #endregion
    }
}