namespace Swsu.BattleFieldMonitor.ViewModels.MapContainer
{
    /// <summary>
    /// ���� (����� ��������)
    /// </summary>
    public class Beacon : MapObject
    {
        #region Constructors 

        /// <summary>
        /// �������������� ��������� ������
        /// </summary>
        public Beacon()
        {

        }

        /// <summary>
        /// �������������� ��������� ������
        /// </summary>
        /// <param name="latitude">������</param>
        /// <param name="longitude">�������</param>
        public Beacon(double latitude, double longitude) : base(latitude, longitude)
        {
        }

        #endregion
    }
}