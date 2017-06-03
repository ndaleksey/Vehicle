using System;
using System.Collections.Generic;
using System.Data.Common;
using NLog;
using Swsu.Geo;
using Swsu.Geo.SimpleFeatures;


namespace Swsu.BattleFieldMonitor.DataAccess
{
	internal static class ObstacleQueries
	{
		#region Properties

		private static ILogger Logger { get; } = LogManager.GetCurrentClassLogger();

		#endregion

		#region Methods

		public static IEnumerable<Record> SelectAll(DbConnection connection)
		{
			using (var command = connection.CreateCommand())
			{
				command.CommandText = "SELECT id, display_name, geometry, x_min, y_min, x_max, y_max FROM nkb_vs.obstacle";

				foreach (var r in GetResults(command))
					yield return r;
			}
		}

		public static IEnumerable<Record> SelectByIds(DbConnection connection, Guid[] ids)
		{
			using (var command = connection.CreateCommand())
			{
				command.CommandText = "SELECT id, display_name, geometry, x_min, y_min, x_max, y_max FROM nkb_vs.obstacle WHERE id = ANY(@ids::uuid[])";
				command.AddParameter("ids").Value = ids;

				foreach (var r in GetResults(command))
					yield return r;
			}
		}

		private static IEnumerable<Record> GetResults(DbCommand command)
		{
			using (var reader = command.ExecuteReader())
				while (reader.Read())
					yield return Record.Create(reader);
		}

		#endregion

		#region Nested Types

		public struct Record
		{
			#region Properties

			public Guid Id { get; set; }

			public string DisplayName { get; set; }
			public IGeometry Geometry { get; set; }

			public GeographicCoordinates MinLocation { get; set; }
			public GeographicCoordinates MaxLocation { get; set; }

			#endregion

			#region Methods

			internal static Record Create(DbDataReader reader)
			{
				IGeometry geometry = null;
				try
				{
					geometry = Geometries.FromText(reader.GetString(2));
				}
				catch (Exception e)
				{
					// TODO: должна возвращаться пустая геометрия

					Logger.Error(e);
				}

				return new Record
				{
					Id = reader.GetGuid(0),
					DisplayName = reader.GetString(1),
					Geometry = geometry,
					MinLocation = new GeographicCoordinates(reader.GetDouble(4), reader.GetDouble(3)),
					MaxLocation = new GeographicCoordinates(reader.GetDouble(6), reader.GetDouble(5)),
				};
			}

			#endregion
		}

		#endregion

		/*#region Methods

		private static IEnumerable<Obstacle> GetObstacles(DbCommand command)
		{
			using (var reader = command.ExecuteReader())
			{
				while (reader.Read())
				{
					var id = reader.GetGuid(0);
					var displayName = reader.GetString(1);
					var geometry = reader.GetString(2);
					var xMin = reader.GetDouble(3);
					var yMin = reader.GetDouble(4);
					var xMax = reader.GetDouble(5);
					var yMax = reader.GetDouble(6);

					IGeometry g = null;
					try
					{
						g = Geometries.FromText(geometry);
					}
					catch (Exception e)
					{
						Logger.Error(e);
					}

					if (g != null)
						yield return
							new Obstacle(id, displayName, g, new GeographicCoordinates(yMin, xMin), new GeographicCoordinates(yMax, xMax));
				}
			}
		}

		public static Task<List<Obstacle>> SelectAllAsync(DbConnection connection)
		{
			return Task.Run(() => SelectAll(connection));
		}

		public static List<Obstacle> SelectAll(DbConnection connection)
		{
			if (connection.State != ConnectionState.Open) throw new ApplicationException("Ошибка обращения к БД. Соединение закрыто");

			var obstacles = new List<Obstacle>();

			using (var cmd = connection.CreateCommand())
			{
				cmd.CommandText = "SELECT id, display_name, geometry, x_min, y_min, x_max, y_max FROM nkb_vs.obstacle";

				obstacles.AddRange(GetObstacles(cmd));
			}
			return obstacles;
		}

		public static Task SelectByIdsAsync(DbConnection connection, Guid[] ids)
		{
			return Task.Run(() => SelectByIds(connection, ids));
		}

		public static List<Obstacle> SelectByIds(DbConnection connection, Guid[] ids)
		{
			if (connection.State != ConnectionState.Open) throw new ApplicationException("Ошибка обращения к БД. Соединение закрыто");

			if (ids == null) throw new NullReferenceException("Список идентификаторов препятствий равен NULL");
			
			var obstacles = new List<Obstacle>();

			if (ids.Length == 0) return obstacles;

			using (var cmd = connection.CreateCommand())
			{
				cmd.CommandText =
					"SELECT id, display_name, geometry, x_min, y_min, x_max, y_max FROM nkb_vs.obstacle WHERE id = ANY(@ids::uuid[])";
				cmd.AddParameter("ids");

				obstacles.AddRange(GetObstacles(cmd));
			}

			return obstacles;
		}

		public static Task UpdateObstacleAsync(DbConnection connection, Obstacle obstacle)
		{
			return Task.Run(() => UpdateObstacle(connection, obstacle));
		}

		public static void UpdateObstacle(DbConnection connection, Obstacle obstacle)
		{
			if (connection.State != ConnectionState.Open) throw new ApplicationException("Ошибка обращения к БД. Соединение закрыто");

			if (obstacle == null) throw new NullReferenceException("Препятствие равно NULL");

			using (var cmd = connection.CreateCommand())
			{
				cmd.CommandText = "UPDATE nkb_vs.obstacle " +
				                  "SET " +
								  "display_name = @display_name, " +
				                  "geometry = @geometry, " +
				                  "x_min = @x_min, " +
				                  "y_min = @y_min, " +
				                  "x_max = @x_max, " +
				                  "y_max = @y_max " +
								  "WHERE id = @id";

				cmd.AddParameter("id", DbType.Guid, obstacle.Id);
				cmd.ExecuteNonQuery();
			}
		}

		#endregion*/
	}
}