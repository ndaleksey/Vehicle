using Swsu.Geo;

namespace Swsu.BattleFieldMonitor.Services.Implementations
{
    internal struct UnmannedVehicleState
    {
        #region Constructors
        public UnmannedVehicleState(string displayName, GeographicCoordinates location, double heading, double speed)
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
            set;
        }

        public GeographicCoordinates Location
        {
            get;
            set;
        }

        public double Heading
        {
            get;
            set;
        }

        public double Speed
        {
            get;
            set;
        }
        #endregion
    }
}