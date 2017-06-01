using System;
using System.Diagnostics.Contracts;
using System.IO;

namespace Swsu.BattleFieldMonitor.Protocol
{
    public abstract class ProtocolHandlerProxyBase
    {
        #region Constants
        public const int MaximumPayloadSize = 1 << 30; // 1 GB
        #endregion

        #region Constructors
        protected ProtocolHandlerProxyBase(Stream requestStream, Stream responseStream)
        {
            RequestStream = requestStream;
            ResponseStream = responseStream;
        }
        #endregion

        #region Properties
        protected Stream RequestStream
        {
            get;
        }

        protected Stream ResponseStream
        {
            get;
        }
        #endregion

        #region Methods
        protected void ReadResponseHeader(out int payloadSize)
        {
            var header = ResponseHeader.Read(ResponseStream);

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

            payloadSize = size;
        }

        protected void WriteRequestHeader(RequestType requestType)
        {
            WriteRequestHeader(requestType, 0);
        }

        protected void WriteRequestHeader(RequestType requestType, int payloadSize)
        {
            if (payloadSize < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(payloadSize));
            }

            Contract.EndContractBlock();

            new RequestHeader(ProtocolVersion.V1, requestType, (uint)payloadSize).Write(RequestStream);
        }

        private static void CheckStatus(ResponseStatus status)
        {
            switch (status)
            {
                case ResponseStatus.OK:
                    break;

                case ResponseStatus.MalformedPayload:
                    throw new MalformedPayloadException();

                case ResponseStatus.WrongRequestType:
                    throw new WrongRequestTypeException();

                default:
                    throw new ProtocolException();
            }
        }
        #endregion
    }
}
