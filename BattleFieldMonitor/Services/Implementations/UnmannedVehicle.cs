using Swsu.Geo;
using System;

namespace Swsu.BattleFieldMonitor.Services.Implementations
{
    internal class UnmannedVehicle : IUnmannedVehicle
    {
        #region Constructors
        public UnmannedVehicle(string displayName, GeographicCoordinates location, double heading, double speed)
        {
            DisplayName = displayName;
            Location = location;
            Heading = heading;
            Speed = speed;
        }
        #endregion

        #region Properties
        public string DisplayName
        {
            get;
        }

        public double Heading
        {
            get;
        }

        public GeographicCoordinates Location
        {
            get;
        }

        public double Speed
        {
            get;
        }
        #endregion

        #region Events
        public event EventHandler Changed;
        #endregion
    }
}
