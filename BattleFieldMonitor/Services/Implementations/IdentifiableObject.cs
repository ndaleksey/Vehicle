using System;

namespace Swsu.BattleFieldMonitor.Services.Implementations
{
    internal abstract class IdentifiableObject : IIdentifiableObject
    {
        #region Constructors
        protected IdentifiableObject(Guid id)
        {
            Id = id;
        }
        #endregion

        #region Properties
        public Guid Id
        {
            get;
        }
        #endregion
    }
}
