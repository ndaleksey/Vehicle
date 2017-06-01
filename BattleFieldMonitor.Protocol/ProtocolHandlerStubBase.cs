using System;
using System.IO;

namespace Swsu.BattleFieldMonitor.Protocol
{
    public abstract class ProtocolHandlerStubBase
    {
        #region Constants
        public const uint MaximumPayloadSize = 1 << 30;
        #endregion

        #region Constructors
        protected ProtocolHandlerStubBase()
        {
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
                    Process(requestStream, responseStream, header.Type, (int)payloadSize);
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

        protected abstract void Process(Stream requestStream, Stream responseStream, RequestType requestType, int payloadSize);

        protected static void WriteResponseHeader(Stream responseStream, int payloadSize)
        {
            new ResponseHeader(ProtocolVersion.V1, ResponseStatus.OK, (uint)payloadSize).Write(responseStream);
        }
        #endregion
    }
}