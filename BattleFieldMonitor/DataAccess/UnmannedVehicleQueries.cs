using Swsu.Geo;
using System;
using System.Collections.Generic;
using System.Data.Common;

namespace Swsu.BattleFieldMonitor.DataAccess
{
    internal static class UnmannedVehicleQueries
    {
        #region Methods
        public static IEnumerable<Record> SelectAll(DbConnection connection)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT id, display_name, x, y, heading, speed FROM nkb_vs.unmanned_vehicle";

                foreach (var r in GetResults(command))
                {
                    yield return r;
                }
            }
        }

        public static IEnumerable<Record> SelectByIds(DbConnection connection, Guid[] ids)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT id, display_name, x, y, heading, speed FROM nkb_vs.unmanned_vehicle WHERE id = ANY(@ids::uuid[])";
                command.AddParameter("ids").Value = ids;

                foreach (var r in GetResults(command))
                {
                    yield return r;
                }
            }
        }

        private static IEnumerable<Record> GetResults(DbCommand command)
        {
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    yield return Record.Create(reader);
                }
            }
        }
        #endregion

        #region Nested Types
        public struct Record
        {
            #region Properties
            public Guid Id
            {
                get;
                set;
            }

            public string DisplayName
            {
                get;
                set;
            }

            public GeographicCoordinates Location
            {
                get;
                set;
            }

            public double Heading
            {
                get;
                set;
            }

            public double Speed
            {
                get;
                set;
            }
            #endregion

            #region Methods
            internal static Record Create(DbDataReader reader)
            {
                return new Record
                {
                    Id = reader.GetGuid(0),
                    DisplayName = reader.GetString(1),
                    Location = new GeographicCoordinates(reader.GetDouble(3), reader.GetDouble(2)),
                    Heading = reader.GetDouble(4),
                    Speed = reader.GetDouble(5)
                };
            }
            #endregion
        }
        #endregion
    }
}
