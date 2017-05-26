namespace Swsu.BattleFieldMonitor.ViewModels.MapContainer
{
    internal struct RectangularEnvelope
    {
        public RectangularEnvelope(double xMinimum, double xMaximum, double yMinimum, double yMaximum)
        {
            XMinimum = xMinimum;
            XMaximum = xMaximum;
            YMinimum = yMinimum;
            YMaximum = yMaximum;
        }

        public double XMaximum { get; set; }

        public double XMinimum { get; set; }

        public double YMaximum { get; set; }

        public double YMinimum { get; set; }
    }
}
