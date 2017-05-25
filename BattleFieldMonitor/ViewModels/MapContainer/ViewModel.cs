using System;
using System.Collections.ObjectModel;
using System.Globalization;
using DevExpress.Mvvm;
using Swsu.BattleFieldMonitor.Converters1.Parameters;
using Swsu.BattleFieldMonitor.ViewModelInterfaces;

namespace Swsu.BattleFieldMonitor.ViewModels.MapContainer
{
    internal class ViewModel : ChildViewModelBase<IMapContainerViewModel, IMapContainerViewModelParent>,
        IMapContainerViewModel
    {
        #region Fields

        private MapToolMode _mapToolMode;
        private double _scaleDenominator;
        private Obstacle _selectedObstacle;

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
        /// Выделенное препятствие (грубо говоря, не выделенное, а то, на котором щёлкнули мышкой, в частности, для вызова контекстного меню)
        /// </summary>
        public Obstacle SelectedObstacle
        {
            get { return _selectedObstacle; }
            set { SetProperty(ref _selectedObstacle, value, nameof(SelectedObstacle)); }
        }

        #endregion

        #region Delegate Commands

        /// <summary>
        /// Команда удаления
        /// </summary>
        public DelegateCommand DeleteCommand { get; }

        /// <summary>
        /// Команда редактирования
        /// </summary>
        public DelegateCommand EditCommand { get; }

        /// <summary>
        /// Команда добавления точки
        /// </summary>
        public DelegateCommand<PointDrawnParameter> OnPointDrawnCommand { get; }

        /// <summary>
        /// Команда добавления линии (границы полигона)
        /// </summary>
        public DelegateCommand<LineDrawnParameter> OnLineDrawnCommand { get; }

        #endregion

        #region Constructors

        public ViewModel()
        {
            MapToolMode = MapToolMode.SimpleSelection;

            EditCommand = new DelegateCommand(Edit);
            DeleteCommand = new DelegateCommand(Delete);
            OnLineDrawnCommand = new DelegateCommand<LineDrawnParameter>(OnLineDrawn);
            OnPointDrawnCommand = new DelegateCommand<PointDrawnParameter>(OnPointDrawn);

            MapObjects = new ObservableCollection<MapObject>();

            Obstacles = new ObservableCollection<Obstacle>
            {
                new Obstacle
                {
                    Coords =
                    {
                        new Coord(0, 0),
                        new Coord(-30, 0),
                        new Coord(-30, -30),
                        new Coord(0, -30)
                    }
                }
            };
        }

        #endregion

        #region Methods

        /// <summary>
        /// Удалить препятствие с карты
        /// </summary>
        private void Delete()
        {
            Obstacles.Remove(_selectedObstacle);
        }

        /// <summary>
        /// Включить/выключить режим редактирования препятствия
        /// </summary>
        private void Edit()
        {
            _selectedObstacle.IsEditMode = !_selectedObstacle.IsEditMode;

            MapToolMode = _selectedObstacle.IsEditMode ? MapToolMode.Reshaping : MapToolMode.SimpleSelection;
        }

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

        #endregion

        #region Methods From Interface

        /// <summary>
        /// Переключиться на инструмент добавления точек
        /// </summary>
        public void SwitchToPointDrawingTool()
        {
            MapToolMode = MapToolMode.PointDrawing;
        }

        /// <summary>
        /// Переключиться на инструмент рисования объектов точками
        /// </summary>
        public void SwitchToPreciseLineStringDrawingTool()
        {
            MapToolMode = MapToolMode.PreciseLineStringDrawing;
        }

        /// <summary>
        /// Переключиться на инструмент Лассо
        /// </summary>
        public void SwitchToQuickLineStringDrawingTool()
        {
            MapToolMode = MapToolMode.QuickLineStringDrawing;
        }

        /// <summary>
        /// Переключиться на инструмент простого выделения
        /// </summary>
        public void SwitchToSimpleSelectionTool()
        {
            MapToolMode = MapToolMode.SimpleSelection;
        }

        #endregion
    }

    /// <summary>
    /// Точка, задаваемая широтой и долготой
    /// </summary>
    public class Coord : BindableBase
    {
        #region Fields

        private double _latitude;
        private double _longitude;

        #endregion

        #region Properties

        /// <summary>
        /// Широта
        /// </summary>
        public double Latitude
        {
            get { return _latitude; }
            set { SetProperty(ref _latitude, value, nameof(Latitude)); }
        }

        /// <summary>
        /// Долгота
        /// </summary>
        public double Longitude
        {
            get { return _longitude; }
            set { SetProperty(ref _longitude, value, nameof(Longitude)); }
        }

        #endregion

        #region Constructors

        public Coord()
        {

        }

        public Coord(double latitude, double longitude) : this()
        {
            Latitude = latitude;
            Longitude = longitude;
        }
        #endregion
    }

    /// <summary>
    /// Объект, отображаемый на карте //TODO: Скорее всего, это будет танк
    /// </summary>
    public class MapObject : BindableBase
    {
        #region Fields

        private double _azimuth;
        private string _calloutText;
        private double _latitude;
        private double _longitude;
        private double _speed;

        #endregion

        #region Properties

        /// <summary>
        /// Азимут
        /// </summary>
        public double Azimuth
        {
            get { return _azimuth; }
            set { SetProperty(ref _azimuth, value, nameof(Azimuth)); }
        }

        /// <summary>
        /// Текст из четырёх строк, отображаемый на выноске
        /// </summary>
        public string CalloutText
        {
            get { return _calloutText; }
            set { SetProperty(ref _calloutText, value, nameof(CalloutText)); }
        }

        /// <summary>
        /// Коллекция координат трассы объекта
        /// </summary>
        public ObservableCollection<Coord> Coords { get; }

        /// <summary>
        /// Широта
        /// </summary>
        public double Latitude
        {
            get { return _latitude; }
            set { SetProperty(ref _latitude, value, nameof(Latitude), UpdateCalloutText); }
        }

        /// <summary>
        /// Долгота
        /// </summary>
        public double Longitude
        {
            get { return _longitude; }
            set { SetProperty(ref _longitude, value, nameof(Longitude), UpdateCalloutText); }
        }

        /// <summary>
        /// Скорость, км/ч
        /// </summary>
        public double Speed
        {
            get { return _speed; }
            set { SetProperty(ref _speed, value, nameof(Speed), UpdateCalloutText); }
        }

        #endregion

        #region Constructors 

        /// <summary>
        /// Инициализирует экземпляр класса
        /// </summary>
        public MapObject()
        {
            UpdateCalloutText();
        }

        /// <summary>
        /// Инициализирует экземпляр класса
        /// </summary>
        /// <param name="latitude">Широта</param>
        /// <param name="longitude">Долдгота</param>
        /// <param name="azimuth">Азимут</param>
        public MapObject(double latitude, double longitude, double azimuth) : this()
        {
            Azimuth = azimuth;
            Latitude = latitude;
            Longitude = longitude;

            Coords = new ObservableCollection<Coord>();
        }

        #endregion

        #region Methods

        private void UpdateCalloutText()
        {
            CalloutText = $"Азимут: {Azimuth.ToString("0.00", CultureInfo.InvariantCulture)}°\n" +
                          $"Широта: {Latitude.ToString("0.000000", CultureInfo.InvariantCulture)}°\n" +
                          $"Долгота: {Longitude.ToString("0.000000", CultureInfo.InvariantCulture)}°\n" +
                          $"Скорость: {Speed.ToString("0.0", CultureInfo.InvariantCulture)} км/ч";
        }

        #endregion

    }

    /// <summary>
    /// Класс, представляющий собой препятствие, отображаемое на карте в виде полигона
    /// </summary>
    public class Obstacle : BindableBase
    {
        #region Properties

        private bool _isEditMode;

        /// <summary>
        /// Коллекция координат полигона
        /// </summary>
        public ObservableCollection<Coord> Coords { get; }

        /// <summary>
        /// Признак, указывающий на то, что объект находится в режиме редактирования
        /// </summary>
        public bool IsEditMode
        {
            get { return _isEditMode; }
            set { SetProperty(ref _isEditMode, value, nameof(IsEditMode)); }
        }

        #endregion

        #region Constructors

        public Obstacle()
        {
            Coords = new ObservableCollection<Coord>();
        }

        #endregion
    }
}