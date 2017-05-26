using System.Globalization;

namespace Swsu.BattleFieldMonitor.ViewModels.MapContainer
{
    /// <summary>
    /// ����������� ����������
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
        /// ������
        /// </summary>
        public double Azimuth
        {
            get { return _azimuth; }
            set { SetProperty(ref _azimuth, value, nameof(Azimuth)); }
        }

        /// <summary>
        /// ����� �� ������ �����, ������������ �� �������
        /// </summary>
        public string CalloutText
        {
            get { return _calloutText; }
            set { SetProperty(ref _calloutText, value, nameof(CalloutText)); }
        }

        /// <summary>
        /// ��������� ��������� ������ �������
        /// </summary>
        //public ObservableCollection<Coord> Coords { get; }

        /// <summary>
        /// ��������, ��/�
        /// </summary>
        public double Speed
        {
            get { return _speed; }
            set { SetProperty(ref _speed, value, nameof(Speed), UpdateCalloutText); }
        }

        #endregion

        #region Constructors 

        /// <summary>
        /// �������������� ��������� ������
        /// </summary>
        public UnmannedVehicle()
        {
            UpdateCalloutText();
        }

        /// <summary>
        /// �������������� ��������� ������
        /// </summary>
        /// <param name="latitude">������</param>
        /// <param name="longitude">��������</param>
        /// <param name="azimuth">������</param>
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
            CalloutText = $"������: {Azimuth.ToString("0.00", CultureInfo.InvariantCulture)}�\n" +
                          $"������: {Latitude.ToString("0.000000", CultureInfo.InvariantCulture)}�\n" +
                          $"�������: {Longitude.ToString("0.000000", CultureInfo.InvariantCulture)}�\n" +
                          $"��������: {Speed.ToString("0.0", CultureInfo.InvariantCulture)} ��/�";
        }

        #endregion
    }
}