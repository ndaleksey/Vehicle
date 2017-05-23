using System.Collections.ObjectModel;
using Swsu.BattleFieldMonitor.Models;
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

        #region Properties

        /// <summary>
        /// Коллекция танков, отображаемых на карте
        /// </summary>
	    public ObservableCollection<MapObject> MapObjects { get; }

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

        #region Constructors

        public ViewModel()
        {
            MapObjects = new ObservableCollection<MapObject>
            {
                new MapObject(0.123456789, 55.123456789, 45.123456789)
            };

            MapToolMode = MapToolMode.Pan;
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