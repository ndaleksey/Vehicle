using DevExpress.Mvvm;
using Swsu.BattleFieldMonitor.ViewModelInterfaces;

namespace Swsu.BattleFieldMonitor.ViewModels.PaneViewModel
{
  internal  class ManualRoutePanelViewModel :
        ChildViewModelBase<IManualRoutePanelViewModel, IManualRoutePanelViewModelParent>, IManualRoutePanelViewModel
    {
        #region Fields
        private bool _switchToRouteDtButtonChecked;
        #endregion

        #region Properties
        /// <summary>
        /// Зажата кнопка "Добавить трасса"
        /// </summary>
        public bool SwitchToRouteDtButtonChecked
        {
            get { return _switchToRouteDtButtonChecked; }
            set
            {
                SetProperty(
                    ref _switchToRouteDtButtonChecked,
                    value,
                    nameof(SwitchToRouteDtButtonChecked));
            }
        }
        #endregion

        #region Commnds

        /// <summary>
        /// Команда переключения на режим добавления трасс
        /// </summary>
        public DelegateCommand SwitchToRouteDtCommand { get; }
        #endregion

        #region Constructors

        public ManualRoutePanelViewModel()
        {
            SwitchToRouteDtCommand = new DelegateCommand(SwitchToRouteDt);
        }
        #endregion

        #region Methods

        /// <summary>
        /// Переключиться на инструмент добавления трасс
        /// </summary>
        private void SwitchToRouteDt()
        {
            if (SwitchToRouteDtButtonChecked)
            {
                Parent?.MapContainer?.SwitchToRouteDrawingTool();
            }
            else
            {
                Parent?.MapContainer?.SwitchToSimpleSelectionTool();
            }
        }

        #endregion
    }
}
