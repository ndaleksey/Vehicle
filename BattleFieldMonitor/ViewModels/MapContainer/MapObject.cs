using DevExpress.Mvvm;

namespace Swsu.BattleFieldMonitor.ViewModels.MapContainer
{
    /// <summary>
    /// ������, ������������ �� �����
    /// </summary>
    public class MapObject : BindableBase
    {
        #region Fields

        private double _latitude;
        private double _longitude;

        #endregion

        #region Properties

        /// <summary>
        /// ������
        /// </summary>
        public double Latitude
        {
            get { return _latitude; }
            set { SetProperty(ref _latitude, value, nameof(Latitude)); }
        }

        /// <summary>
        /// �������
        /// </summary>
        public double Longitude
        {
            get { return _longitude; }
            set { SetProperty(ref _longitude, value, nameof(Longitude)); }
        }

        #endregion

        #region Constructors 

        /// <summary>
        /// �������������� ��������� ������
        /// </summary>
        public MapObject()
        {
            
        }

        /// <summary>
        /// �������������� ��������� ������
        /// </summary>
        /// <param name="latitude">������</param>
        /// <param name="longitude">��������</param>
        /// <param name="azimuth">������</param>
        public MapObject(double latitude, double longitude) : this()
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        #endregion
    }
}