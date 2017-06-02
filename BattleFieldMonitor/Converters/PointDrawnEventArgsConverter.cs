using DevExpress.Mvvm.UI;
using Swsu.BattleFieldMonitor.Common;
using Swsu.BattleFieldMonitor.Converters.Parameters;
using Swsu.Maps.Windows;
using Swsu.Maps.Windows.Tools;

namespace Swsu.BattleFieldMonitor.Converters
{
    public class PointDrawnEventArgsConverter : EventArgsConverterBase<PointDrawnEventArgs>
    {
        #region Constructors
        public PointDrawnEventArgsConverter() { }
        #endregion

        #region Properties
        public AngleUnit AngleUnit { get; set; } = AngleUnit.Degree;
        #endregion

        #region Methods
        protected override object Convert(object sender, PointDrawnEventArgs args)
        {
            return new PointDrawnParameter(GeographicCoordinatesTuple.FromLocation(args.GeographicalLocation, AngleUnit));
        }
        #endregion
    }
}
