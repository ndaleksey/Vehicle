namespace Swsu.BattleFieldMonitor.ViewModelInterfaces
{
    internal interface IRouteClearPanelViewModelParent : IViewModelParent<IRouteClearPanelViewModel>
    {
        #region Properties

        IMapContainerViewModel MapContainer { get; }

        #endregion
    }
}