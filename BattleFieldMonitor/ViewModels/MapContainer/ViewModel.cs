using Swsu.BattleFieldMonitor.ViewModelInterfaces;

namespace Swsu.BattleFieldMonitor.ViewModels.MapContainer
{
    internal class ViewModel : ChildViewModelBase<IMapContainerViewModel, IMapContainerViewModelParent>,
        IMapContainerViewModel
    {
        #region Fields
        private MapToolMode _mapToolMode;

        private double _scaleDenominator;
        #endregion

        #region Constructors
        public ViewModel()
        {
            MapToolMode = MapToolMode.Pan;
        }
        #endregion

        #region Properties
        public MapToolMode MapToolMode
        {
            get { return _mapToolMode; }
            private set { SetProperty(ref _mapToolMode, value, nameof(MapToolMode)); }
        }

        public double ScaleDenominator
        {
            get { return _scaleDenominator; }
            set { SetProperty(ref _scaleDenominator, value, nameof(ScaleDenominator), OnScaleDenominatorChanged); }
        }
        #endregion

        #region Methods
        private void OnScaleDenominatorChanged(double oldValue, double newValue)
        {
            Parent?.NotifyScaleDenominatorChanged(oldValue, newValue);
        }
        #endregion
    }
}