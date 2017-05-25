using System.Collections.ObjectModel;
using DevExpress.Mvvm;
using Swsu.BattleFieldMonitor.Converters1.Parameters;
using Swsu.BattleFieldMonitor.ViewModelInterfaces;
using Swsu.Geo;

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

        #endregion

        #region Constructors

        public ViewModel()
        {
            MapToolMode = MapToolMode.SimpleSelection;

            EditCommand = new DelegateCommand(Edit);
            ExitEditModeCommand = new DelegateCommand(ExitEditMode);
            DeleteCommand = new DelegateCommand(Delete);
            OnLineDrawnCommand = new DelegateCommand<LineDrawnParameter>(OnLineDrawn);
            OnBeaconDrawnCommand = new DelegateCommand<PointDrawnParameter>(OnBeaconDrawn);
            OnPointDrawnCommand = new DelegateCommand<PointDrawnParameter>(OnPointDrawn);

            UnmannedVehicles = new ObservableCollection<UnmannedVehicle>();

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
            //var random = new Random();
            //var latitude = parameter.Location.Latitude;
            //var longitude = parameter.Location.Longitude;
            //var azimuth = 360 * random.NextDouble() - 180;
            //var speed = 100 * random.NextDouble();

            //var newVehicle = new UnmannedVehicle(latitude, longitude, azimuth)
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

            var obstacle = new Obstacle();

            var sphere = new Sphere(6378136);

            for (int angle = 0; angle < 360; angle++)
            {
                var solution = sphere.SolveDirectGeodeticProblem(latitude, longitude, angle, 1000000);

                obstacle.Coords.Add(new Coord(solution.Latitude2, solution.Longitude2));
            }

            Obstacles.Add(obstacle);
        }

        private void OnScaleDenominatorChanged(double oldValue, double newValue)
        {
            Parent?.NotifyScaleDenominatorChanged(oldValue, newValue);
        }

        #endregion

        #region Methods From Interface
        
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
}