using Swsu.Geo;

namespace Swsu.BattleFieldMonitor.Services
{
    internal class BeaconState
    {
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
        #endregion
    }
}
