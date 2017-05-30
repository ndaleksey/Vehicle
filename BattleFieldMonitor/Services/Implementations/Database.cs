using Npgsql;
using Swsu.BattleFieldMonitor.Properties;
using Swsu.BattleFieldMonitor.Services.Implementations.Notifications;
using Swsu.Geo;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace Swsu.BattleFieldMonitor.Services.Implementations
{
    internal class Database : IDatabase
    {
        #region Constants
        private const string ChannelName = "nkb_vs";
        #endregion

        #region Fields
        private readonly Repository<IUnmannedVehicle> _unmannedVehicles;
        #endregion

        #region Constructors
        public Database()
        {
            _unmannedVehicles = new Repository<IUnmannedVehicle>(SynchronizationContext.Current);

            var thread = new Thread(LoadObjectsAndWaitForChanges);
            thread.IsBackground = true;
            thread.Start();
        }
        #endregion

        #region Properties
        public IRepository<IUnmannedVehicle> UnmannedVehicles
        {
            get { return _unmannedVehicles; }
        }
        #endregion

        #region Methods 
        private Repository FindRepository(string tableName)
        {
            switch (tableName)
            {
                case "unmanned_vehicle":
                    return _unmannedVehicles;

                default:
                    return null;
            }
        }

        private void LoadObjectsAndWaitForChanges()
        {
            using (var connection = new NpgsqlConnection(Settings.Default.DatabaseConnectionString))
            {
                connection.Open();

                LoadUnmannedVehicles(connection);
                connection.Listen(ChannelName);

                connection.Notification += OnNotification;

                for (;;)
                {
                    // ожидаем извещений...
                    connection.Wait();

                    while (_unmannedVehicles.UpdatedObjectIds.Count > 0)
                    {
                        var ids = _unmannedVehicles.UpdatedObjectIds.ToArray();
                        _unmannedVehicles.UpdatedObjectIds.Clear();
                        ReloadUnmannedVehicles(connection, ids);
                    }
                }
            }
        }

        private void LoadUnmannedVehicles(DbConnection connection)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT id, display_name, x, y, heading, speed FROM nkb_vs.unmanned_vehicle";

                using (var reader = command.ExecuteReader())
                {
                    var objects = new List<UnmannedVehicle>();

                    while (reader.Read())
                    {
                        var id = reader.GetGuid(0);
                        var displayName = reader.GetString(1);
                        var x = reader.GetDouble(2);
                        var y = reader.GetDouble(3);
                        var heading = reader.GetDouble(4);
                        var speed = reader.GetDouble(5);
                        var location = new GeographicCoordinates(y, x);
                        objects.Add(new UnmannedVehicle(id, displayName, location, heading, speed));
                    }

                    _unmannedVehicles.Add(objects);
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

            repository.OnNotification(notification.Operation, notification.Old, notification.New);
        }

        private void ReloadUnmannedVehicles(DbConnection connection, Guid[] ids)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT id, display_name, x, y, heading, speed FROM nkb_vs.unmanned_vehicle WHERE id = ANY(@ids::uuid[])";
                command.AddParameter("ids").Value = ids;

                using (var reader = command.ExecuteReader())
                {
                    var objects = new List<UnmannedVehicle>();

                    while (reader.Read())
                    {
                        /*
                        var id = reader.GetGuid(0);
                        var displayName = reader.GetString(1);
                        var x = reader.GetDouble(2);
                        var y = reader.GetDouble(3);
                        var heading = reader.GetDouble(4);
                        var speed = reader.GetDouble(5);
                        var location = new GeographicCoordinates(y, x);
                        objects.Add(new UnmannedVehicle(id, displayName, location, heading, speed));
                        */

                        Trace.TraceInformation("***");
                    }
                }
            }
        }
        #endregion
    }
}