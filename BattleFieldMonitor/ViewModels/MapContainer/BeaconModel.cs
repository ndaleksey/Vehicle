using Swsu.BattleFieldMonitor.Common;

namespace Swsu.BattleFieldMonitor.ViewModels.MapContainer
{
    /// <summary>
    /// Маяк (точка возврата)
    /// </summary>
    public class BeaconModel : MapObjectModel
    {
        #region Fields

        private bool _isTracked;
        private ViewModel _parentViewModel;
        private static int _beaconNumber = 1;

        #endregion

        #region Properties

        /// <summary>
        /// Признак, указывающий, что за объектом необходимо следить
        /// </summary>
        internal bool IsTracked
        {
            get { return _isTracked; }
            set { SetProperty(ref _isTracked, value, nameof(IsTracked), UpdateTracking); }
        }

        /// <summary>
        /// Родительская ViewModel для доступа к свойствам отслеживания
        /// </summary>
        internal ViewModel ParentViewModel
        {
            get { return _parentViewModel; }
            set { SetProperty(ref _parentViewModel, value, nameof(ParentViewModel), UpdateTracking); }
        }

        #endregion

        #region Constructors 

        /// <summary>
        /// Инициализирует экземпляр класса
        /// </summary>
        internal BeaconModel(ViewModel parentViewModel)
        {
            ParentViewModel = parentViewModel;
        }

        /// <summary>
        /// Инициализирует экземпляр класса
        /// </summary>
        /// <param name="parentViewModel">Родительская ViewModel для доступа к функциям слежения</param>
        /// <param name="latitude">Широта</param>
        /// <param name="longitude">Долгота</param>
        internal BeaconModel(ViewModel parentViewModel, double latitude, double longitude) : base(latitude, longitude)
        {
            DisplayName = $"Маяк {_beaconNumber}";
            _beaconNumber++;

            ParentViewModel = parentViewModel;
        }

        #endregion

        #region Methods

        private void UpdateTracking()
        {
            if (ParentViewModel != null && ParentViewModel.IsCenteringModeEnabled)
            {
                if (IsTracked)
                {
                    ParentViewModel.MapViewerService.Locate(new GeographicCoordinatesTuple(Latitude, Longitude));
                }

            }
        }

        #endregion
    }
}
