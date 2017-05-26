using System;
using System.Collections.ObjectModel;
using System.Globalization;
using DevExpress.Mvvm;
using Swsu.BattleFieldMonitor.Common;
using Swsu.BattleFieldMonitor.Converters1.Parameters;
using Swsu.BattleFieldMonitor.Services;
using Swsu.BattleFieldMonitor.ViewModelInterfaces;
using Swsu.Geo;

namespace Swsu.BattleFieldMonitor.ViewModels.MapContainer
{
    internal class ViewModel : ChildViewModelBase<IMapContainerViewModel, IMapContainerViewModelParent>,
        IMapContainerViewModel
    {
        #region Fields

        private MapToolMode _mapToolMode;
        private bool _isScalingModeEnabled;
        private double _scaleDenominator;
        private Obstacle _selectedObstacle;

        #endregion

        #region Properties

        /// <summary>
        /// Коллекция маяков, отображаемых на карте
        /// </summary>
	    public ObservableCollection<Beacon> Beacons { get; }

        /// <summary>
        /// Режим инструмента карты
        /// </summary>
        public MapToolMode MapToolMode
        {
            get { return _mapToolMode; }
            private set { SetProperty(ref _mapToolMode, value, nameof(MapToolMode)); }
        }

        private IMapViewerService1 MapViewerService => GetService<IMapViewerService1>();

        /// <summary>
        /// Признак, указывающий, включен ли режим масштабирования (слежения за танком)
        /// </summary>
        public bool IsScalingModeEnabled
        {
            get { return _isScalingModeEnabled; }
            private set { SetProperty(ref _isScalingModeEnabled, value, nameof(IsScalingModeEnabled)); }
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

        /// <summary>
        /// Коллекция танков, отображаемых на карте
        /// </summary>
	    public ObservableCollection<UnmannedVehicle> UnmannedVehicles { get; }

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
        /// Команда завершения редактирования
        /// </summary>
        public DelegateCommand ExitEditModeCommand { get; }

        /// <summary>
        /// Команда добавления маяка
        /// </summary>
        public DelegateCommand<PointDrawnParameter> OnBeaconDrawnCommand { get; }

        /// <summary>
        /// Команда добавления точки
        /// </summary>
        public DelegateCommand<PointDrawnParameter> OnPointDrawnCommand { get; }

        /// <summary>
        /// Команда добавления линии (границы полигона)
        /// </summary>
        public DelegateCommand<LineDrawnParameter> OnLineDrawnCommand { get; }

        /// <summary>
        /// Команда добавления отслеживаемого объекта
        /// </summary>
        public DelegateCommand SetTrackedVehicleCommand { get; }

        #endregion

        #region Constructors

        public ViewModel(IDatabase database)
        {
            MapToolMode = MapToolMode.SimpleSelection;

            EditCommand = new DelegateCommand(Edit);
            ExitEditModeCommand = new DelegateCommand(ExitEditMode);
            DeleteCommand = new DelegateCommand(Delete);
            OnLineDrawnCommand = new DelegateCommand<LineDrawnParameter>(OnLineDrawn);
            OnBeaconDrawnCommand = new DelegateCommand<PointDrawnParameter>(OnBeaconDrawn);
            OnPointDrawnCommand = new DelegateCommand<PointDrawnParameter>(OnPointDrawn);
            SetTrackedVehicleCommand = new DelegateCommand(SetTrackedVehicle);

            UnmannedVehicles = new ObservableCollection<UnmannedVehicle>();

            foreach (var unmannedVehicle in database.UnmannedVehicles.Objects)
            {
                UnmannedVehicles.Add(new UnmannedVehicle(this, unmannedVehicle.Location.Latitude, unmannedVehicle.Location.Longitude, unmannedVehicle.Heading) { Speed = unmannedVehicle.Speed });
            }

            database.UnmannedVehicles.ObjectsAdded += UnmannedVehicles_ObjectsAdded;
            database.UnmannedVehicles.ObjectsRemoved += UnmannedVehicles_ObjectsRemoved;

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

            Beacons = new ObservableCollection<Beacon>
            {
                new Beacon(10, 10)
            };
        }

        private void UnmannedVehicles_ObjectsAdded(object sender, ObjectsAddedEventArgs<IUnmannedVehicle> e)
        {
            foreach (var unmannedVehicle in e.Objects)
            {
                var newUnnamedVehicle = new UnmannedVehicle(this, unmannedVehicle.Location.Latitude, unmannedVehicle.Location.Longitude, unmannedVehicle.Heading)
                {
                    //TODO: Добавить имя
                    Speed = unmannedVehicle.Speed
                };

                UnmannedVehicles.Add(newUnnamedVehicle);
            }
        }

        private void UnmannedVehicles_ObjectsRemoved(object sender, ObjectsRemovedEventArgs<IUnmannedVehicle> e)
        {
            //TODO:
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
        /// Включить режим редактирования препятствия
        /// </summary>
        private void Edit()
        {
            _selectedObstacle.IsEditMode = true;

            MapToolMode = MapToolMode.Reshaping;
        }

        /// <summary>
        /// Завершить режим редактирования препятствия
        /// </summary>
        private void ExitEditMode()
        {
            foreach (var obstacle in Obstacles)
            {
                obstacle.IsEditMode = false;
            }

            MapToolMode = MapToolMode.SimpleSelection;
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
        /// Метод, вызываемый при добавлении на карту маяка
        /// </summary>
        /// <param name="parameter">Параметр, содержащий координаты точки</param>
        private void OnBeaconDrawn(PointDrawnParameter parameter)
        {
            var latitude = parameter.Location.Latitude;
            var longitude = parameter.Location.Longitude;

            var newBeacon = new Beacon(latitude, longitude);

            Beacons.Add(newBeacon);
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
            var speed = 100 * random.NextDouble();

            var newVehicle = new UnmannedVehicle(this, latitude, longitude, azimuth)
            {
                Speed = speed,
                //Coords = 
                //{
                //    new Coord(latitude, longitude),
                //    new Coord(latitude-10, longitude-10),
                //    new Coord(latitude-20, longitude-10)
                //}
            };

            UnmannedVehicles.Add(newVehicle);

            if (IsScalingModeEnabled)
            {
                MapViewerService.Locate(new GeographicCoordinatesTuple(latitude, longitude));
            }
            
            //var latitude = parameter.Location.Latitude;
            //var longitude = parameter.Location.Longitude;

            //var obstacle = new Obstacle();

            //var sphere = new Sphere(6378136);

            //for (int angle = 0; angle < 360; angle++)
            //{
            //    var solution = sphere.SolveDirectGeodeticProblem(latitude, longitude, angle, 1000000);

            //    obstacle.Coords.Add(new Coord(solution.Latitude2, solution.Longitude2));
            //}

            //Obstacles.Add(obstacle);
        }

        private void OnScaleDenominatorChanged(double oldValue, double newValue)
        {
            Parent?.NotifyScaleDenominatorChanged(oldValue, newValue);
        }

        /// <summary>
        /// Установить отслеживаемый объект
        /// </summary>
        private void SetTrackedVehicle()
        {
            
        }

        #endregion

        #region Methods From Interface

        /// <summary>
        /// Включить/отключить режим масштабирования (слежения за танком)
        /// </summary>
        /// <param name="value">Истина, если включить, иначе ложь</param>
        public void SetScalingMode(bool value)
        {
            IsScalingModeEnabled = value;
        }

        /// <summary>
        /// Переключиться на инструмент добавления маяков
        /// </summary>
        public void SwitchToBeaconDrawingTool()
        {
            MapToolMode = MapToolMode.BeaconDrawing;
        }

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
    /// Беспилотный автомобиль
    /// </summary>
    public class UnmannedVehicle : MapObject
    {
        #region Fields

        private double _azimuth;
        private string _calloutText;
        private ViewModel _parentViewModel;
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
        /// Родительская ViewModel для доступа к свойствам отслеживания
        /// </summary>
        internal ViewModel ParentViewModel
        {
            get { return _parentViewModel; }
            set { SetProperty(ref _parentViewModel, value, nameof(ParentViewModel)); }
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
        internal UnmannedVehicle(ViewModel parentViewModel)
        {
            UpdateCalloutText();
        }

        /// <summary>
        /// Инициализирует экземпляр класса
        /// </summary>
        /// <param name="latitude">Широта</param>
        /// <param name="longitude">Долдгота</param>
        /// <param name="azimuth">Азимут</param>
        internal UnmannedVehicle(ViewModel parentViewModel, double latitude, double longitude, double azimuth) : this(parentViewModel)
        {
            Azimuth = azimuth;
            Latitude = latitude;
            Longitude = longitude;

            //Coords = new ObservableCollection<Coord>();
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
    /// Маяк (точка возврата)
    /// </summary>
    public class Beacon : MapObject
    {
        #region Constructors 

        /// <summary>
        /// Инициализирует экземпляр класса
        /// </summary>
        public Beacon()
        {

        }

        /// <summary>
        /// Инициализирует экземпляр класса
        /// </summary>
        /// <param name="latitude">Широта</param>
        /// <param name="longitude">Долгота</param>
        public Beacon(double latitude, double longitude) : base(latitude, longitude)
        {
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
    /// Объект, отображаемый на карте
    /// </summary>
    public class MapObject : BindableBase
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

        /// <summary>
        /// Инициализирует экземпляр класса
        /// </summary>
        public MapObject()
        {

        }

        /// <summary>
        /// Инициализирует экземпляр класса
        /// </summary>
        /// <param name="latitude">Широта</param>
        /// <param name="longitude">Долдгота</param>
        /// <param name="azimuth">Азимут</param>
        public MapObject(double latitude, double longitude) : this()
        {
            Latitude = latitude;
            Longitude = longitude;
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