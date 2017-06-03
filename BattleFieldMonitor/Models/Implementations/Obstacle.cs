using System;
using Swsu.BattleFieldMonitor.Services.Implementations;
using Swsu.Geo;
using Swsu.Geo.SimpleFeatures;

namespace Swsu.BattleFieldMonitor.Models.Implementations
{
	internal class Obstacle : IdentifiableObject, IObstacle
	{
		#region Constructors

		public Obstacle(Guid id, string displayName, IGeometry geometry, GeographicCoordinates minLocation,
			GeographicCoordinates maxLocation) : base(id)
		{
			DisplayName = displayName;
			MinLocation = minLocation;
			MaxLocation = maxLocation;
			Geometry = geometry;
		}

		#endregion

		#region Properties

		public string DisplayName { get; private set; }
		public GeographicCoordinates MinLocation { get; private set; }
		public GeographicCoordinates MaxLocation { get; private set; }
		public IGeometry Geometry { get; private set; }

		#endregion

		#region Methods

		protected virtual void OnChanged(EventArgs e)
		{
			Changed?.Invoke(this, e);
		}

		internal void Set(string displayName, IGeometry geometry, GeographicCoordinates minLocation,
			GeographicCoordinates maxLocation)
		{
			DisplayName = displayName;
			MinLocation = minLocation;
			MaxLocation = maxLocation;
			Geometry = geometry;
			OnChanged(EventArgs.Empty);
		}

		#endregion

		#region Events

		public event EventHandler Changed;

		#endregion
	}
}