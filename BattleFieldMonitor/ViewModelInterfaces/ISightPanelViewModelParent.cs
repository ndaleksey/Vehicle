namespace Swsu.BattleFieldMonitor.ViewModelInterfaces
{
    internal interface ISightPanelViewModelParent : IViewModelParent<ISightPanelViewModel>
    {
        #region Properties
        IMapContainerViewModel MapContainer { get; }

        #endregion
    }
}