namespace Swsu.BattleFieldMonitor.ViewModelInterfaces
{
    internal interface IManualRoutePanelViewModelParent : IViewModelParent<IManualRoutePanelViewModel>
    {
        #region Properties

        IMapContainerViewModel MapContainer { get; }

        #endregion
    }
}