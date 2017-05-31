using System.Runtime.InteropServices;

namespace Swsu.BattleFieldMonitor.Protocol
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Coordinates3D
    {
        #region Fields
        public double Latitude;

        public double Longitude;

        public double Altitude;
        #endregion

        #region Methods
        public void SwapBytes()
        {
            BinaryDataHelpers.SwapBytes(ref Latitude);
            BinaryDataHelpers.SwapBytes(ref Longitude);
            BinaryDataHelpers.SwapBytes(ref Altitude);
        }
        #endregion
    }
}
