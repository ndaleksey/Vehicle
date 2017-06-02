namespace Swsu.BattleFieldMonitor.ViewModelInterfaces
{
    internal interface IMapPanelViewModelParent : IViewModelParent<IMapPanelViewModel>
    {
        #region Properties

        IMapContainerViewModel MapContainer { get; }

        #endregion
    }
}