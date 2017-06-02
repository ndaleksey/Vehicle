using Swsu.Common;
using System.IO;
using System.Runtime.InteropServices;

namespace Swsu.BattleFieldMonitor.Protocol
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct RequestHeader
    {
        #region Fields
        public ProtocolVersion Version;

        public RequestType Type;

        public uint PayloadSize;
        #endregion

        #region Constructors
        public RequestHeader(ProtocolVersion version, RequestType type) : this(version, type, 0)
        {
        }

        public RequestHeader(ProtocolVersion version, RequestType type, uint payloadSize)
        {
            Version = version;
            Type = type;
            PayloadSize = payloadSize;
        }
        #endregion

        #region Methods
        public void Deconstruct(out ProtocolVersion version, out RequestType type, out uint payloadSize)
        {
            version = Version;
            type = Type;
            payloadSize = PayloadSize;
        }

        public void SwapBytes()
        {
            BinaryDataHelpers.SwapBytes(ref PayloadSize);
        }

        public static unsafe bool TryRead(Stream stream, out RequestHeader result)
        {
            RequestHeader value;

            if (stream.TryReadFully((byte*)&value, sizeof(RequestHeader)))
            {
                value.SwapBytes();
                result = value;
                return true;
            }

            return DefaultHelpers.False(out result);
        }

        public unsafe void Write(Stream stream)
        {
            var value = this;
            value.SwapBytes();
            stream.Write((byte*)&value, sizeof(RequestHeader));
        }
        #endregion    
    }
}