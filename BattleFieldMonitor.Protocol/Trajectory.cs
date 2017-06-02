using System.IO;

namespace Swsu.BattleFieldMonitor.Protocol
{
    public class Trajectory
    {
        #region Fields
        public ulong Timestamp;
        #endregion

        #region Constructors
        public Trajectory()
        {
            Waypoints = new Coordinates3DCollection();
        }
        #endregion

        #region Properties
        public Coordinates3DCollection Waypoints
        {
            get;
        }

        internal int SizeInBytes
        {
            get { return sizeof(ulong) + Waypoints.SizeInBytes; }
        }
        #endregion

        #region Methods
        internal void Read(Stream stream)
        {
            var timestamp = stream.ReadUInt64();
            BinaryDataHelpers.SwapBytes(ref timestamp);
            Timestamp = timestamp;
            Waypoints.Read(stream);
        }

        internal void Write(Stream stream)
        {
            var timestamp = Timestamp;
            BinaryDataHelpers.SwapBytes(ref timestamp);
            stream.Write(timestamp);
            Waypoints.Write(stream);
        }
        #endregion
    }
}