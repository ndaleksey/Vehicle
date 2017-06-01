using System.IO;
using System.Runtime.InteropServices;

namespace Swsu.BattleFieldMonitor.Protocol
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct SegmentHeader
    {
        #region Fields
        public float MaximumDeviation;

        public float MaximumSpeed;

        public byte ObstacleAvoidance;
        #endregion

        #region Methods
        public static unsafe SegmentHeader Read(Stream stream)
        {
            SegmentHeader value;
            stream.ReadFully((byte*)&value, sizeof(SegmentHeader));
            value.SwapBytes();
            return value;
        }

        public void SwapBytes()
        {
            BinaryDataHelpers.SwapBytes(ref MaximumDeviation);
            BinaryDataHelpers.SwapBytes(ref MaximumSpeed);
        }

        public unsafe void Write(Stream stream)
        {
            var value = this;
            value.SwapBytes();
            stream.Write((byte*)&value, sizeof(SegmentHeader));
        }
        #endregion
    }
}