using System.Collections.ObjectModel;
using DevExpress.Mvvm;

namespace Swsu.BattleFieldMonitor.Models
{
    /// <summary>
    /// Класс, представляющий собой препятствие, отображаемое на карте в виде полигона
    /// </summary>
    public class Obstacle : BindableBase
    {
        #region Properties

        public ObservableCollection<Coord> Coords { get; }

        #endregion

        #region Constructors

        public Obstacle()
        {
            Coords = new ObservableCollection<Coord>
            {
                new Coord(0, 0),
                new Coord(-30, 0),
                new Coord(-30, -30),
                new Coord(0, -30)
            };
        }

        #endregion
    }
}
