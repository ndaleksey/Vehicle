namespace Swsu.BattleFieldMonitor.ViewModelInterfaces
{
    internal interface IViewModelParent<in TViewModel>
        where TViewModel : class
    {
        #region Methods
        void Register(TViewModel viewModel);

        void Unregister(TViewModel viewModel);
        #endregion
    }
}
