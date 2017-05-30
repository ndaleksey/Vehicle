using Npgsql;
using Swsu.BattleFieldMonitor.Properties;
using Swsu.BattleFieldMonitor.Services.Implementations.Notifications;
using Swsu.Geo;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
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
            _unmannedVehicles = new Repository<IUnmannedVehicle>(this);

            SynchronizationContext = SynchronizationContext.Current;
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

        private SynchronizationContext SynchronizationContext
        {
            get;
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
                        objects.Add(new UnmannedVehicle(displayName, location, heading, speed));
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
        #endregion

        #region Nested Types     
        private class Repository
        {
            #region Methods
            internal void OnNotification(Operation operation, PrimaryKey oldKey, PrimaryKey newKey)
            {
                switch (operation)
                {
                    case Operation.Insert:
                        if (newKey == null)
                        {
                            // TODO: Issue some warning...
                            return;
                        }

                        OnNotificationInsert(newKey.Id);
                        break;

                    case Operation.Update:
                        if (oldKey == null || newKey == null)
                        {
                            // TODO: Issue some warning...
                            return;
                        }

                        OnNotificationUpdate(oldKey.Id, newKey.Id);
                        break;

                    case Operation.Delete:
                        if (oldKey == null)
                        {
                            // TODO: Issue some warning...
                            return;
                        }

                        OnNotificationDelete(oldKey.Id);
                        break;
                }
            }

            private void OnNotificationDelete(Guid id)
            {
                Trace.TraceInformation("Object deleted.");
            }

            private void OnNotificationInsert(Guid id)
            {
                Trace.TraceInformation("Object inserted.");
            }

            private void OnNotificationUpdate(Guid oldId, Guid newId)
            {
                Trace.TraceInformation("Object updated.");
            }
            #endregion
        }

        private class Repository<T> : Repository, IRepository<T>
        {
            #region Fields
            private readonly List<T> _objects = new List<T>();

            private readonly Database _outer;
            #endregion

            #region Constructors
            public Repository(Database outer)
            {
                _outer = outer;
            }
            #endregion

            #region Properites
            public IReadOnlyCollection<T> Objects
            {
                get { return _objects; }
            }
            #endregion

            #region Methods
            protected virtual void OnObjectsAdded(ObjectsAddedEventArgs<T> e)
            {
                ObjectsAdded?.Invoke(this, e);
            }

            protected virtual void OnObjectsRemoved(ObjectsRemovedEventArgs<T> e)
            {
                ObjectsRemoved?.Invoke(this, e);
            }

            internal void Add(IReadOnlyCollection<T> objects)
            {
                _outer.SynchronizationContext.Send(AddCallback, objects);
            }            

            private void AddCallback(object state)
            {
                var objects = (IReadOnlyCollection<T>)state;
                _objects.AddRange(objects);
                OnObjectsAdded(new ObjectsAddedEventArgs<T>(objects, ObjectsAdditionReason.Loaded));
            }
            #endregion

            #region Events
            public event EventHandler<ObjectsAddedEventArgs<T>> ObjectsAdded;

            public event EventHandler<ObjectsRemovedEventArgs<T>> ObjectsRemoved;
            #endregion
        }
        #endregion
    }
}