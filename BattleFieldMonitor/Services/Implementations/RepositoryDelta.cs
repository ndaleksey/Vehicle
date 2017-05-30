﻿using Swsu.BattleFieldMonitor.Services.Implementations.Notifications;
using System;
using System.Data.Common;
using System.Collections.Generic;
using System.Threading;

namespace Swsu.BattleFieldMonitor.Services.Implementations
{
    internal abstract class RepositoryDelta
    {
        #region Fields
        private readonly HashSet<Guid> _deletedObjectIds = new HashSet<Guid>();

        private readonly HashSet<Guid> _insertedObjectIds = new HashSet<Guid>();

        private readonly HashSet<Guid> _updatedObjectIds = new HashSet<Guid>();
        #endregion

        #region Properties
        protected IEnumerable<Guid> DeletedObjectIds
        {
            get { return _deletedObjectIds; }
        }

        protected IEnumerable<Guid> InsertedObjectIds
        {
            get { return _insertedObjectIds; }
        }

        protected IEnumerable<Guid> UpdatedObjectIds
        {
            get { return _updatedObjectIds; }
        }
        #endregion

        #region Methods
        internal void ProcessNotification(Operation operation, PrimaryKey oldKey, PrimaryKey newKey)
        {
            switch (operation)
            {
                case Operation.Insert:
                    if (newKey == null)
                    {
                        // TODO: Issue some warning...
                        return;
                    }

                    ProcessNotificationInsert(newKey.Id);
                    break;

                case Operation.Update:
                    if (oldKey == null || newKey == null)
                    {
                        // TODO: Issue some warning...
                        return;
                    }

                    ProcessNotificationUpdate(oldKey.Id, newKey.Id);
                    break;

                case Operation.Delete:
                    if (oldKey == null)
                    {
                        // TODO: Issue some warning...
                        return;
                    }

                    ProcessNotificationDelete(oldKey.Id);
                    break;
            }
        }

        private void ProcessNotificationDelete(Guid id)
        {
            _deletedObjectIds.Add(id);
        }

        private void ProcessNotificationInsert(Guid id)
        {
            _insertedObjectIds.Add(id);
        }

        private void ProcessNotificationUpdate(Guid oldId, Guid newId)
        {
            if (oldId == newId)
            {
                _updatedObjectIds.Add(newId);
            }
            else
            {
                _deletedObjectIds.Add(oldId);
                _insertedObjectIds.Add(newId);
            }
        }
        #endregion
    }

    internal abstract class RepositoryDelta<T> : RepositoryDelta
        where T : IdentifiableObject
    {
        #region Methods
        protected internal abstract void LoadDelta(DbConnection connection, IReadOnlyDictionary<Guid, T> objectById, SynchronizationContext synchronizationContext);
        #endregion
    }
}