using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Swsu.BattleFieldMonitor.ViewModelInterfaces;

namespace Swsu.BattleFieldMonitor.ViewModels.PaneViewModel
{
    internal class RouteAutomatPanelViewModel :
        ChildViewModelBase<IRouteAutomatPanelViewModel, IRouteAutomatPanelViewModelParent>, IRouteAutomatPanelViewModel
    {
    }
}
