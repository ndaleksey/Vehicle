using Swsu.Geo;

namespace Swsu.BattleFieldMonitor.Services
{
    internal interface IBeacon
    {
        #region Properties
        string DisplayName
        {
            get;
        }

        GeographicCoordinates Location
        {
            get;
        }
        #endregion
    }
}
