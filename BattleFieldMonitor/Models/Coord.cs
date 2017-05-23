using DevExpress.Mvvm;

namespace Swsu.BattleFieldMonitor.Models
{
    /// <summary>
    /// Точка, задаваемая широтой и долготой
    /// </summary>
    public class Coord : BindableBase
    {
        #region Fields

        private double _latitude;
        private double _longitude;

        #endregion

        #region Properties

        /// <summary>
        /// Широта
        /// </summary>
        public double Latitude
        {
            get { return _latitude; }
            set { SetProperty(ref _latitude, value, nameof(Latitude)); }
        }

        /// <summary>
        /// Долгота
        /// </summary>
        public double Longitude
        {
            get { return _longitude; }
            set { SetProperty(ref _longitude, value, nameof(Longitude)); }
        }

        #endregion

        #region Constructors

        public Coord()
        {

        }

        public Coord(double latitude, double longitude) : this()
        {
            Latitude = latitude;
            Longitude = longitude;
        }
        #endregion
    }
}
