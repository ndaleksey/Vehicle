using DevExpress.Mvvm;
using Swsu.BattleFieldMonitor.ViewModelInterfaces;
using Swsu.Common.Windows.DevExpress;
using System.Diagnostics;
using System.Windows.Input;

namespace Swsu.BattleFieldMonitor.ViewModels.Main
{
	internal class ViewModel : ViewModelBaseExtended,
		IMapContainerViewModelParent,
		IMeasurementsPanelViewModelParent,
		ILayoutPanelViewModelParent,
		IManualRoutePanelViewModelParent,
		AddGraphicSymbolsPanelViewModelParent,
		IBindingPanelViewModelParent,
		IMapPanelViewModelParent
	{
		#region Constants

		private const double Scale = 0.75;

		#endregion

		#region Fields


		private double _scaleDenominator;
		private IMapContainerViewModel _mapContainer;
		private IMeasurementsPanelViewModel _measurementsPanel;
		private ILayoutPanelViewModel _layoutPanel;
		private IManualRoutePanelViewModel _manualRoutePanel;
		private IAddGraphicSymbolsPanelViewModel _addGraphicSymbolsPanel;
		private IBindingPanelViewModel _bindingPanel;
		private IMapPanelViewModel _mapPanel;

		private bool _isPanelVisible;
		#endregion

		#region Properties

		public bool IsPanelVisible
		{
			get { return _isPanelVisible;}
			set { SetProperty(ref _isPanelVisible, value, nameof(IsPanelVisible)); }
		}

		public double ScaleDenominator
		{
			get { return _scaleDenominator; }
			set { SetProperty(ref _scaleDenominator, value, nameof(ScaleDenominator), OnScaleDenominatorChanged); }
		}
        
		#endregion

		#region Commands

		public ICommand MapScaleInCommand { get; }
		public ICommand MapScaleOutCommand { get; }

		public ICommand MeasurementPressedCommand { get; }
		public ICommand AddBadgeCommand { get; }
		#endregion

		#region Constructors

		public ViewModel()
		{
			MapScaleInCommand = new DelegateCommand(MapScaleIn, CanMapScaleIn);
			MapScaleOutCommand = new DelegateCommand(MapScaleOut, CanMapScaleOut);
			MeasurementPressedCommand = new DelegateCommand(MeasurementPressed);
			AddBadgeCommand = new DelegateCommand(AddBadge);
		}
		
		private void MeasurementPressed()
		{
			IsPanelVisible = !IsPanelVisible;
		}

		private void AddBadge()
		{
            IsPanelVisible = !IsPanelVisible;
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

		IMapContainerViewModel IMeasurementsPanelViewModelParent.MapContainer => _mapContainer;
		IMapContainerViewModel ILayoutPanelViewModelParent.MapContainer => _mapContainer;
		IMapContainerViewModel IManualRoutePanelViewModelParent.MapContainer => _mapContainer;
		IMapContainerViewModel AddGraphicSymbolsPanelViewModelParent.MapContainer => _mapContainer;
		IMapContainerViewModel IBindingPanelViewModelParent.MapContainer => _mapContainer;
		IMapContainerViewModel IMapPanelViewModelParent.MapContainer => _mapContainer;

		#endregion

		#region Methods

		private void OnIsMiniMapEnabledChanged(bool oldValue, bool newValue)
		{
			if (_mapContainer != null)
			{
				_mapContainer?.SetMiniMap(newValue);
			}
		}

		private void OnIsCenteringModeEnabledChanged(bool oldValue, bool newValue)
		{
			if (_mapContainer != null)
			{
				_mapContainer.SetCenteringMode(newValue);
			}
		}

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


		void IViewModelParent<IMeasurementsPanelViewModel>.Register(IMeasurementsPanelViewModel viewModel)
		{
			Debug.Assert(_measurementsPanel == null);
			_measurementsPanel = viewModel;
		}

		void IViewModelParent<IMeasurementsPanelViewModel>.Unregister(IMeasurementsPanelViewModel viewModel)
		{
			Debug.Assert(_measurementsPanel == viewModel);
			_measurementsPanel = null;
		}


		void IViewModelParent<ILayoutPanelViewModel>.Register(ILayoutPanelViewModel viewModel)
		{
			Debug.Assert(_layoutPanel == null);
			_layoutPanel = viewModel;
		}

		void IViewModelParent<ILayoutPanelViewModel>.Unregister(ILayoutPanelViewModel viewModel)
		{
			Debug.Assert(_layoutPanel == viewModel);
			_layoutPanel = null;
		}

		void IViewModelParent<IManualRoutePanelViewModel>.Register(IManualRoutePanelViewModel viewModel)
		{
			Debug.Assert(_manualRoutePanel == null);
			_manualRoutePanel = viewModel;
		}

		void IViewModelParent<IManualRoutePanelViewModel>.Unregister(IManualRoutePanelViewModel viewModel)
		{
			Debug.Assert(_manualRoutePanel == viewModel);
			_manualRoutePanel = null;
		}


		void IViewModelParent<IAddGraphicSymbolsPanelViewModel>.Register(IAddGraphicSymbolsPanelViewModel viewModel)
		{
			Debug.Assert(_addGraphicSymbolsPanel == null);
			_addGraphicSymbolsPanel = viewModel;
		}

		void IViewModelParent<IAddGraphicSymbolsPanelViewModel>.Unregister(IAddGraphicSymbolsPanelViewModel viewModel)
		{
			Debug.Assert(_addGraphicSymbolsPanel == viewModel);
			_addGraphicSymbolsPanel = null;
		}

		void IViewModelParent<IBindingPanelViewModel>.Register(IBindingPanelViewModel viewModel)
		{
			Debug.Assert(_bindingPanel == null);
			_bindingPanel = viewModel;
		}

		void IViewModelParent<IBindingPanelViewModel>.Unregister(IBindingPanelViewModel viewModel)
		{
			Debug.Assert(_bindingPanel == viewModel);
			_bindingPanel = null;
		}


		void IViewModelParent<IMapPanelViewModel>.Register(IMapPanelViewModel viewModel)
		{
			Debug.Assert(_mapPanel == null);
			_mapPanel = viewModel;
		}

		void IViewModelParent<IMapPanelViewModel>.Unregister(IMapPanelViewModel viewModel)
		{
			Debug.Assert(_mapPanel == viewModel);
			_mapPanel = null;
		}
        
		#endregion
	}
}