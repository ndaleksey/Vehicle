using System;
using System.Collections.Generic;

namespace Swsu.BattleFieldMonitor.Services
{
    internal interface IRepository<T>
    {
        #region Properties
        IReadOnlyCollection<T> Objects
        {
            get;
        }
        #endregion

        #region Events
        event EventHandler<ObjectsAddedEventArgs<T>> ObjectsAdded;

        event EventHandler<ObjectsRemovedEventArgs<T>> ObjectsRemoved;
        #endregion
    }
}
