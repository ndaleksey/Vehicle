using System.Collections.ObjectModel;
using System.Globalization;
using DevExpress.Mvvm;

namespace Swsu.BattleFieldMonitor.Models
{
    /// <summary>
    /// Объект, отображаемый на карте //TODO: Скорее всего, это будет танк
    /// </summary>
    public class MapObject : BindableBase
    {
        #region Fields

        private double _azimuth;
        private string _calloutText;
        private double _latitude;
        private double _longitude;
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
        public ObservableCollection<Coord> Coords { get; }

        /// <summary>
        /// Широта
        /// </summary>
        public double Latitude
        {
            get { return _latitude; }
            set { SetProperty(ref _latitude, value, nameof(Latitude), UpdateCalloutText); }
        }

        /// <summary>
        /// Долгота
        /// </summary>
        public double Longitude
        {
            get { return _longitude; }
            set { SetProperty(ref _longitude, value, nameof(Longitude), UpdateCalloutText); }
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
        public MapObject()
        {
            UpdateCalloutText();
        }

        /// <summary>
        /// Инициализирует экземпляр класса
        /// </summary>
        /// <param name="latitude">Широта</param>
        /// <param name="longitude">Долдгота</param>
        /// <param name="azimuth">Азимут</param>
        public MapObject(double latitude, double longitude, double azimuth) : this()
        {
            Azimuth = azimuth;
            Latitude = latitude;
            Longitude = longitude;

            //TODO: Убрать лишнее
            Coords = new ObservableCollection<Coord>()
            {
                new Coord(0.123456789, 55.123456789),
                new Coord(-10, 51.0),
                new Coord(-20, 50.0)
            };
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
