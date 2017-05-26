namespace Swsu.BattleFieldMonitor.Services
{
    internal interface IDatabase
    {
        #region Properties
        /*
        IRepository<IBeacon> Beacons
        {
            get;
        }
        */

        IRepository<IUnmannedVehicle> UnmannedVehicles
        {
            get;
        }
        #endregion
    }
}
