namespace Swsu.BattleFieldMonitor.ViewModelInterfaces
{
    internal interface AddGraphicSymbolsPanelViewModelParent : IViewModelParent<IAddGraphicSymbolsPanelViewModel>
    {
        #region Properties

        IMapContainerViewModel MapContainer { get; }

        #endregion

    }
}