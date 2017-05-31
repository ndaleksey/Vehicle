using System.IO;
using System.Runtime.InteropServices;

namespace Swsu.BattleFieldMonitor.Protocol
{
    [StructLayout(LayoutKind.Sequential)]
    public struct VehicleTelemetry
    {
        #region Fields
        public Coordinates3D Coordinates;

        public float Heading;

        public float Pitch;

        public float Roll;

        public float Speed;
        #endregion

        #region Methods
        public static unsafe VehicleTelemetry Read(Stream stream)
        {
            VehicleTelemetry value;
            stream.ReadFully((byte*)&value, sizeof(VehicleTelemetry));
            value.SwapBytes();
            return value;
        }

        public void SwapBytes()
        {
            Coordinates.SwapBytes();
            BinaryDataHelpers.SwapBytes(ref Heading);
            BinaryDataHelpers.SwapBytes(ref Pitch);
            BinaryDataHelpers.SwapBytes(ref Roll);
            BinaryDataHelpers.SwapBytes(ref Speed);
        }

        public unsafe void Write(Stream stream)
        {
            var value = this;
            value.SwapBytes();
            stream.Write((byte*)&value, sizeof(VehicleTelemetry));
        }
        #endregion
    }
}
