using System.Windows.Input;
using Swsu.BattleFieldMonitor.ViewModels.MapContainer;
using Swsu.Maps.Windows.Shapes;

namespace Swsu.BattleFieldMonitor.Views
{
    partial class MapContainerView
    {
        #region Fields

        //private ViewModel _viewModel;

        #endregion

        #region Constructors

        public MapContainerView()
        {
            InitializeComponent();
            //_viewModel = new ViewModel();
            //DataContext = _viewModel;
        }

        #endregion

        private void ContentElement_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            var polygonShape = sender as PolygonShape;
            if (polygonShape != null)
            {
            	var obstacle = polygonShape.DataContext as Obstacle;
                ((ViewModel) DataContext).SelectedObject = obstacle;
            }

            var compositeShape = sender as CompositeShape;
            if (compositeShape != null)
            {
                var vehicle = compositeShape.DataContext as UnmannedVehicle;
                ((ViewModel)DataContext).SelectedObject = vehicle;
            }

            var lineStringShape = sender as LineStringShape;
            if (lineStringShape != null)
            {
                var route = lineStringShape.DataContext as RouteModel;
                ((ViewModel)DataContext).SelectedObject = route;
            }
        }
    }
}
