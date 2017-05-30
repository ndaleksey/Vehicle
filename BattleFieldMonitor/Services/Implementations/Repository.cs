using Swsu.BattleFieldMonitor.Services.Implementations.Notifications;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace Swsu.BattleFieldMonitor.Services.Implementations
{
    internal class Repository
    {
        #region Constructors
        public Repository(SynchronizationContext synchronizationContext)
        {
            SynchronizationContext = synchronizationContext;
            UpdatedObjectIds = new HashSet<Guid>();
        }
        #endregion

        #region Properties
        public ISet<Guid> UpdatedObjectIds
        {
            get;
        }

        protected SynchronizationContext SynchronizationContext
        {
            get;
        }
        #endregion

        #region Methods
        internal void OnNotification(Operation operation, PrimaryKey oldKey, PrimaryKey newKey)
        {
            switch (operation)
            {
                case Operation.Insert:
                    if (newKey == null)
                    {
                        // TODO: Issue some warning...
                        return;
                    }

                    OnNotificationInsert(newKey.Id);
                    break;

                case Operation.Update:
                    if (oldKey == null || newKey == null)
                    {
                        // TODO: Issue some warning...
                        return;
                    }

                    OnNotificationUpdate(oldKey.Id, newKey.Id);
                    break;

                case Operation.Delete:
                    if (oldKey == null)
                    {
                        // TODO: Issue some warning...
                        return;
                    }

                    OnNotificationDelete(oldKey.Id);
                    break;
            }
        }

        private void OnNotificationDelete(Guid id)
        {
            Trace.TraceInformation("Object deleted.");
        }

        private void OnNotificationInsert(Guid id)
        {
            Trace.TraceInformation("Object inserted.");
        }

        private void OnNotificationUpdate(Guid oldId, Guid newId)
        {
            Trace.TraceInformation("Object updated.");

            if (newId == oldId)
            {
                UpdatedObjectIds.Add(newId);
            }
        }
        #endregion
    }

    internal class Repository<T> : Repository, IRepository<T>
        where T : IIdentifiableObject
    {
        #region Fields
        private readonly IdentifiableObjectCollection<T> _objects = new IdentifiableObjectCollection<T>();
        #endregion

        #region Constructors
        public Repository(SynchronizationContext synchronizationContext) : base(synchronizationContext)
        {
        }
        #endregion

        #region Properites
        public IReadOnlyCollection<T> Objects
        {
            get { return _objects; }
        }
        #endregion

        #region Methods
        protected virtual void OnObjectsAdded(ObjectsAddedEventArgs<T> e)
        {
            ObjectsAdded?.Invoke(this, e);
        }

        protected virtual void OnObjectsRemoved(ObjectsRemovedEventArgs<T> e)
        {
            ObjectsRemoved?.Invoke(this, e);
        }

        internal void Add(IReadOnlyCollection<T> objects)
        {
            SynchronizationContext.Send(AddCallback, objects);
        }

        private void AddCallback(object state)
        {
            var objects = (IReadOnlyCollection<T>)state;

            foreach (var o in objects)
            {
                _objects.Add(o);
            }

            OnObjectsAdded(new ObjectsAddedEventArgs<T>(objects, ObjectsAdditionReason.Loaded));
        }
        #endregion

        #region Events
        public event EventHandler<ObjectsAddedEventArgs<T>> ObjectsAdded;

        public event EventHandler<ObjectsRemovedEventArgs<T>> ObjectsRemoved;
        #endregion
    }
}