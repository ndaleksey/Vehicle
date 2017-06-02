namespace Swsu.BattleFieldMonitor.ViewModelInterfaces
{
    internal interface IBindingPanelViewModelParent : IViewModelParent<IBindingPanelViewModel>
    {
        #region Properties

        IMapContainerViewModel MapContainer { get; }

        #endregion

    }
}