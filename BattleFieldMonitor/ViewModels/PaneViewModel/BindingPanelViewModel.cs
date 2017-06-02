using Swsu.BattleFieldMonitor.ViewModelInterfaces;

namespace Swsu.BattleFieldMonitor.ViewModels.PaneViewModel
{
    class BindingPanelViewModel:ChildViewModelBase<IBindingPanelViewModel, IBindingPanelViewModelParent>, IBindingPanelViewModel
    {
        #region Fields
        private bool _isCenteringModeEnabled;

        #endregion

        #region Properties
        /// <summary>
        /// Включен режим центрирования (слежения за танком)
        /// </summary>
        public bool IsCenteringModeEnabled
        {
            get { return _isCenteringModeEnabled; }
            set { SetProperty(ref _isCenteringModeEnabled, value, nameof(IsCenteringModeEnabled), OnIsCenteringModeEnabledChanged); }
        }

        #endregion

        #region Commands

        #endregion

        #region Constructors

        #endregion

        #region Methods
        private void OnIsCenteringModeEnabledChanged(bool oldValue, bool newValue)
        {
            if (Parent?.MapContainer != null)
            {
                Parent?.MapContainer?.SetCenteringMode(newValue);
            }
        }

        #endregion
    }
}
