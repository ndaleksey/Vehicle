namespace Swsu.BattleFieldMonitor.ViewModelInterfaces
{
    internal interface IRouteAutomatPanelViewModelParent : IViewModelParent<IRouteAutomatPanelViewModel>
    {
        #region Properties

        IMapContainerViewModel MapContainer { get; }

        #endregion

    }
}