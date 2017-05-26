using System;
using System.Collections.Generic;

namespace Swsu.BattleFieldMonitor.Services
{
    internal class ObjectsAddedEventArgs<T> : EventArgs
    {
        #region Constructors
        internal ObjectsAddedEventArgs(IReadOnlyCollection<T> objects, ObjectsAdditionReason reason)
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

        public ObjectsAdditionReason Reason
        {
            get;
        }
        #endregion
    }
}
