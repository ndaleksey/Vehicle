using System.Collections.Generic;

namespace Swsu.BattleFieldMonitor.Services
{
    internal interface IDatabase
    {
        #region Properties
        IReadOnlyCollection<IUnmannedVehicle> UnmannedVehicles
        {
            get;
        }
        #endregion
    }
}
