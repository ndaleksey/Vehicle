using System;
using System.Collections.ObjectModel;
using DevExpress.Mvvm;
using Swsu.BattleFieldMonitor.Converters1.Parameters;
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
        private bool _switchToPointDtButtonChecked;
        private bool _switchToPreciseLsdtButtonChecked;
        private bool _switchToQuickLsdtButtonChecked;

        #endregion

        #region Properties

        /// <summary>
        /// Коллекция танков, отображаемых на карте
        /// </summary>
	    public ObservableCollection<MapObject> MapObjects { get; }

        /// <summary>
        /// Режим инструмента карты
        /// </summary>
        public MapToolMode MapToolMode
        {
            get { return _mapToolMode; }
            private set { SetProperty(ref _mapToolMode, value, nameof(MapToolMode)); }
        }

        /// <summary>
        /// Коллекция препятствий, отображаемых на карте
        /// </summary>
	    public ObservableCollection<Obstacle> Obstacles { get; }

        /// <summary>
        /// Знаменатель масштаба
        /// </summary>
        public double ScaleDenominator
        {
            get { return _scaleDenominator; }
            set { SetProperty(ref _scaleDenominator, value, nameof(ScaleDenominator), OnScaleDenominatorChanged); }
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

        #endregion

        #region Delegate Commands

        /// <summary>
        /// Команда добавления точки
        /// </summary>
        public DelegateCommand<PointDrawnParameter> OnPointDrawnCommand { get; }

        /// <summary>
        /// Команда добавления линии (границы полигона)
        /// </summary>
        public DelegateCommand<LineDrawnParameter> OnLineDrawnCommand { get; }

        /// <summary>
        /// Команда переключения на режим добавления точек
        /// </summary>
        public DelegateCommand SwitchToPointDtCommand { get; }

        /// <summary>
        /// Команда переключения на режим рисования объектов точками
        /// </summary>
        public DelegateCommand SwitchToPreciseLsdtCommand { get; }

        /// <summary>
        /// Команда переключенияы на режим рисования объектов лассо
        /// </summary>
        public DelegateCommand SwitchToQuickLsdtCommand { get; }

        #endregion

        #region Constructors

        public ViewModel()
        {
            MapToolMode = MapToolMode.Pan;

            OnLineDrawnCommand = new DelegateCommand<LineDrawnParameter>(OnLineDrawn);
            OnPointDrawnCommand = new DelegateCommand<PointDrawnParameter>(OnPointDrawn);
            SwitchToPointDtCommand = new DelegateCommand(SwitchToPointDt);
            SwitchToPreciseLsdtCommand = new DelegateCommand(SwitchToPreciseLsdt);
            SwitchToQuickLsdtCommand = new DelegateCommand(SwitchToQuickLsdt);

            MapObjects = new ObservableCollection<MapObject>();

            Obstacles = new ObservableCollection<Obstacle>();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Метод, вызываемый при добавлении на карту полигона
        /// </summary>
        /// <param name="parameter">Параметр, содержащий координаты точки</param>
        private void OnLineDrawn(LineDrawnParameter parameter)
        {
            var newObstacle = new Obstacle();

            foreach (var location in parameter.Locations)
            {
                newObstacle.Coords.Add(new Coord(location.Latitude, location.Longitude));
            }

            Obstacles.Add(newObstacle);
        }

        /// <summary>
        /// Метод, вызываемый при добавлении на карту точки
        /// </summary>
        /// <param name="parameter">Параметр, содержащий координаты точки</param>
        private void OnPointDrawn(PointDrawnParameter parameter)
        {
            //TODO: Передать Сергею Мирошниченко
            var random = new Random();
            var latitude = parameter.Location.Latitude;
            var longitude = parameter.Location.Longitude;
            var azimuth = 360 * random.NextDouble() - 180;
            var speed = 10 * random.NextDouble();

            var newMapObject = new MapObject(latitude, longitude, azimuth) {Speed = speed};
            MapObjects.Add(newMapObject);
        }

        private void OnScaleDenominatorChanged(double oldValue, double newValue)
        {
            Parent?.NotifyScaleDenominatorChanged(oldValue, newValue);
        }

        /// <summary>
        /// Переключиться на инструмент добавления точек
        /// </summary>
        private void SwitchToPointDt()
        {
            if (SwitchToPointDtButtonChecked)
            {
                SwitchToPreciseLsdtButtonChecked = false;
                SwitchToQuickLsdtButtonChecked = false;
                MapToolMode = MapToolMode.PointDrawing;
            }
            else
            {
                MapToolMode = MapToolMode.Pan;
            }
        }

        /// <summary>
        /// Переключиться на инструмент рисования объектов точками
        /// </summary>
        private void SwitchToPreciseLsdt()
        {
            if (SwitchToPreciseLsdtButtonChecked)
            {
                SwitchToPointDtButtonChecked = false;
                SwitchToQuickLsdtButtonChecked = false;
                MapToolMode = MapToolMode.PreciseLineStringDrawing;
            }
            else
            {
                MapToolMode = MapToolMode.Pan;
            }
        }

        /// <summary>
        /// Переключиться на инструмент Лассо
        /// </summary>
        private void SwitchToQuickLsdt()
        {
            if (SwitchToQuickLsdtButtonChecked)
            {
                SwitchToPointDtButtonChecked = false;
                SwitchToPreciseLsdtButtonChecked = false;
                MapToolMode = MapToolMode.QuickLineStringDrawing;
            }
            else
            {
                MapToolMode = MapToolMode.Pan;
            }
        }
        #endregion
    }
}