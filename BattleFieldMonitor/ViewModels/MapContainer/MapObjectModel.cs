using DevExpress.Mvvm;

namespace Swsu.BattleFieldMonitor.ViewModels.MapContainer
{
    /// <summary>
    /// Объект, отображаемый на карте
    /// </summary>
    public class MapObjectModel : BindableBase
    {
        #region Fields

        private string _displayName;
        private double _latitude;
        private double _longitude;

        #endregion

        #region Properties

        /// <summary>
        /// Отображаемое имя
        /// </summary>
        public string DisplayName
        {
            get { return _displayName; }
            set { SetProperty(ref _displayName, value, nameof(DisplayName)); }
        }

        /// <summary>
        /// Широта
        /// </summary>
        public double Latitude
        {
            get { return _latitude; }
            set { SetProperty(ref _latitude, value, nameof(Latitude), OnLatitudeChanged); }
        }

        /// <summary>
        /// Долгота
        /// </summary>
        public double Longitude
        {
            get { return _longitude; }
            set { SetProperty(ref _longitude, value, nameof(Longitude), OnLongitudeChanged); }
        }

        #endregion

        #region Constructors 

        /// <summary>
        /// Инициализирует экземпляр класса
        /// </summary>
        public MapObjectModel()
        {

        }

        /// <summary>
        /// Инициализирует экземпляр класса
        /// </summary>
        /// <param name="latitude">Широта</param>
        /// <param name="longitude">Долгота</param>
        public MapObjectModel(double latitude, double longitude) : this()
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="latitude">Широта</param>
        /// <param name="longitude">Долгота</param>
        /// <param name="displayName">Отображаемое имя</param>
        public MapObjectModel(double latitude, double longitude, string displayName) : this(latitude, longitude)
        {
            DisplayName = displayName;
        }

        #endregion

        #region Methods

        protected virtual void OnLatitudeChanged() { }

        protected virtual void OnLongitudeChanged() { }

        #endregion
    }
}
