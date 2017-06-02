using System.IO;
using System.Runtime.InteropServices;

namespace Swsu.BattleFieldMonitor.Protocol
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Coordinates3D
    {
        #region Fields
        public double Latitude;

        public double Longitude;

        public double Altitude;
        #endregion

        #region Constructors       
        public Coordinates3D(double latitude, double longitude, double altitude)
        {
            Latitude = latitude;
            Longitude = longitude;
            Altitude = altitude;
        }
        #endregion

        #region Methods
        public void Deconstruct(out double latitude, out double longitude, out double altitude)
        {
            latitude = Latitude;
            longitude = Longitude;
            altitude = Altitude;
        }

        public static unsafe Coordinates3D Read(Stream stream)
        {
            Coordinates3D value;
            stream.ReadFully((byte*)&value, sizeof(Coordinates3D));
            value.SwapBytes();
            return value;
        }

        public void SwapBytes()
        {
            BinaryDataHelpers.SwapBytes(ref Latitude);
            BinaryDataHelpers.SwapBytes(ref Longitude);
            BinaryDataHelpers.SwapBytes(ref Altitude);
        }

        public unsafe void Write(Stream stream)
        {
            var value = this;
            value.SwapBytes();
            stream.Write((byte*)&value, sizeof(Coordinates3D));
        }
        #endregion
    }
}