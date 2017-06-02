using Swsu.Common;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Swsu.BattleFieldMonitor.Protocol
{
    public class Segment
    {
        #region Fields
        private SegmentHeader _header;

        private BlittableCollection<Coordinates3D> _waypoints;
        #endregion

        #region Properties
        public float MaximumDeviation
        {
            get { return _header.MaximumDeviation; }
            set { _header.MaximumDeviation = value; }
        }

        public float MaximumSpeed
        {
            get { return _header.MaximumSpeed; }
            set { _header.MaximumSpeed = value; }
        }

        public byte ObstacleAvoidance
        {
            get { return _header.ObstacleAvoidance; }
            set { _header.ObstacleAvoidance = value; }
        }

        public IList<Coordinates3D> Waypoints
        {
            get { return PropertyHelpers.Initialized(ref _waypoints, () => new Coordinates3DCollection()); }
        }
        #endregion

        #region Methods
        public void Write(Stream stream)
        {
            _header.Write(stream);
        }
        #endregion
    }
}
