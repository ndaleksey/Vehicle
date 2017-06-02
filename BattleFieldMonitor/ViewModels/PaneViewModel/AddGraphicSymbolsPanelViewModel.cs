using DevExpress.Mvvm;
using Swsu.BattleFieldMonitor.ViewModelInterfaces;

namespace Swsu.BattleFieldMonitor.ViewModels.PaneViewModel
{
    internal class AddGraphicSymbolsPanelViewModel :
        ChildViewModelBase<IAddGraphicSymbolsPanelViewModel, AddGraphicSymbolsPanelViewModelParent>, IAddGraphicSymbolsPanelViewModel
    {
        #region Fields
        private bool _switchToBeaconDtButtonChecked;
        #endregion

        #region Properties

        /// <summary>
        /// Зажата кнопка "Добавить маяк"
        /// </summary>
        public bool SwitchToBeaconDtButtonChecked
        {
            get { return _switchToBeaconDtButtonChecked; }
            set
            {
                SetProperty(
                    ref _switchToBeaconDtButtonChecked,
                    value,
                    nameof(SwitchToBeaconDtButtonChecked));
            }
        }

        #endregion

        #region Commands
        /// <summary>
        /// Команда переключения на режим добавления маяков
        /// </summary>
        public DelegateCommand SwitchToBeaconDtCommand { get; }

        #endregion

        #region Constructors
        public AddGraphicSymbolsPanelViewModel()
        {
            SwitchToBeaconDtCommand = new DelegateCommand(SwitchToBeaconDt);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Переключиться на инструмент добавления маяков
        /// </summary>
        private void SwitchToBeaconDt()
        {
            if (SwitchToBeaconDtButtonChecked)
            {
                Parent?.MapContainer?.SwitchToBeaconDrawingTool();
            }
            else
            {
                Parent?.MapContainer?.SwitchToSimpleSelectionTool();
            }
        }

        #endregion

    }
}