namespace Swsu.BattleFieldMonitor.ViewModels.MapContainer
{
    /// <summary>
    /// Маяк (точка возврата)
    /// </summary>
    public class Beacon : MapObject
    {
        #region Constructors 

        /// <summary>
        /// Инициализирует экземпляр класса
        /// </summary>
        public Beacon()
        {

        }

        /// <summary>
        /// Инициализирует экземпляр класса
        /// </summary>
        /// <param name="latitude">Широта</param>
        /// <param name="longitude">Долгота</param>
        public Beacon(double latitude, double longitude) : base(latitude, longitude)
        {
        }

        #endregion
    }
}