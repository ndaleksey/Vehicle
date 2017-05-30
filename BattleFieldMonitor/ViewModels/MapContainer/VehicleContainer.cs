using System.Windows;
using Swsu.Maps.Windows;
using Swsu.Maps.Windows.Infrastructure;

namespace Swsu.BattleFieldMonitor.ViewModels.MapContainer
{
    internal class VehicleContainer : FrameworkContentElement, IContainer
    {
        #region Dependency Properties

        public readonly static DependencyProperty LatitudeProperty = DependencyProperty.Register(
            nameof(Latitude),
            typeof(double),
            typeof(VehicleContainer),
            new FrameworkPropertyMetadata(
                double.NaN,
                CallbackHelpers.Create<VehicleContainer, double>(o => o.OnLatitudeChanged)));

        public readonly static DependencyProperty LongitudeProperty = DependencyProperty.Register(
            nameof(Longitude),
            typeof(double),
            typeof(VehicleContainer),
            new FrameworkPropertyMetadata(
                double.NaN,
                CallbackHelpers.Create<VehicleContainer, double>(o => o.OnLongitudeChanged)));

        public readonly static DependencyProperty AzimuthProperty = DependencyProperty.Register(
            nameof(Azimuth),
            typeof(double),
            typeof(VehicleContainer),
            new FrameworkPropertyMetadata(
                0.0,
                CallbackHelpers.Create<VehicleContainer, double>(o => o.OnAzimuthChanged)));

        #endregion

        #region Properties

        /// <summary>
        /// Широта
        /// </summary>
        public double Latitude
        {
            get { return (double) GetValue(LatitudeProperty); }
            set { SetValue(LatitudeProperty, value); }
        }

        /// <summary>
        /// Долгота
        /// </summary>
        public double Longitude
        {
            get { return (double) GetValue(LongitudeProperty); }
            set { SetValue(LongitudeProperty, value); }
        }

        /// <summary>
        /// Азимут
        /// </summary>
        public double Azimuth
        {
            get { return (double)GetValue(AzimuthProperty); }
            set { SetValue(AzimuthProperty, value); }
        }

        #endregion

        #region Methods

        private void OnLatitudeChanged(double oldValue, double newValue)
        {

        }

        private void OnLongitudeChanged(double oldValue, double newValue)
        {

        }

        private void OnAzimuthChanged(double oldValue, double newValue)
        {

        }

        #endregion
    }
}
