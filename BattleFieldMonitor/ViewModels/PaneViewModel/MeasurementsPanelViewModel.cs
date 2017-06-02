using DevExpress.Mvvm;
using Swsu.BattleFieldMonitor.ViewModelInterfaces;

namespace Swsu.BattleFieldMonitor.ViewModels.PaneViewModel
{
    internal class MeasurementsPanelViewModel :
        ChildViewModelBase<IMeasurementsPanelViewModel, IMeasurementsPanelViewModelParent>, IMeasurementsPanelViewModel
    {


        #region Fields

        private bool _switchToDistanceMeasurementToolButtonChecked;
        private bool _switchToAngleMeasurementToolButtonChecked;
        private bool _switchToHeightMeasurementToolButtonChecked;

        #endregion

        #region Properties

        /// <summary>
        /// Зажата кнопка "Измерение расстояний"
        /// </summary>
        public bool SwitchToDistanceMeasurementToolButtonChecked
        {
            get { return _switchToDistanceMeasurementToolButtonChecked; }
            set
            {
                SetProperty(
                    ref _switchToDistanceMeasurementToolButtonChecked,
                    value,
                    nameof(SwitchToDistanceMeasurementToolButtonChecked));
            }
        }

        /// <summary>
        /// Зажата кнопка "Измерение углов"
        /// </summary>
        public bool SwitchToAngleMeasurementToolButtonChecked
        {
            get { return _switchToAngleMeasurementToolButtonChecked; }
            set
            {
                SetProperty(
                    ref _switchToAngleMeasurementToolButtonChecked,
                    value,
                    nameof(SwitchToAngleMeasurementToolButtonChecked));
            }
        }

        /// <summary>
        /// Зажата кнопка "Измерение перепадов высот"
        /// </summary>
        public bool SwitchToHeightMeasurementToolButtonChecked
        {
            get { return _switchToHeightMeasurementToolButtonChecked; }
            set
            {
                SetProperty(
                    ref _switchToHeightMeasurementToolButtonChecked,
                    value,
                    nameof(SwitchToHeightMeasurementToolButtonChecked));
            }
        }

        #endregion


        #region Commands

        /// <summary>
        /// Команда переключения на режим измерения расстояний
        /// </summary>
        public DelegateCommand SwitchToDistanceMeasurementToolCommand { get; }

        /// <summary>
        /// Команда переключения на режим измерения углов
        /// </summary>
        public DelegateCommand SwitchToAngleMeasurementToolCommand { get; }

        /// <summary>
        /// Команда переключения на режим измерения перепадов высот
        /// </summary>
        public DelegateCommand SwitchToHeightMeasurementToolCommand { get; }

        #endregion

        #region Constructors

        public MeasurementsPanelViewModel()
        {
            SwitchToDistanceMeasurementToolCommand = new DelegateCommand(SwitchToDistanceMeasurementTool);
            SwitchToAngleMeasurementToolCommand = new DelegateCommand(SwitchToAngleMeasurementTool);
            SwitchToHeightMeasurementToolCommand = new DelegateCommand(SwitchToHeightMeasurementTool);
        }


        #endregion

        #region Methods

        /// Переключиться на режим измерения расстояний
        private void SwitchToDistanceMeasurementTool()
        {
            if (SwitchToDistanceMeasurementToolButtonChecked)
            {
                SwitchToAngleMeasurementToolButtonChecked = false;
                SwitchToHeightMeasurementToolButtonChecked = false;
                Parent?.MapContainer?.SwitchToDistanceMeasurementTool();
            }
            else
            {
                Parent?.MapContainer?.SwitchToSimpleSelectionTool();
            }

        }

        /// <summary>
        /// Переключиться на режим измерения углов
        /// </summary>
        private void SwitchToAngleMeasurementTool()
        {
            if (SwitchToAngleMeasurementToolButtonChecked)
            {
                SwitchToDistanceMeasurementToolButtonChecked = false;
                SwitchToHeightMeasurementToolButtonChecked = false;
                Parent?.MapContainer?.SwitchToAngleMeasurementTool();
            }
            else
            {
                Parent?.MapContainer?.SwitchToSimpleSelectionTool();
            }


        }

        /// <summary>
        /// Переключиться на режим измерения перепадов высот
        /// </summary>
        private void SwitchToHeightMeasurementTool()
        {
            if (SwitchToHeightMeasurementToolButtonChecked)
            {

                SwitchToAngleMeasurementToolButtonChecked = false;
                SwitchToDistanceMeasurementToolButtonChecked = false;
                Parent?.MapContainer?.SwitchToHeightMeasurementTool();
            }
            else
            {
                Parent?.MapContainer?.SwitchToSimpleSelectionTool();
            }
        }

        #endregion

    }
}
