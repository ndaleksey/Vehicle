using System;

namespace Swsu.BattleFieldMonitor.Services
{
    internal interface IIdentifiableObject
    {
        #region Properties
        Guid Id
        {
            get;
        }
        #endregion
    }
}
