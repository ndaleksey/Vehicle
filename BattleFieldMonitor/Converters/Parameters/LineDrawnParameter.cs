using System.Collections.Generic;
using Swsu.BattleFieldMonitor.Common;

namespace Swsu.BattleFieldMonitor.Converters.Parameters
{
    public class LineDrawnParameter
    {
        #region Properties

        public IList<GeographicCoordinatesTuple> Locations { get; }

        #endregion

        #region Constructors

        public LineDrawnParameter(IEnumerable<GeographicCoordinatesTuple> locations)
        {
            Locations = new List<GeographicCoordinatesTuple>(locations);
        }

        #endregion

    }
}
