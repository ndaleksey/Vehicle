using System.IO;
using System.Runtime.InteropServices;

namespace Swsu.BattleFieldMonitor.Protocol
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct ResponseHeader
    {
        #region Fields
        public ProtocolVersion Version;

        public ResponseStatus Status;

        public uint PayloadSize;
        #endregion

        #region Constructors
        public ResponseHeader(ProtocolVersion version, ResponseStatus status) : this(version, status, 0)
        {
        }

        public ResponseHeader(ProtocolVersion version, ResponseStatus status, uint payloadSize)
        {
            Version = version;
            Status = status;
            PayloadSize = payloadSize;
        }
        #endregion

        #region Methods
        public void Deconstruct(out ProtocolVersion version, out ResponseStatus status, out uint payloadSize)
        {
            version = Version;
            status = Status;
            payloadSize = PayloadSize;
        }

        public static unsafe ResponseHeader Read(Stream stream)
        {
            ResponseHeader value;
            stream.ReadFully((byte*)&value, sizeof(ResponseHeader));
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
            stream.Write((byte*)&value, sizeof(ResponseHeader));
        }
        #endregion
    }
}