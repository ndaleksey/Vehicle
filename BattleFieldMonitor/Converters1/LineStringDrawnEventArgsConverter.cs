using System.Linq;
using DevExpress.Mvvm.UI;
using Swsu.BattleFieldMonitor.Common;
using Swsu.Maps.Windows;
using Swsu.Maps.Windows.Tools;
using Swsu.BattleFieldMonitor.Converters1.Parameters;

namespace Swsu.BattleFieldMonitor.Converters1
{
    class LineStringDrawnEventArgsConverter : EventArgsConverterBase<LineStringDrawnEventArgs>
    {
        #region Properties
        public AngleUnit AngleUnit { get; set; } = AngleUnit.Degree;

        #endregion

        #region Constructors

        public LineStringDrawnEventArgsConverter() { }
        #endregion
        
        #region Methods

        protected override object Convert(object sender, LineStringDrawnEventArgs args)
        {
            //return args.Points.Select(p => ToTuple(p.GeographicLocation, AngleUnit)).ToArray();
            //return new PointDrawnParameter(GeographicCoordinatesTuple.FromLocation(args.GeographicalLocation, AngleUnit));
            return new LineDrawnParameter(args.Points.Select(p => ToTuple(p.GeographicLocation, AngleUnit)));
        }

        protected static GeographicCoordinatesTuple ToTuple(GeographicLocation location, AngleUnit unit)
        {
            return new GeographicCoordinatesTuple(unit.FromAngle(location.Latitude), unit.FromAngle(location.Longitude));
        }

        #endregion
    }
}
