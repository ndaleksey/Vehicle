using Swsu.BattleFieldMonitor.Common;

namespace Swsu.BattleFieldMonitor.Converters.Parameters
{
    public class PointDrawnParameter
    {
        #region Constructors

        public PointDrawnParameter(GeographicCoordinatesTuple location)
        {
            Location = location;
        }

        #endregion

        #region Properties

        public GeographicCoordinatesTuple Location { get; }

        #endregion
    }
}
