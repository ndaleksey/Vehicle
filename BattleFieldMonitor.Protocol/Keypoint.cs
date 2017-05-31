using System.Runtime.InteropServices;

namespace Swsu.BattleFieldMonitor.Protocol
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Keypoint
    {
        #region Fields
        public Coordinates3D Coordinates;

        public uint StayTime;
        #endregion

        #region Constructors
        public Keypoint(Coordinates3D coordinates, uint stayTime)
        {
            Coordinates = coordinates;
            StayTime = stayTime;
        }
        #endregion

        #region Methods
        public void SwapBytes()
        {
            Coordinates.SwapBytes();
            BinaryDataHelpers.SwapBytes(ref StayTime);
        }
        #endregion
    }
}