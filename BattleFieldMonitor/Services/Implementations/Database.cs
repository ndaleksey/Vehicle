using Npgsql;
using Swsu.BattleFieldMonitor.Properties;
using System.Data.Common;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using Swsu.Geo;

namespace Swsu.BattleFieldMonitor.Services.Implementations
{
    internal class Database : IDatabase
    {
        #region Fields
        private readonly DbConnection _connection;

        private readonly Collection<UnmannedVehicle> _unmannedVehicles;
        #endregion

        #region Constructors
        public Database()
        {
            _connection = new NpgsqlConnection(Settings.Default.DatabaseConnectionString);
            _connection.Open();

            _unmannedVehicles = new Collection<UnmannedVehicle>();

            ReloadUnmannedVehicles();
        }
        #endregion

        #region Properties
        public IReadOnlyCollection<IUnmannedVehicle> UnmannedVehicles
        {
            get { return _unmannedVehicles; }
        }
        #endregion

        #region Methods
        private void ReloadUnmannedVehicles()
        {
            _unmannedVehicles.Clear();

            using (var command = _connection.CreateCommand())
            {
                command.CommandText = "SELECT id, display_name, x, y, heading, speed FROM unmanned_vehicle";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var id = reader.GetGuid(0);
                        var displayName = reader.GetString(1);
                        var x = reader.GetDouble(2);
                        var y = reader.GetDouble(3);
                        var heading = reader.GetDouble(4);
                        var speed = reader.GetDouble(5);
                        var location = new GeographicCoordinates(y, x);
                        _unmannedVehicles.Add(new UnmannedVehicle(displayName, location, heading, speed));
                    }
                }
            }
        }
        #endregion
    }
}