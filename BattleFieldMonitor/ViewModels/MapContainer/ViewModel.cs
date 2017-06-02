using System.Collections.ObjectModel;
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
        private bool _isMiniMapEnabled;
        private bool _isCenteringModeEnabled;
        private double _scaleDenominator;
        private object _selectedObject;
        private object _trackedObject;

        #endregion

        #region Properties

        /// <summary>
        /// Коллекция маяков, отображаемых на карте
        /// </summary>
	    public ObservableCollection<BeaconModel> Beacons { get; }

        /// <summary>
        /// Режим инструмента карты
        /// </summary>
        public MapToolMode MapToolMode
        {
            get { return _mapToolMode; }
            private set { SetProperty(ref _mapToolMode, value, nameof(MapToolMode)); }
        }

        internal IMapViewerService1 MapViewerService => GetService<IMapViewerService1>();


        /// <summary>
        /// Признак, указывающий, включена ли мини-карта
        /// </summary>
        public bool IsMiniMapEnabled
        {
            get { return _isMiniMapEnabled; }
            private set { SetProperty(ref _isMiniMapEnabled, value, nameof(IsMiniMapEnabled)); }
        }

        /// <summary>
        /// Признак, указывающий, включен ли режим центрирования (слежения за танком)
        /// </summary>
        public bool IsCenteringModeEnabled
        {
            get { return _isCenteringModeEnabled; }
            private set { SetProperty(ref _isCenteringModeEnabled, value, nameof(IsCenteringModeEnabled), OnIsScalingModeEnabledChanged); }
        }

        private void OnIsScalingModeEnabledChanged(bool oldValue, bool newValue)
        {
            if (oldValue)
            {
                foreach (var unmannedVehicle in UnmannedVehicles)
                {
                    unmannedVehicle.IsTracked = false;
                }

                foreach (var beacon in Beacons)
                {
                    beacon.IsTracked = false;
                }
            }

            //TODO: Нужно ли сразу же следить за танком при включении режима масштабирования?
            if (newValue)
            {
                if (UnmannedVehicles.Count == 1)
                {
                    UnmannedVehicles[0].IsTracked = true;
                    _trackedObject = UnmannedVehicles[0];
                }
            }
        }

        /// <summary>
        /// Коллекция препятствий, отображаемых на карте
        /// </summary>
	    public ObservableCollection<ObstacleModel> Obstacles { get; }

        /// <summary>
        /// Коллекция трасс, отображаемых на карте
        /// </summary>
	    public ObservableCollection<RouteModel> Routes { get; }

        /// <summary>
        /// Знаменатель масштаба
        /// </summary>
        public double ScaleDenominator
        {
            get { return _scaleDenominator; }
            set { SetProperty(ref _scaleDenominator, value, nameof(ScaleDenominator), OnScaleDenominatorChanged); }
        }

        /// <summary>
        /// Выделенный объект, линия или препятствие (грубо говоря, не выделенный, а тот, на котором щёлкнули мышкой, в частности, для вызова контекстного меню)
        /// </summary>
        public object SelectedObject
        {
            get { return _selectedObject; }
            set { SetProperty(ref _selectedObject, value, nameof(SelectedObject)); }
        }

        /// <summary>
        /// Коллекция танков, отображаемых на карте
        /// </summary>
	    public ObservableCollection<UnmannedVehicleModel> UnmannedVehicles { get; }

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
        /// Команда измерения
        /// </summary>
        public DelegateCommand<object> OnMeasuredCommand { get; }

        /// <summary>
        /// Команда добавления отслеживаемого объекта
        /// </summary>
        public DelegateCommand SetTrackedObjectCommand { get; }

        /// <summary>
        /// Команда добавления трассы
        /// </summary>
        public DelegateCommand<LineDrawnParameter> OnRouteDrawnCommand { get; }

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
            SetTrackedObjectCommand = new DelegateCommand(SetTrackedObject);
            OnRouteDrawnCommand = new DelegateCommand<LineDrawnParameter>(OnRouteDrawn);
            OnMeasuredCommand = new DelegateCommand<object>(OnMeasured);

            UnmannedVehicles = new ObservableCollection<UnmannedVehicleModel>();

            //TODO: После того, как была добавлена данная загрузка, возник следующий баг: после закрытия окна процесс остается висеть в памяти
            foreach (var unmannedVehicle in database.UnmannedVehicles.Objects)
            {
                UnmannedVehicles.Add(new UnmannedVehicleModel(this, unmannedVehicle));
            }

            database.UnmannedVehicles.ObjectsAdded += UnmannedVehicles_ObjectsAdded;
            database.UnmannedVehicles.ObjectsRemoved += UnmannedVehicles_ObjectsRemoved;

            Obstacles = new ObservableCollection<ObstacleModel>
            {
                //new Obstacle
                //{
                //    Coords =
                //    {
                //        new Coord(0, 0),
                //        new Coord(-30, 0),
                //        new Coord(-30, -30),
                //        new Coord(0, -30)
                //    }
                //}
            };

            Beacons = new ObservableCollection<BeaconModel>
            {
                //new Beacon(10, 10)
            };

            Routes = new ObservableCollection<RouteModel>();

            
        }

        private void OnMeasured(object obj)
        {
            
        }

        private void UnmannedVehicles_ObjectsAdded(object sender, ObjectsAddedEventArgs<IUnmannedVehicle> e)
        {
            foreach (var unmannedVehicle in e.Objects)
            {
                var newUnnamedVehicle = new UnmannedVehicleModel(this, unmannedVehicle);
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
        /// Удалить объект с карты
        /// </summary>
        private void Delete()
        {
            if (_selectedObject is ObstacleModel)
            {
                var selectedObstacle = _selectedObject as ObstacleModel;
                Obstacles.Remove(selectedObstacle); //TODO: Здесь может возникать исключение "Индекс находится за пределами диапазона", когда количество элементов в коллекции - 0, а мы пытаемся удалить. Оно возникает после того, как мы нажимаеем "Завершить редактирование", а затем снова - "Удалить"
            }

            if (_selectedObject is RouteModel)
            {
                var selectedRoute = _selectedObject as RouteModel;
                Routes.Remove(selectedRoute);
            }

            if (_selectedObject is BeaconModel)
            {
                var selectedBeacon = _selectedObject as BeaconModel;
                Beacons.Remove(selectedBeacon);
            }
        }

        /// <summary>
        /// Включить режим редактирования объекта
        /// </summary>
        private void Edit()
        {
            var obstacle = _selectedObject as ObstacleModel;
            if (obstacle != null)
            {
                var selectedObstacle = obstacle;
                selectedObstacle.IsEditMode = true;
                MapToolMode = MapToolMode.PolygonReshaping;
            }

            var model = _selectedObject as RouteModel;
            if (model != null)
            {
                var selectedRoute = model;
                selectedRoute.IsEditMode = true;
                MapToolMode = MapToolMode.LineReshaping;
            }
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
            var newObstacle = new ObstacleModel();

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

            var newBeacon = new BeaconModel(this, latitude, longitude);

            Beacons.Add(newBeacon);
        }

        /// <summary>
        /// Метод, вызываемый при добавлении на карту точки
        /// </summary>
        /// <param name="parameter">Параметр, содержащий координаты точки</param>
        private void OnPointDrawn(PointDrawnParameter parameter)
        {
            //TODO: Передать Сергею Мирошниченко
            //var random = new Random();
            //var latitude = parameter.Location.Latitude;
            //var longitude = parameter.Location.Longitude;
            //var azimuth = 360 * random.NextDouble() - 180;
            //var speed = 100 * random.NextDouble();

            //var newVehicle = new UnmannedVehicle(this, latitude, longitude, azimuth)
            //{
            //    Speed = speed,
            //    //Coords = 
            //    //{
            //    //    new Coord(latitude, longitude),
            //    //    new Coord(latitude-10, longitude-10),
            //    //    new Coord(latitude-20, longitude-10)
            //    //}
            //};

            //UnmannedVehicles.Add(newVehicle);

            var latitude = parameter.Location.Latitude;
            var longitude = parameter.Location.Longitude;

            var obstacle = new ObstacleModel();

            var sphere = new Sphere(6378136);

            for (int angle = 0; angle < 360; angle++)
            {
                var solution = sphere.SolveDirectGeodeticProblem(latitude, longitude, angle, 1000000);

                obstacle.Coords.Add(new Coord(solution.Latitude2, solution.Longitude2));
            }

            Obstacles.Add(obstacle);
        }

        /// <summary>
        /// Метод, вызываемый при добавлении на карту трассы
        /// </summary>
        /// <param name="parameter">Параметр, содержащий координаты точки</param>
        private void OnRouteDrawn(LineDrawnParameter parameter)
        {
            var newRouteModel = new RouteModel();

            foreach (var location in parameter.Locations)
            {
                newRouteModel.Coords.Add(new Coord(location.Latitude, location.Longitude));
            }

            Routes.Add(newRouteModel);
        }

        private void OnScaleDenominatorChanged(double oldValue, double newValue)
        {
            Parent?.NotifyScaleDenominatorChanged(oldValue, newValue);
            UpdateCentering();
        }

        /// <summary>
        /// Установить отслеживаемый объект
        /// </summary>
        private void SetTrackedObject()
        {
            if (_selectedObject is UnmannedVehicleModel)
            {
                foreach (var unmannedVehicle in UnmannedVehicles)
                {
                    unmannedVehicle.IsTracked = false;
                }

                foreach (var beacon in Beacons)
                {
                    beacon.IsTracked = false;
                }

                _trackedObject = _selectedObject;
                var selectedVehicle = (UnmannedVehicleModel) _selectedObject;
                selectedVehicle.IsTracked = true;
            }

            if (_selectedObject is BeaconModel)
            {
                foreach (var unmannedVehicle in UnmannedVehicles)
                {
                    unmannedVehicle.IsTracked = false;
                }

                foreach (var beacon in Beacons)
                {
                    beacon.IsTracked = false;
                }

                _trackedObject = _selectedObject;
                var selectedBeacon = (BeaconModel) _selectedObject;
                selectedBeacon.IsTracked = true;
            }
        }

        #endregion

        #region Methods From Interface

        /// <summary>
        /// Включить/отключить мини-карту
        /// </summary>
        /// <param name="value">Истина, если включить, иначе ложь</param>
        public void SetMiniMap(bool value)
        {
            IsMiniMapEnabled = value;
        }

        /// <summary>
        /// Включить/отключить режим масштабирования (слежения за танком)
        /// </summary>
        /// <param name="value">Истина, если включить, иначе ложь</param>
        public void SetCenteringMode(bool value)
        {
            IsCenteringModeEnabled = value;
        }

        /// <summary>
        /// Переключиться на инструмент измерения углов
        /// </summary>
        public void SwitchToAngleMeasurementTool()
        {
            MapToolMode = MapToolMode.AngleMeasurement;
            UnmannedVehicles[0].ShowCallout = false;
        }

        /// <summary>
        /// Переключиться на инструмент добавления маяков
        /// </summary>
        public void SwitchToBeaconDrawingTool()
        {
            MapToolMode = MapToolMode.BeaconDrawing;
        }

        /// <summary>
        /// Переключиться на инструмент измерения расстояний
        /// </summary>
        public void SwitchToDistanceMeasurementTool()
        {
            MapToolMode = MapToolMode.DistanceMeasurement;
        }

        public void SwitchToHeightMeasurementTool()
        {
            MapToolMode = MapToolMode.HeightMeasurement;
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
        /// Переключиться на инструмент добавления трасс
        /// </summary>
        public void SwitchToRouteDrawingTool()
        {
            MapToolMode = MapToolMode.RouteDrawing;
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
            UnmannedVehicles[0].ShowCallout = true;
        }
//
//        public void SwitchToHeightMeasurementTool()
//        {
//            MapToolMode = MapToolMode.HeightMeasurement;
//        }

        private void UpdateCentering()
        {
            if (IsCenteringModeEnabled)
            {
                if (_trackedObject is UnmannedVehicleModel)
                {
                    var trackedVehicle = _trackedObject as UnmannedVehicleModel;
                    MapViewerService.Locate(new GeographicCoordinatesTuple(trackedVehicle.Latitude, trackedVehicle.Longitude));
                }

                if (_trackedObject is BeaconModel)
                {
                    var trackedBeacon = _trackedObject as BeaconModel;
                    MapViewerService.Locate(new GeographicCoordinatesTuple(trackedBeacon.Latitude, trackedBeacon.Longitude));
                }
            }
        }

        #endregion
    }
}