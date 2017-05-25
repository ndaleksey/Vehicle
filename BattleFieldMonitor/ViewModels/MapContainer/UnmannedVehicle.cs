using System.Globalization;

namespace Swsu.BattleFieldMonitor.ViewModels.MapContainer
{
    /// <summary>
    /// Беспилотный автомобиль
    /// </summary>
    public class UnmannedVehicle : MapObject
    {
        #region Fields

        private double _azimuth;
        private string _calloutText;
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
        /// Коллекция координат трассы объекта
        /// </summary>
        //public ObservableCollection<Coord> Coords { get; }

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
        public UnmannedVehicle()
        {
            UpdateCalloutText();
        }

        /// <summary>
        /// Инициализирует экземпляр класса
        /// </summary>
        /// <param name="latitude">Широта</param>
        /// <param name="longitude">Долдгота</param>
        /// <param name="azimuth">Азимут</param>
        public UnmannedVehicle(double latitude, double longitude, double azimuth) : this()
        {
            Azimuth = azimuth;
            Latitude = latitude;
            Longitude = longitude;

            //Coords = new ObservableCollection<Coord>();
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

        #endregion
    }
}