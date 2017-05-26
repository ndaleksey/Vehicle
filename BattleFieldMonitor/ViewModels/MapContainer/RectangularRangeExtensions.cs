using Swsu.Maps.Windows;

namespace Swsu.BattleFieldMonitor.ViewModels.MapContainer
{
    internal static class RectangularRangeExtensions
    {
        internal static RectangularEnvelope ToEnvelope(this RectangularRange range, LengthUnit lengthUnit)
        {
            return new RectangularEnvelope(
                lengthUnit.FromLength(range.XMinimum), lengthUnit.FromLength(range.XMaximum),
                lengthUnit.FromLength(range.YMinimum), lengthUnit.FromLength(range.YMaximum));
        }
    }
}
