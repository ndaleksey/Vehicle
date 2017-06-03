using Swsu.BattleFieldMonitor.Models;

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

		IRepository<IObstacle> Obstacles
		{
			get;
		}
		#endregion
	}
}
