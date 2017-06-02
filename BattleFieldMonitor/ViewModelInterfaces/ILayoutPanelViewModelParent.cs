namespace Swsu.BattleFieldMonitor.ViewModelInterfaces
{
    internal interface ILayoutPanelViewModelParent : IViewModelParent<ILayoutPanelViewModel>
    {
        #region Properties
        IMapContainerViewModel MapContainer { get; }
        #endregion

    }
}