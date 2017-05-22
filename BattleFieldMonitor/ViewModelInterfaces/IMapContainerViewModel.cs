namespace Swsu.BattleFieldMonitor.ViewModelInterfaces
{
    /// <summary>
    /// Используется моделями представлений, содержащих MapContainer.
    /// Реализуется моделью представления MapContainer.
    /// </summary>
    internal interface IMapContainerViewModel
    {
        #region Properties
        double ScaleDenominator
        {
            get;
            set;
        }
        #endregion
    }
}
