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
            	var obstacle = polygonShape.DataContext as ObstacleModel;
                ((ViewModel) DataContext).SelectedObject = obstacle;
            }

            var compositeShape = sender as CompositeShape;
            if (compositeShape != null)
            {
                if (compositeShape.DataContext is UnmannedVehicleModel)
                {
                    var vehicle = (UnmannedVehicleModel) compositeShape.DataContext;
                    ((ViewModel)DataContext).SelectedObject = vehicle;
                }

                if (compositeShape.DataContext is RouteModel)
                {
                    var route = (RouteModel) compositeShape.DataContext;
                    ((ViewModel)DataContext).SelectedObject = route;
                }
            }

            var geographicPointShape = sender as GeographicPointShape;
            if (geographicPointShape != null)
            {
                var beacon = geographicPointShape.DataContext as BeaconModel;
                ((ViewModel)DataContext).SelectedObject = beacon;
            }
        }
    }
}
