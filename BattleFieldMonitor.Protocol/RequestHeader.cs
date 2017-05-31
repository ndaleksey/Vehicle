using System.IO;
using System.Runtime.InteropServices;

namespace Swsu.BattleFieldMonitor.Protocol
{
    [StructLayout(LayoutKind.Sequential)]
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

        public static unsafe RequestHeader Read(Stream stream)
        {
            RequestHeader value;
            stream.ReadFully((byte*)&value, sizeof(RequestHeader));
            value.SwapBytes();
            return value;
        }

        public void SwapBytes()
        {
            BinaryDataHelpers.SwapBytes(ref PayloadSize);
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