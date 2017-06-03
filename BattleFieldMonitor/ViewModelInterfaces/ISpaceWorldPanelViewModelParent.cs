namespace Swsu.BattleFieldMonitor.ViewModelInterfaces
{
    internal interface ISpaceWorldPanelViewModelParent : IViewModelParent<ISpaceWorldPanelViewModel>
    {
        #region Properties
        IMapContainerViewModel MapContainer { get; }

        #endregion

    }
}