using System;
using Swsu.BattleFieldMonitor.Services;
using Swsu.Geo;
using Swsu.Geo.SimpleFeatures;

namespace Swsu.BattleFieldMonitor.Models
{
	internal interface IObstacle : IIdentifiableObject
	{
		#region Properties

		string DisplayName { get; }
		GeographicCoordinates MinLocation { get; }
		GeographicCoordinates MaxLocation { get; }
		IGeometry Geometry { get; }

		#endregion

		#region Events

		event EventHandler Changed;

		#endregion
	}
}