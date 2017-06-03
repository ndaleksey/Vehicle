namespace Swsu.BattleFieldMonitor.ViewModelInterfaces
{
    internal interface IMoveMapPanelViewModeParent : IViewModelParent<IMoveMapPanelViewModel>
    {
        #region Properties

        IMapContainerViewModel MapContainer { get; }

        #endregion
    }
}