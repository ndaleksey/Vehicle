namespace Swsu.BattleFieldMonitor.ViewModelInterfaces
{
    /// <summary>
    /// Используется моделью представления MapContainer.
    /// Реализуется моделями представлений, содержащих MapContainer.
    /// </summary>
    internal interface IMapContainerViewModelParent : IViewModelParent<IMapContainerViewModel>
    {
        #region Methods
        void NotifyScaleDenominatorChanged(double oldValue, double newValue);
        #endregion
    }
}
