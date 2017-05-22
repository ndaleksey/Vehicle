using DevExpress.Mvvm;
using Swsu.BattleFieldMonitor.ViewModelInterfaces;
using Swsu.Common.Windows.DevExpress;
using System.Diagnostics;
using System.Windows.Input;

namespace Swsu.BattleFieldMonitor.ViewModels.Main
{
    internal class ViewModel : ViewModelBaseExtended,
        IMapContainerViewModelParent
    {
        #region Constants

        private const double Scale = 0.75;
        #endregion

        #region Fields
        private double _scaleDenominator;
        private IMapContainerViewModel _mapContainer;
        #endregion

        #region Properties
        public double ScaleDenominator
        {
            get { return _scaleDenominator; }
            set { SetProperty(ref _scaleDenominator, value, nameof(ScaleDenominator), OnScaleDenominatorChanged); }
        }        
        #endregion

        #region Commands
        public ICommand MapScaleInCommand { get; }
        public ICommand MapScaleOutCommand { get; }
        #endregion

        #region Constructors

        public ViewModel()
        {
            MapScaleInCommand = new DelegateCommand(MapScaleIn, CanMapScaleIn);
            MapScaleOutCommand = new DelegateCommand(MapScaleOut, CanMapScaleOut);

            //			ScaleDenominator = 1e7;
        }

        #endregion

        #region Commands' methods
        private bool CanMapScaleIn()
        {
            return true;
        }

        private void MapScaleIn()
        {
            //			ScaleDenominator *= Scale;
        }

        private bool CanMapScaleOut()
        {
            return true;
        }

        private void MapScaleOut()
        {
            //			ScaleDenominator /= Scale;
        }
        #endregion

        #region Methods
        private void OnScaleDenominatorChanged(double oldValue, double newValue)
        {
            if (_mapContainer != null)
            {
                _mapContainer.ScaleDenominator = newValue;
            }
        }

        void IMapContainerViewModelParent.NotifyScaleDenominatorChanged(double oldValue, double newValue)
        {
            ScaleDenominator = newValue;
        }

        void IViewModelParent<IMapContainerViewModel>.Register(IMapContainerViewModel viewModel)
        {
            Debug.Assert(_mapContainer == null);
            _mapContainer = viewModel;
        }

        void IViewModelParent<IMapContainerViewModel>.Unregister(IMapContainerViewModel viewModel)
        {
            Debug.Assert(_mapContainer == viewModel);
            _mapContainer = null;
        }
        #endregion
    }
}