using System.Windows.Input;
using Swsu.BattleFieldMonitor.ViewModels.MapContainer;
using Swsu.Maps.Windows.Shapes;

namespace Swsu.BattleFieldMonitor.Views
{
    partial class MapContainerView
    {
        #region Fields

        private ViewModel _viewModel;

        #endregion

        #region Constructors

        public MapContainerView()
        {
            InitializeComponent();
            _viewModel = new ViewModel();
            DataContext = _viewModel;
        }

        #endregion

        private void ContentElement_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is PolygonShape)
            {
                var polygonShape = sender as PolygonShape;
                var obstacle = polygonShape.DataContext as Obstacle;
                _viewModel.SelectedObstacle = obstacle;
            }
        }
    }
}
