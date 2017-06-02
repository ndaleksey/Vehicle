using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Swsu.BattleFieldMonitor.ViewModelInterfaces;

namespace Swsu.BattleFieldMonitor.ViewModels.PaneViewModel
{
    internal class MapPanelViewModel :
        ChildViewModelBase<IMapPanelViewModel, IMapPanelViewModelParent>, IMapPanelViewModel
    {
        #region Fields
        private bool _isMiniMapEnabled;

        #endregion

        #region Properties
        /// <summary>
        /// Включена мини-карта
        /// </summary>
        public bool IsMiniMapEnabled
        {
            get { return _isMiniMapEnabled; }
            set { SetProperty(ref _isMiniMapEnabled, value, nameof(IsMiniMapEnabled), OnIsMiniMapEnabledChanged); }
        }

        #endregion

        #region Commands

        #endregion

        #region Constructors

        #endregion

        #region Methods
        private void OnIsMiniMapEnabledChanged(bool oldValue, bool newValue)
        {
            if (Parent?.MapContainer != null)
            {
                Parent?.MapContainer?.SetMiniMap(newValue);
            }
        }
        #endregion
    }
}
