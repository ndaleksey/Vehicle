using Npgsql;
using Swsu.BattleFieldMonitor.Properties;
using Swsu.Geo;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading;

namespace Swsu.BattleFieldMonitor.Services.Implementations
{
    internal class Database : IDatabase
    {
        #region Fields
        private readonly UnmannedVehiclesImpl _unmannedVehicles;
        #endregion

        #region Constructors
        public Database()
        {
            _unmannedVehicles = new UnmannedVehiclesImpl(this);

            SynchronizationContext = SynchronizationContext.Current;
            new Thread(LoadObjectsAndWaitForChanges).Start();
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
        private void LoadObjectsAndWaitForChanges()
        {
            using (var connection = new NpgsqlConnection(Settings.Default.DatabaseConnectionString))
            {
                connection.Open();
                LoadUnmannedVehicles(connection);

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
        #endregion

        #region Nested Types
        private class UnmannedVehiclesImpl : IRepository<IUnmannedVehicle>
        {
            #region Fields
            private readonly List<UnmannedVehicle> _objects = new List<UnmannedVehicle>();

            private readonly Database _outer;
            #endregion

            #region Constructors
            public UnmannedVehiclesImpl(Database outer)
            {
                _outer = outer;
            }
            #endregion

            #region Properites
            public IReadOnlyCollection<IUnmannedVehicle> Objects
            {
                get { return _objects; }
            }
            #endregion

            #region Methods
            protected virtual void OnObjectsAdded(ObjectsAddedEventArgs<IUnmannedVehicle> e)
            {
                ObjectsAdded?.Invoke(this, e);
            }

            protected virtual void OnObjectsRemoved(ObjectsRemovedEventArgs<IUnmannedVehicle> e)
            {
                ObjectsRemoved?.Invoke(this, e);
            }

            internal void Add(IReadOnlyCollection<UnmannedVehicle> objects)
            {
                _outer.SynchronizationContext.Send(AddCallback, objects);
            }

            private void AddCallback(object state)
            {
                var objects = (IReadOnlyCollection<UnmannedVehicle>)state;
                _objects.AddRange(objects);
                OnObjectsAdded(new ObjectsAddedEventArgs<IUnmannedVehicle>(objects, ObjectsAdditionReason.Loaded));
            }
            #endregion

            #region Events
            public event EventHandler<ObjectsAddedEventArgs<IUnmannedVehicle>> ObjectsAdded;

            public event EventHandler<ObjectsRemovedEventArgs<IUnmannedVehicle>> ObjectsRemoved;
            #endregion
        }
        #endregion
    }
}