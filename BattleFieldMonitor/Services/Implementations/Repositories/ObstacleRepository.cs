using System.Data.Common;
using Swsu.BattleFieldMonitor.Models;
using Swsu.BattleFieldMonitor.Models.Implementations;

namespace Swsu.BattleFieldMonitor.Services.Implementations.Repositories
{
	internal class ObstacleRepository : Repository<Obstacle, IObstacle>
	{
		protected override void LoadDeltaCore(DbConnection connection, RepositoryDelta repositoryDelta)
		{
			
		}

		protected internal override void Reload(DbConnection connection)
		{
			
		}
	}
}