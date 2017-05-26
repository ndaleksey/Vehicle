using System.Collections.ObjectModel;
using DevExpress.Mvvm;

namespace Swsu.BattleFieldMonitor.ViewModels.MapContainer
{
    public class RouteModel : BindableBase
    {
        #region Properties

        /// <summary>
        /// Коллекция координат трассы объекта
        /// </summary>
        public ObservableCollection<Coord> Coords { get; }

        #endregion

        #region Constructors

        public RouteModel()
        {
            Coords = new ObservableCollection<Coord>();
        }

        #endregion
    }
}
