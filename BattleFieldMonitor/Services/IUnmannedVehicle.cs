using Swsu.Geo;
using System;

namespace Swsu.BattleFieldMonitor.Services
{
    internal interface IUnmannedVehicle : IIdentifiableObject
    {
        #region Properties
        string DisplayName
        {
            get;
        }

        double Heading
        {
            get;
        }

        GeographicCoordinates Location
        {
            get;
        }

        double Speed
        {
            get;
        }
        #endregion

        #region Events
        event EventHandler Changed;
        #endregion
    }
}
