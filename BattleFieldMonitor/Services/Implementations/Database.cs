using Npgsql;
using Swsu.BattleFieldMonitor.DataAccess;
using Swsu.BattleFieldMonitor.Properties;
using Swsu.BattleFieldMonitor.Services.Implementations.Notifications;
using Swsu.Geo;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Swsu.BattleFieldMonitor.Models;
using Swsu.BattleFieldMonitor.Services.Implementations.Repositories;

namespace Swsu.BattleFieldMonitor.Services.Implementations
{
    internal class Database : IDatabase
    {
        #region Constants
        private const string ChannelName = "nkb_vs";
        #endregion

        #region Fields
        private readonly UnmannedVehicleRepository _unmannedVehicles;
        private readonly ObstacleRepository _obstacles;
        #endregion

        #region Constructors
        public Database()
        {
            _unmannedVehicles = new UnmannedVehicleRepository();
			_obstacles = new ObstacleRepository();

            var thread = new Thread(LoadObjectsAndWaitForChanges);
            thread.IsBackground = true;
            thread.Start();
        }
        #endregion

        #region Properties
        public IRepository<IUnmannedVehicle> UnmannedVehicles => _unmannedVehicles;

	    public IRepository<IObstacle> Obstacles => _obstacles;

	    private IEnumerable<Repository> Repositories
        {
            get
            {
                yield return _unmannedVehicles;
                // yield return _beacons;
                 yield return _obstacles;
                // ...
            }
        }
        #endregion

        #region Methods 
        private Repository FindRepository(string tableName)
        {
            switch (tableName)
            {
                case "unmanned_vehicle":
                    return _unmannedVehicles;

				case "obstacle":
					return _obstacles;

				default:
                    return null;
            }
        }

        private void LoadObjectsAndWaitForChanges()
        {
            using (var connection = new NpgsqlConnection(Settings.Default.DatabaseConnectionString))
            {
                connection.Open();

                foreach (var r in Repositories)
                {
                    r.Reload(connection);
                }

                connection.Listen(ChannelName);
                connection.Notification += OnNotification;

                for (;;)
                {
                    // ожидаем извещений...
                    connection.Wait();

                    while (Repositories.Any(r => r.HasDelta))
                    {
                        foreach (var r in Repositories)
                        {
                            if (r.HasDelta)
                            {
                                r.LoadDelta(connection);
                            }
                        }
                    }
                }
            }
        }

        private void OnNotification(object sender, NpgsqlNotificationEventArgs e)
        {
            Trace.TraceInformation("Got notification: '{0}', '{1}'", e.Condition, e.AdditionalInformation);

            if (e.Condition != ChannelName)
            {
                return;
            }

            // TODO: Add some error-handling...
            var notification = Notification.Parse(e.AdditionalInformation);
            var repository = FindRepository(notification.TableName);

            if (repository == null)
            {
                return;
            }

            repository.ProcessNotification(notification.Operation, notification.Old, notification.New);
        }
        #endregion

        #region Nested Types
        private class UnmannedVehicleRepository : Repository<UnmannedVehicle, IUnmannedVehicle>
        {
            #region Methods
            protected override void LoadDeltaCore(DbConnection connection, RepositoryDelta repositoryDelta)
            {
                var records = new List<UpdateRecord>();

                // TODO: Support all kinds of changes. Now only updates are supported.
                foreach (var r in UnmannedVehicleQueries.SelectByIds(connection, repositoryDelta.UpdatedObjectIds.ToArray()))
                {
                    UnmannedVehicle obj;

                    if (TryGetObjectById(r.Id, out obj))
                    {
                        records.Add(new UpdateRecord(obj, r.DisplayName, r.Location, r.Heading, r.Speed));
                    }
                }

                SynchronizationContext.Post(Update, records);
            }

            protected internal override void Reload(DbConnection connection)
            {
                var objects = new List<UnmannedVehicle>();

                foreach (var r in UnmannedVehicleQueries.SelectAll(connection))
                {
                    objects.Add(new UnmannedVehicle(r.Id, r.DisplayName, r.Location, r.Heading, r.Speed));
                }

                Add(objects);
            }

            private void Update(object o)
            {
                var records = (IEnumerable<UpdateRecord>)o;

                foreach (var r in records)
                {
                    r.Target.Set(r.DisplayName, r.Location, r.Heading, r.Speed);
                }
            }
            #endregion

            #region Nested Types
            private struct UpdateRecord
            {
                #region Constructors
                public UpdateRecord(UnmannedVehicle target, string displayName, GeographicCoordinates location, double heading, double speed)
                {
                    Target = target;
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
                }

                public GeographicCoordinates Location
                {
                    get;
                }

                public double Heading
                {
                    get;
                }

                public double Speed
                {
                    get;
                }

                public UnmannedVehicle Target
                {
                    get;
                }
                #endregion
            }
            #endregion
        }
        #endregion
    }
}