using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using NLog;
using Swsu.BattleFieldMonitor.DataAccess;
using Swsu.BattleFieldMonitor.Models;
using Swsu.BattleFieldMonitor.Models.Implementations;
using Swsu.Geo;
using Swsu.Geo.SimpleFeatures;

namespace Swsu.BattleFieldMonitor.Services.Implementations.Repositories
{
	internal class ObstacleRepository : Repository<Obstacle, IObstacle>
	{
		#region Properties

		internal static ILogger Logger { get; } = LogManager.GetCurrentClassLogger();
		#endregion

		#region Methods

		protected override void LoadDeltaCore(DbConnection connection, RepositoryDelta repositoryDelta)
		{
			var records = new List<UpdateRecord>();

			// TODO: Support all kinds of changes. Now only updates are supported.
			foreach (var r in ObstacleQueries.SelectByIds(connection, repositoryDelta.UpdatedObjectIds.ToArray()))
			{
				Obstacle obj;

				if (TryGetObjectById(r.Id, out obj))
					records.Add(new UpdateRecord(obj, r.DisplayName, r.Geometry, r.MinLocation, r.MaxLocation));
			}

			SynchronizationContext.Post(Update, records);
		}

		protected internal override void Reload(DbConnection connection)
		{
			var objects = new List<Obstacle>();

			foreach (var r in ObstacleQueries.SelectAll(connection))
				objects.Add(new Obstacle(r.Id, r.DisplayName, r.Geometry, r.MinLocation, r.MaxLocation));

			Add(objects);
		}

		private void Update(object state)
		{
			var records = (IEnumerable<UpdateRecord>) state;

			foreach (var r in records)
			{
//				Logger.Info(r.DisplayName);
				Debug.WriteLine(r.DisplayName);
				r.Target.Set(r.DisplayName, r.Geometry, r.MinLocation, r.MaxLocation);
			}
		}

		#endregion

		#region Nested Types

		private struct UpdateRecord
		{
			#region Constructors

			public UpdateRecord(Obstacle target, string displayName, IGeometry geometry, GeographicCoordinates minLocation,
				GeographicCoordinates maxLocation)
			{
				Target = target;
				DisplayName = displayName;
				Geometry = geometry;
				MinLocation = minLocation;
				MaxLocation = maxLocation;
			}

			#endregion

			#region Properties

			public string DisplayName { get; }
			public IGeometry Geometry { get; }
			public GeographicCoordinates MinLocation { get; }
			public GeographicCoordinates MaxLocation { get; }
			public Obstacle Target { get; }

			#endregion
		}

		#endregion
	}
}