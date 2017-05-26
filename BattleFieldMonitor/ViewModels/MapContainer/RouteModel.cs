using System.Collections.ObjectModel;
using DevExpress.Mvvm;

namespace Swsu.BattleFieldMonitor.ViewModels.MapContainer
{
    public class RouteModel : BindableBase
    {
        #region Fields

        private bool _isEditMode;

        #endregion

        #region Properties

        /// <summary>
        /// Коллекция координат трассы объекта
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

        public RouteModel()
        {
            Coords = new ObservableCollection<Coord>();
        }

        #endregion
    }
}
