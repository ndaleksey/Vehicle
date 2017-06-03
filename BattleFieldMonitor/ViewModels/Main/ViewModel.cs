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
		IMapPanelViewModelParent,
        IScalePanelViewModelParent,
        IRouteAutomatPanelViewModelParent,
        IRouteClearPanelViewModelParent,
        IMoveMapPanelViewModeParent,
        ILayersPanelViewModelParent, 
        ISpaceWorldPanelViewModelParent,
        INavigationPanelViewModelParent,
        ISightPanelViewModelParent
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
		private IScalePanelViewModel _scalePanel;
		private IRouteAutomatPanelViewModel _routeAutomatPanel;
		private IRouteClearPanelViewModel _routeClearPanel;
		private IMoveMapPanelViewModel _moveMapPanel;
		private ILayersPanelViewModel _layersMapPanel;
		private ISpaceWorldPanelViewModel _spaceWorldPanel;
		private INavigationPanelViewModel _navigationPanel;
		private ISightPanelViewModel _sightPanel;

        private bool _switchMapButtonChecked;
        private bool _switchLayersButtonChecked;
        private bool _switchSpaceWorldButtonChecked;
        private bool _switchScaleButtonChecked;
        private bool _switchNavigationButtonChecked;
        private bool _switchSightButtonChecked;
        private bool _switchBindingButtonChecked;

        private bool _switchMeasurementsButtonChecked;
        private bool _switchRouteAutomaticButtonChecked;
        private bool _switchRouteManualButtonChecked;
        private bool _switchRouteClearButtonChecked;
        private bool _switchRouteLayoutButtonChecked;
        private bool _switchMoveMapButtonChecked;
        private bool _switchAddGraphicDesignationsButtonChecked;

        private bool _isPanelVisible;
        #endregion

        #region Properties

        public double ScaleDenominator
        {
            get { return _scaleDenominator; }
            set { SetProperty(ref _scaleDenominator, value, nameof(ScaleDenominator), OnScaleDenominatorChanged); }
        }


        public bool IsPanelVisible
		{
			get { return _isPanelVisible;}
			set { SetProperty(ref _isPanelVisible, value, nameof(IsPanelVisible)); }
		}


        /// <summary>
        /// Зажата кнопка "Карта"
        /// </summary>
        public bool IsCheckedMap
        {
            get { return _switchMapButtonChecked; }
            set { SetProperty(ref _switchMapButtonChecked, value, nameof(IsCheckedMap)); }
        }

        /// <summary>
        /// Зажата кнопка "Слои"
        /// </summary>
        public bool IsCheckedLayers
        {
            get { return _switchLayersButtonChecked; }
            set { SetProperty(ref _switchLayersButtonChecked, value, nameof(IsCheckedLayers)); }
        }

        /// <summary>
        /// Зажата кнопка "3D мир"
        /// </summary>
        public bool IsCheckedSpaceWorld
        {
            get { return _switchSpaceWorldButtonChecked; }
            set { SetProperty(ref _switchSpaceWorldButtonChecked, value, nameof(IsCheckedSpaceWorld)); }
        }

        /// <summary>
        /// Зажата кнопка "Масштаб"
        /// </summary>
        public bool IsCheckedScale
        {
            get { return _switchScaleButtonChecked; }
            set { SetProperty(ref _switchScaleButtonChecked, value, nameof(IsCheckedScale)); }
        }

        /// <summary>
        /// Зажата кнопка "Навигация"
        /// </summary>
        public bool IsCheckedNavigation
        {
            get { return _switchNavigationButtonChecked; }
            set { SetProperty(ref _switchNavigationButtonChecked, value, nameof(IsCheckedNavigation)); }
        }

        /// <summary>
        /// Зажата кнопка "Ракурс"
        /// </summary>
        public bool IsCheckedSight
        {
            get { return _switchSightButtonChecked; }
            set { SetProperty(ref _switchSightButtonChecked, value, nameof(IsCheckedSight)); }
        }

        /// <summary>
        /// Зажата кнопка "Привязка"
        /// </summary>
        public bool IsCheckedBinding
        {
            get { return _switchBindingButtonChecked; }
            set { SetProperty(ref _switchBindingButtonChecked, value, nameof(IsCheckedBinding)); }
        }

        /// <summary>
        /// Зажата кнопка "Измерения"
        /// </summary>
        public bool IsCheckedMeasurements
        {
            get { return _switchMeasurementsButtonChecked; }
            set {SetProperty(ref _switchMeasurementsButtonChecked,value,nameof(IsCheckedMeasurements));}
        }

	    /// <summary>
	    /// Зажата кнопка "Маршрут автомат"
	    /// </summary>
	    public bool IsCheckedRouteAutomatic
	    {
	        get { return _switchRouteAutomaticButtonChecked; }
	        set { SetProperty(ref _switchRouteAutomaticButtonChecked, value, nameof(IsCheckedRouteAutomatic)); }
	    }

	    /// <summary>
	    /// Зажата кнопка "Маршрут ручной"
	    /// </summary>
	    public bool IsCheckedRouteManual
	    {
	        get { return _switchRouteManualButtonChecked; }
	        set { SetProperty(ref _switchRouteManualButtonChecked, value, nameof(IsCheckedRouteManual)); }
	    }

	    /// <summary>
        /// Зажата кнопка "Маршрут очистить"
        /// </summary>
        public bool IsCheckedRouteClear
        {
            get { return _switchRouteClearButtonChecked; }
	        set { SetProperty(ref _switchRouteClearButtonChecked, value, nameof(IsCheckedRouteClear)); }
        }

        /// <summary>
        /// Зажата кнопка "Разметка"
        /// </summary>
        public bool IsCheckedRouteLayout
        {
            get { return _switchRouteLayoutButtonChecked; }
            set { SetProperty(ref _switchRouteLayoutButtonChecked, value, nameof(IsCheckedRouteLayout)); }
        }

        /// <summary>
        /// Зажата кнопка "Движение по карте"
        /// </summary>
        public bool IsCheckedMoveMap
        {
            get { return _switchMoveMapButtonChecked; }
            set { SetProperty(ref _switchMoveMapButtonChecked, value, nameof(IsCheckedMoveMap)); }
        }

        /// <summary>
        /// Зажата кнопка "Добавить УГО"
        /// </summary>
        public bool IsCheckedAddGraphicDesignations
        {
            get { return _switchAddGraphicDesignationsButtonChecked; }
            set {SetProperty(ref _switchAddGraphicDesignationsButtonChecked,value,nameof(IsCheckedAddGraphicDesignations));}
        }

        #endregion

        #region Commands

        public ICommand MapScaleInCommand { get; }
		public ICommand MapScaleOutCommand { get; }


        /// <summary>
        /// Комманда для открытия панели "Карта"
        /// </summary>
        public ICommand MapPressedCommand { get; }

        /// <summary>
        /// Комманда для открытия панели "Слои"
        /// </summary>
        public ICommand LayersPressedCommand { get; }

        /// <summary>
        /// Комманда для открытия панели "3D мир"
        /// </summary>
        public ICommand SpaceWorldPressedCommand { get; }

        /// <summary>
        /// Комманда для открытия панели "Масштаб"
        /// </summary>
        public ICommand ScalePressedCommand { get; }

        /// <summary>
        /// Комманда для открытия панели "Навигация"
        /// </summary>
        public ICommand NavigationPressedCommand { get; }

        /// <summary>
        /// Комманда для открытия панели "Ракурс"
        /// </summary>
        public ICommand SightPressedCommand { get; }

        /// <summary>
        /// Комманда для открытия панели "Привязка"
        /// </summary>
        public ICommand BindingPressedCommand { get; }



        /// <summary>
        /// Комманда для открытия панели "Измерения"
        /// </summary>
		public ICommand MeasurementPressedCommand { get; }

        /// <summary>
        /// Комманда для открытия панели "Маршрут автомат"
        /// </summary>
        public ICommand RouteAutomatPressedCommand { get; }

        /// <summary>
        /// Комманда для открытия панели "Маршрут ручной"
        /// </summary>
		public ICommand RouteManualPressedCommand { get; }

        /// <summary>
        /// Комманда для открытия панели "Маршрут очистить"
        /// </summary>
        public ICommand RouteClearPressedCommand { get; }

        /// <summary>
        /// Комманда для открытия панели "Разметка"
        /// </summary>
        public ICommand LayoutPressedCommand { get; }

        /// <summary>
        /// Комманда для открытия панели "Движение по карте"
        /// </summary>
        public ICommand MoveMapPressedCommand { get; }
        
        /// <summary>
        /// Комманда для открытия панели "Добавить УГО"
        /// </summary>
        public ICommand AddGraphicDesignationsPressedCommand { get; }


        #endregion

        #region Constructors

        public ViewModel()
		{

            MapPressedCommand = new DelegateCommand(MapPressed);
            LayersPressedCommand = new DelegateCommand(LayersPressed);
            SpaceWorldPressedCommand = new DelegateCommand(SpaceWorldPressed);
            ScalePressedCommand = new DelegateCommand(ScalePressed);
            NavigationPressedCommand = new DelegateCommand(NavigationPressed);
            NavigationPressedCommand = new DelegateCommand(NavigationPressed);
            SightPressedCommand = new DelegateCommand(SightPressed);
            BindingPressedCommand = new DelegateCommand(BindingPressed);

            MeasurementPressedCommand = new DelegateCommand(MeasurementPressed);
            RouteAutomatPressedCommand = new DelegateCommand(RouteAutomatPressed);
			RouteManualPressedCommand = new DelegateCommand(RouteManualPressed);
            RouteClearPressedCommand = new DelegateCommand(RouteClearPressed);
            LayoutPressedCommand = new DelegateCommand(LayoutPressed);
            MoveMapPressedCommand = new DelegateCommand(MoveMapPressedPressed);
            AddGraphicDesignationsPressedCommand = new DelegateCommand(AddGraphicDesignationsPressed);
		}

        #endregion


        #region Methods


        IMapContainerViewModel IMeasurementsPanelViewModelParent.MapContainer => _mapContainer;
        IMapContainerViewModel ILayoutPanelViewModelParent.MapContainer => _mapContainer;
        IMapContainerViewModel IManualRoutePanelViewModelParent.MapContainer => _mapContainer;
        IMapContainerViewModel AddGraphicSymbolsPanelViewModelParent.MapContainer => _mapContainer;
        IMapContainerViewModel IBindingPanelViewModelParent.MapContainer => _mapContainer;
        IMapContainerViewModel IMapPanelViewModelParent.MapContainer => _mapContainer;
        IMapContainerViewModel IScalePanelViewModelParent.MapContainer => _mapContainer;
        IMapContainerViewModel IRouteAutomatPanelViewModelParent.MapContainer => _mapContainer;
        IMapContainerViewModel IRouteClearPanelViewModelParent.MapContainer => _mapContainer;
        IMapContainerViewModel IMoveMapPanelViewModeParent.MapContainer => _mapContainer;
        IMapContainerViewModel ILayersPanelViewModelParent.MapContainer => _mapContainer;
        IMapContainerViewModel ISpaceWorldPanelViewModelParent.MapContainer => _mapContainer;
        IMapContainerViewModel INavigationPanelViewModelParent.MapContainer => _mapContainer;
        IMapContainerViewModel ISightPanelViewModelParent.MapContainer => _mapContainer;


        private void MeasurementPressed()
        {
            IsPanelVisible = !IsPanelVisible;

        }
        private void MoveMapPressedPressed()
        {
            IsPanelVisible = !IsPanelVisible;
        }

        private void RouteAutomatPressed()
        {
            IsPanelVisible = !IsPanelVisible;
        }

        private void RouteClearPressed()
        {
            IsPanelVisible = !IsPanelVisible;
        }

        private void LayoutPressed()
        {
            IsPanelVisible = !IsPanelVisible;
        }

        private void AddGraphicDesignationsPressed()
        {
            IsPanelVisible = !IsPanelVisible;
        }

        private void RouteManualPressed()
        {
            IsPanelVisible = !IsPanelVisible;
        }

        private void MapPressed()
        {
            IsPanelVisible = !IsPanelVisible;
        }

        private void LayersPressed()
        {
            IsPanelVisible = !IsPanelVisible;
        }

        private void SpaceWorldPressed()
        {
            IsPanelVisible = !IsPanelVisible;
        }

        private void ScalePressed()
        {
            IsPanelVisible = !IsPanelVisible;
        }

        private void NavigationPressed()
        {
            IsPanelVisible = !IsPanelVisible;
        }

        private void SightPressed()
        {
            IsPanelVisible = !IsPanelVisible;
        }

        private void BindingPressed()
        {
            IsPanelVisible = !IsPanelVisible;
        }

        /*        private void OnIsMiniMapEnabledChanged(bool oldValue, bool newValue)
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
                }*/

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

        void IViewModelParent<IScalePanelViewModel>.Register(IScalePanelViewModel viewModel)
        {
            Debug.Assert(_scalePanel == null);
            _scalePanel = viewModel;
        }

        void IViewModelParent<IScalePanelViewModel>.Unregister(IScalePanelViewModel viewModel)
        {
            Debug.Assert(_scalePanel == viewModel);
            _scalePanel = null;
        }

        void IViewModelParent<IRouteAutomatPanelViewModel>.Register(IRouteAutomatPanelViewModel viewModel)
        {
            Debug.Assert(_routeAutomatPanel == null);
            _routeAutomatPanel = viewModel;
        }

        void IViewModelParent<IRouteAutomatPanelViewModel>.Unregister(IRouteAutomatPanelViewModel viewModel)
        {
            Debug.Assert(_routeAutomatPanel == viewModel);
            _routeAutomatPanel = null;
        }

        void IViewModelParent<IRouteClearPanelViewModel>.Register(IRouteClearPanelViewModel viewModel)
        {
            Debug.Assert(_routeClearPanel == null);
            _routeClearPanel = viewModel;
        }

        void IViewModelParent<IRouteClearPanelViewModel>.Unregister(IRouteClearPanelViewModel viewModel)
        {
            Debug.Assert(_routeClearPanel == viewModel);
            _routeClearPanel = null;
        }

        void IViewModelParent<IMoveMapPanelViewModel>.Register(IMoveMapPanelViewModel viewModel)
        {
            Debug.Assert(_moveMapPanel == null);
            _moveMapPanel = viewModel;
        }

        void IViewModelParent<IMoveMapPanelViewModel>.Unregister(IMoveMapPanelViewModel viewModel)
        {
            Debug.Assert(_moveMapPanel == viewModel);
            _moveMapPanel = null;
        }

        void IViewModelParent<ILayersPanelViewModel>.Register(ILayersPanelViewModel viewModel)
        {
            Debug.Assert(_moveMapPanel == null);
            _layersMapPanel = viewModel;
        }

        void IViewModelParent<ILayersPanelViewModel>.Unregister(ILayersPanelViewModel viewModel)
        {
            Debug.Assert(_layersMapPanel == viewModel);
            _moveMapPanel = null;
        }

        void IViewModelParent<ISpaceWorldPanelViewModel>.Register(ISpaceWorldPanelViewModel viewModel)
        {
            Debug.Assert(_spaceWorldPanel == null);
            _spaceWorldPanel = viewModel;
        }

        void IViewModelParent<ISpaceWorldPanelViewModel>.Unregister(ISpaceWorldPanelViewModel viewModel)
        {
            Debug.Assert(_spaceWorldPanel == viewModel);
            _spaceWorldPanel = null;
        }

        void IViewModelParent<INavigationPanelViewModel>.Register(INavigationPanelViewModel viewModel)
        {
            Debug.Assert(_navigationPanel == null);
            _navigationPanel = viewModel;
        }

        void IViewModelParent<INavigationPanelViewModel>.Unregister(INavigationPanelViewModel viewModel)
        {
            Debug.Assert(_navigationPanel == viewModel);
            _navigationPanel = null;
        }

        void IViewModelParent<ISightPanelViewModel>.Register(ISightPanelViewModel viewModel)
        {
            Debug.Assert(_sightPanel == null);
            _sightPanel = viewModel;
        }

        void IViewModelParent<ISightPanelViewModel>.Unregister(ISightPanelViewModel viewModel)
        {
            Debug.Assert(_sightPanel == viewModel);
            _sightPanel = null;
        }

        #endregion
    }
}
 