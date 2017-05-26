namespace Swsu.BattleFieldMonitor.Services
{
    internal interface IDatabaseCommands
    {
        #region Properties
        IRepositoryCommands<IBeacon, BeaconState> Beacons
        {
            get;
        }
        #endregion
    }
}
