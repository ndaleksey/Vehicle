using System.Globalization;
using Swsu.BattleFieldMonitor.Common;

namespace Swsu.BattleFieldMonitor.ViewModels.MapContainer
{
    /// <summary>
    /// Беспилотный автомобиль
    /// </summary>
    public class UnmannedVehicleModel : MapObjectModel
    {
        #region Fields

        private double _azimuth;
        private string _calloutText;
        private bool _isTracked;
        private ViewModel _parentViewModel;
        private double _speed;

        #endregion

        #region Properties

        /// <summary>
        /// Азимут
        /// </summary>
        public double Azimuth
        {
            get { return _azimuth; }
            set { SetProperty(ref _azimuth, value, nameof(Azimuth)); }
        }

        /// <summary>
        /// Текст из четырёх строк, отображаемый на выноске
        /// </summary>
        public string CalloutText
        {
            get { return _calloutText; }
            set { SetProperty(ref _calloutText, value, nameof(CalloutText)); }
        }

        /// <summary>
        /// Признак, указывающий, что за объектом необходимо следить
        /// </summary>
        internal bool IsTracked
        {
            get { return _isTracked; }
            set { SetProperty(ref _isTracked, value, nameof(IsTracked), UpdateTracking); }
        }

        /// <summary>
        /// Родительская ViewModel для доступа к свойствам отслеживания
        /// </summary>
        internal ViewModel ParentViewModel
        {
            get { return _parentViewModel; }
            set { SetProperty(ref _parentViewModel, value, nameof(ParentViewModel), UpdateTracking); }
        }

        /// <summary>
        /// Скорость, км/ч
        /// </summary>
        public double Speed
        {
            get { return _speed; }
            set { SetProperty(ref _speed, value, nameof(Speed), UpdateCalloutText); }
        }

        #endregion

        #region Constructors 

        /// <summary>
        /// Инициализирует экземпляр класса
        /// </summary>
        internal UnmannedVehicleModel(ViewModel parentViewModel)
        {
            ParentViewModel = parentViewModel;
            //UpdateCalloutText();
        }

        /// <summary>
        /// Инициализирует экземпляр класса
        /// </summary>
        /// <param name="parentViewModel">Родительская ViewModel для доступа к функциям слежения</param>
        /// <param name="latitude">Широта</param>
        /// <param name="longitude">Долдгота</param>
        /// <param name="azimuth">Азимут</param>
        internal UnmannedVehicleModel(ViewModel parentViewModel, double latitude, double longitude, double azimuth) : this(parentViewModel)
        {
            Azimuth = azimuth;
            Latitude = latitude;
            Longitude = longitude;
        }

        #endregion

        #region Methods

        private void UpdateCalloutText()
        {
            CalloutText = $"Азимут: {Azimuth.ToString("0.00", CultureInfo.InvariantCulture)}°\n" +
                          $"Широта: {Latitude.ToString("0.000000", CultureInfo.InvariantCulture)}°\n" +
                          $"Долгота: {Longitude.ToString("0.000000", CultureInfo.InvariantCulture)}°\n" +
                          $"Скорость: {Speed.ToString("0.0", CultureInfo.InvariantCulture)} км/ч";
        }

        protected override void OnLatitudeChanged()
        {
            UpdateTracking();
        }

        protected override void OnLongitudeChanged()
        {
            UpdateTracking();
        }

        private void UpdateTracking()
        {
            if (ParentViewModel != null && ParentViewModel.IsCenteringModeEnabled)
            {
                if (IsTracked)
                {
                    ParentViewModel.MapViewerService.Locate(new GeographicCoordinatesTuple(Latitude, Longitude));
                }
            }
        }

        #endregion
    }
}
