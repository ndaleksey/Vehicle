using System;
using System.Collections.ObjectModel;

namespace Swsu.BattleFieldMonitor.Services.Implementations
{
    internal class IdentifiableObjectCollection<T> : KeyedCollection<Guid, T>
        where T : IIdentifiableObject
    {
        #region Methods
        protected override Guid GetKeyForItem(T item)
        {
            return item.Id;
        }
        #endregion
    }
}
