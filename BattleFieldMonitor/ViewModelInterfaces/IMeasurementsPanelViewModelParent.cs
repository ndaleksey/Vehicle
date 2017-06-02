namespace Swsu.BattleFieldMonitor.ViewModelInterfaces
{
    internal interface IMeasurementsPanelViewModelParent : IViewModelParent<IMeasurementsPanelViewModel>
    {
        #region Properties

        IMapContainerViewModel MapContainer { get; }

        #endregion
    }
}
