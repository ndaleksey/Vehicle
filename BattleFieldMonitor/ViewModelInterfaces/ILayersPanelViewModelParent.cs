namespace Swsu.BattleFieldMonitor.ViewModelInterfaces
{
    internal interface ILayersPanelViewModelParent : IViewModelParent<ILayersPanelViewModel>
    {
        #region Properties
        IMapContainerViewModel MapContainer { get; }

        #endregion
    }
}