namespace Swsu.BattleFieldMonitor.ViewModelInterfaces
{
    internal interface INavigationPanelViewModelParent : IViewModelParent<INavigationPanelViewModel>
    {
        #region Properties
        IMapContainerViewModel MapContainer { get; }

        #endregion

    }
}