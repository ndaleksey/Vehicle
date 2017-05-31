using System.Globalization;
using Swsu.BattleFieldMonitor.Common;
using Swsu.BattleFieldMonitor.Services;

namespace Swsu.BattleFieldMonitor.ViewModels.MapContainer
{
    /// <summary>
    /// Беспилотный автомобиль
    /// </summary>
    public class UnmannedVehicleModel : MapObjectModel
    {
        #region Fields
        //TODO: Добавить имя
        private double _azimuth;
        private string _calloutText;
        private bool _isTracked;
        private ViewModel _parentViewModel;
        private bool _showCallout;
        private double _speed;
        private IUnmannedVehicle _source;

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
        /// Признак, определяющий, показывать ли выноску
        /// </summary>
        public bool ShowCallout
        {
            get { return _showCallout; }
            set { SetProperty(ref _showCallout, value, nameof(ShowCallout)); }
        }

        /// <summary>
        /// Скорость, м/с
        /// </summary>
        public double Speed
        {
            get { return _speed; }
            set { SetProperty(ref _speed, value, nameof(Speed), UpdateCalloutText); }
        }

        /// <summary>
        /// РТК в базе данных
        /// </summary>
        internal IUnmannedVehicle Source
        {
            get { return _source; }
            set { SetProperty(ref _source, value, nameof(Source)); }
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
        /// <param name="source">РТК в базе данных, откуда считываются все параметры этой модели</param>
        internal UnmannedVehicleModel(ViewModel parentViewModel, IUnmannedVehicle source) : this(parentViewModel)
        {
            Source = source;
            UpdateFromSource();

            //TODO: Использовать слабые событие
            Source.Changed += Vehicle_Changed;
        }

        private void Vehicle_Changed(object sender, System.EventArgs e)
        {
            UpdateFromSource();
        }

        #endregion

        #region Methods

        private void UpdateCalloutText()
        {
            CalloutText = $"Азимут: {Azimuth.ToString("0.00", CultureInfo.InvariantCulture)}°\n" +
                          $"Широта: {Latitude.ToString("0.000000", CultureInfo.InvariantCulture)}°\n" +
                          $"Долгота: {Longitude.ToString("0.000000", CultureInfo.InvariantCulture)}°\n" +
                          $"Скорость: {(Speed * 3.6).ToString("0.0", CultureInfo.InvariantCulture)} км/ч";
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

        /// <summary>
        /// Обновляет параметры модели РТК при изменениях в базе
        /// </summary>
        private void UpdateFromSource()
        {
            Azimuth = Source.Heading;
            Latitude = Source.Location.Latitude;
            Longitude = Source.Location.Longitude;
        }

        #endregion
    }
}
