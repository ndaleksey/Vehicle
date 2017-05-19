using System.Windows.Input;
using DevExpress.Mvvm;
using Swsu.BattleFieldMonitor.Infrastructure;

namespace Swsu.BattleFieldMonitor.ViewModels
{
	public class MainViewModel : ViewModelBase
	{
		#region Constants

		private const double Scale = 0.75;
		#endregion

		#region Fields
		private int _scaleDenominator;
		private MapToolMode _mapToolMode;
		#endregion

		#region Properties
		public int ScaleDenominator
		{
			get { return _scaleDenominator; }
			set { SetProperty(ref _scaleDenominator, value, nameof(ScaleDenominator)); }
		}
		public MapToolMode MapToolMode
		{
			get { return _mapToolMode; }
			private set { SetProperty(ref _mapToolMode, value, nameof(MapToolMode)); }
		}

		#endregion

		#region Commands
		public ICommand MapScaleInCommand { get; }
		public ICommand MapScaleOutCommand { get; }
		#endregion

		#region Constructors

		public MainViewModel()
		{
			MapScaleInCommand = new DelegateCommand(MapScaleIn, CanMapScaleIn);
			MapScaleOutCommand = new DelegateCommand(MapScaleOut, CanMapScaleOut);

			MapToolMode = MapToolMode.Pan;
//			ScaleDenominator = 1e7;
		}

		#endregion

		#region Commands' methods
		private bool CanMapScaleIn()
		{
			return true;
		}

		private void MapScaleIn()
		{
//			ScaleDenominator *= Scale;
		}

		private bool CanMapScaleOut()
		{
			return true;
		}

		private void MapScaleOut()
		{
//			ScaleDenominator /= Scale;
		}
		#endregion

		#region Methods

		#endregion

	}
}