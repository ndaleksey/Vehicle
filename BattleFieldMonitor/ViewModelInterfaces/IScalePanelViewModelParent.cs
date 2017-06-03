namespace Swsu.BattleFieldMonitor.ViewModelInterfaces
{
    internal interface IScalePanelViewModelParent : IViewModelParent<IScalePanelViewModel>, IViewModelParent<IMapContainerViewModel>
    {
        #region Properties

        IMapContainerViewModel MapContainer { get; }

        #endregion

    }
}