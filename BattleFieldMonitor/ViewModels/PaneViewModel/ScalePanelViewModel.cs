using System.Windows.Input;
using DevExpress.Mvvm;
using Swsu.BattleFieldMonitor.ViewModelInterfaces;

namespace Swsu.BattleFieldMonitor.ViewModels.PaneViewModel
{
    internal class ScalePanelViewModel : ChildViewModelBase<IScalePanelViewModel, IScalePanelViewModelParent>, IScalePanelViewModel
    {

        #region Fields
        private double _scaleDenominator;
        #endregion

        #region Properties

        /// <summary>
        /// Масштаб
        ///  </summary>
        public double ScaleDenominator
        {
            get { return _scaleDenominator; }
            set { SetProperty(ref _scaleDenominator, value, nameof(ScaleDenominator)); }
        }

        #endregion

        #region Commands
        public ICommand MapScaleInCommand { get; }
        public ICommand MapScaleOutCommand { get; }

        #endregion

        #region Constructors
        public ScalePanelViewModel()
        {
            MapScaleInCommand = new DelegateCommand(MapScaleIn);
            MapScaleOutCommand = new DelegateCommand(MapScaleOut);
        }

        #endregion

        #region Methods

        private void MapScaleOut()
        {
            Parent.MapContainer.ScaleDenominator /= 0.75;
        }

        private void MapScaleIn()
        {
            Parent.MapContainer.ScaleDenominator *= 0.75;
        }


        #endregion

    }
}