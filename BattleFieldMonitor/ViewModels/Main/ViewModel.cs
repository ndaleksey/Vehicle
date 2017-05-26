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
        private bool _isScalingModeEnabled;
        private bool _switchToBeaconDtButtonChecked;
        private bool _switchToPointDtButtonChecked;
        private bool _switchToPreciseLsdtButtonChecked;
        private bool _switchToRouteDtButtonChecked;
        private bool _switchToQuickLsdtButtonChecked;

        #endregion

        #region Properties
        public double ScaleDenominator
        {
            get { return _scaleDenominator; }
            set { SetProperty(ref _scaleDenominator, value, nameof(ScaleDenominator), OnScaleDenominatorChanged); }
        }

        /// <summary>
        /// Включен режим масштабирования (слежения за танком)
        /// </summary>
        public bool IsScalingModeEnabled
        {
            get { return _isScalingModeEnabled; }
            set { SetProperty(ref _isScalingModeEnabled, value, nameof(IsScalingModeEnabled), OnIsScalingModeEnabledChanged); }
        }

        /// <summary>
        /// Зажата кнопка "Добавить маяк"
        /// </summary>
        public bool SwitchToBeaconDtButtonChecked
        {
            get { return _switchToBeaconDtButtonChecked; }
            set
            {
                SetProperty(
                    ref _switchToBeaconDtButtonChecked,
                    value,
                    nameof(SwitchToBeaconDtButtonChecked));
            }
        }

        /// <summary>
        /// Зажата кнопка "Добавить точку"
        /// </summary>
        public bool SwitchToPointDtButtonChecked
        {
            get { return _switchToPointDtButtonChecked; }
            set
            {
                SetProperty(
                    ref _switchToPointDtButtonChecked,
                    value,
                    nameof(SwitchToPointDtButtonChecked));
            }
        }

        /// <summary>
        /// Зажата кнопка "Рисование точками"
        /// </summary>
        public bool SwitchToPreciseLsdtButtonChecked
        {
            get { return _switchToPreciseLsdtButtonChecked; }
            set
            {
                SetProperty(
                    ref _switchToPreciseLsdtButtonChecked,
                    value,
                    nameof(SwitchToPreciseLsdtButtonChecked));
            }
        }

        /// <summary>
        /// Зажата кнопка "Лассо"
        /// </summary>
        public bool SwitchToQuickLsdtButtonChecked
        {
            get { return _switchToQuickLsdtButtonChecked; }
            set
            {
                SetProperty(
                    ref _switchToQuickLsdtButtonChecked,
                    value,
                    nameof(SwitchToQuickLsdtButtonChecked));
            }
        }

        /// <summary>
        /// Зажата кнопка "Добавить трасса"
        /// </summary>
        public bool SwitchToRouteDtButtonChecked
        {
            get { return _switchToRouteDtButtonChecked; }
            set
            {
                SetProperty(
                    ref _switchToRouteDtButtonChecked,
                    value,
                    nameof(SwitchToRouteDtButtonChecked));
            }
        }
        #endregion

        #region Commands

        public ICommand MapScaleInCommand { get; }
        public ICommand MapScaleOutCommand { get; }

        /// <summary>
        /// Команда переключения на режим добавления точек
        /// </summary>
        public DelegateCommand SwitchToPointDtCommand { get; }

        /// <summary>
        /// Команда переключения на режим рисования объектов точками
        /// </summary>
        public DelegateCommand SwitchToPreciseLsdtCommand { get; }

        /// <summary>
        /// Команда переключения на режим добавления трасс
        /// </summary>
        public DelegateCommand SwitchToRouteDtCommand { get; }

        /// <summary>
        /// Команда переключенияы на режим рисования объектов лассо
        /// </summary>
        public DelegateCommand SwitchToQuickLsdtCommand { get; }

        /// <summary>
        /// Команда переключения на режим добавления маяков
        /// </summary>
        public DelegateCommand SwitchToBeaconDtCommand { get; }

        /// <summary>
        /// Команда включения режима масштабирования (слежения за танком)
        /// </summary>
        public DelegateCommand EnableScalingModeCommand { get; }

        #endregion

        #region Constructors

        public ViewModel()
        {
            MapScaleInCommand = new DelegateCommand(MapScaleIn, CanMapScaleIn);
            MapScaleOutCommand = new DelegateCommand(MapScaleOut, CanMapScaleOut);
            SwitchToPointDtCommand = new DelegateCommand(SwitchToPointDt);
            SwitchToPreciseLsdtCommand = new DelegateCommand(SwitchToPreciseLsdt);
            SwitchToRouteDtCommand = new DelegateCommand(SwitchToRouteDt);
            SwitchToQuickLsdtCommand = new DelegateCommand(SwitchToQuickLsdt);
            SwitchToBeaconDtCommand = new DelegateCommand(SwitchToBeaconDt);

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

        
        private void OnIsScalingModeEnabledChanged(bool oldValue, bool newValue)
        {
            if (_mapContainer != null)
            {
                _mapContainer.SetScalingMode(newValue);
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

        /// <summary>
        /// Переключиться на инструмент добавления маяков
        /// </summary>
        private void SwitchToBeaconDt()
        {
            if (SwitchToBeaconDtButtonChecked)
            {
                SwitchToPointDtButtonChecked = false;
                SwitchToPreciseLsdtButtonChecked = false;
                SwitchToQuickLsdtButtonChecked = false;
                SwitchToRouteDtButtonChecked = false;
                _mapContainer.SwitchToBeaconDrawingTool();
            }
            else
            {
                _mapContainer.SwitchToSimpleSelectionTool();
            }
        }

        /// <summary>
        /// Переключиться на инструмент добавления точек
        /// </summary>
        private void SwitchToPointDt()
        {
            if (SwitchToPointDtButtonChecked)
            {
                SwitchToBeaconDtButtonChecked = false;
                SwitchToPreciseLsdtButtonChecked = false;
                SwitchToQuickLsdtButtonChecked = false;
                SwitchToRouteDtButtonChecked = false;
                _mapContainer.SwitchToPointDrawingTool();
            }
            else
            {
                _mapContainer.SwitchToSimpleSelectionTool();
            }
        }

        /// <summary>
        /// Переключиться на инструмент рисования объектов точками
        /// </summary>
        private void SwitchToPreciseLsdt()
        {
            if (SwitchToPreciseLsdtButtonChecked)
            {
                SwitchToBeaconDtButtonChecked = false;
                SwitchToPointDtButtonChecked = false;
                SwitchToQuickLsdtButtonChecked = false;
                SwitchToRouteDtButtonChecked = false;
                _mapContainer.SwitchToPreciseLineStringDrawingTool();
            }
            else
            {
                _mapContainer.SwitchToSimpleSelectionTool();
            }
        }

        /// <summary>
        /// Переключиться на инструмент добавления трасс
        /// </summary>
        private void SwitchToRouteDt()
        {
            if (SwitchToRouteDtButtonChecked)
            {
                SwitchToPointDtButtonChecked = false;
                SwitchToBeaconDtButtonChecked = false;
                SwitchToPreciseLsdtButtonChecked = false;
                SwitchToQuickLsdtButtonChecked = false;
                _mapContainer.SwitchToRouteDrawingTool();
            }
            else
            {
                _mapContainer.SwitchToSimpleSelectionTool();
            }
        }

        /// <summary>
        /// Переключиться на инструмент Лассо
        /// </summary>
        private void SwitchToQuickLsdt()
        {
            if (SwitchToQuickLsdtButtonChecked)
            {
                SwitchToBeaconDtButtonChecked = false;
                SwitchToPointDtButtonChecked = false;
                SwitchToPreciseLsdtButtonChecked = false;
                SwitchToRouteDtButtonChecked = false;
                _mapContainer.SwitchToQuickLineStringDrawingTool();
            }
            else
            {
                _mapContainer.SwitchToSimpleSelectionTool();
            }
        }

        #endregion
    }
}