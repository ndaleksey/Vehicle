using System;
using System.Collections.Generic;

namespace Swsu.BattleFieldMonitor.Services
{
    internal class ObjectsRemovedEventArgs<T> : EventArgs
    {
        #region Constructors
        internal ObjectsRemovedEventArgs(IReadOnlyCollection<T> objects, ObjectsRemovalReason reason)
        {
            Objects = objects;
            Reason = reason;
        }
        #endregion

        #region Properties
        public IReadOnlyCollection<T> Objects
        {
            get;
        }

        public ObjectsRemovalReason Reason
        {
            get;
        }
        #endregion
    }
}
