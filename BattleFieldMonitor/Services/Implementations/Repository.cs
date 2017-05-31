using Swsu.BattleFieldMonitor.Services.Implementations.Notifications;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Threading;

namespace Swsu.BattleFieldMonitor.Services.Implementations
{
    internal abstract class Repository
    {
        #region Fields
        private RepositoryDelta _delta;
        #endregion

        #region Constructors
        public Repository()
        {
            SynchronizationContext = SynchronizationContext.Current;
        }
        #endregion

        #region Properties      
        protected SynchronizationContext SynchronizationContext
        {
            get;
        }

        internal bool HasDelta
        {
            get { return _delta != null; }
        }
        #endregion

        #region Methods
        protected abstract void LoadDeltaCore(DbConnection connection, RepositoryDelta repositoryDelta);

        protected internal abstract void Reload(DbConnection connection);

        internal void LoadDelta(DbConnection connection)
        {
            Debug.Assert(_delta != null);

            // Дельта не должна изменяться после начала загрузки,
            // поэтому сбрасываем текущую дельту, чтобы извещения,
            // приходящие во время загрузки ранее сформированной дельты,
            // приводили бы к созданию новой дельты.
            LoadDeltaCore(connection, Helpers.Exchange(ref _delta, null));
        }

        internal void ProcessNotification(Operation operation, PrimaryKey oldKey, PrimaryKey newKey)
        {
            if (_delta == null)
            {
                _delta = new RepositoryDelta();
            }

            _delta.ProcessNotification(operation, oldKey, newKey);
        }
        #endregion
    }

    internal abstract class Repository<T, TI> : Repository, IRepository<TI>
        where T : IdentifiableObject, TI
        where TI : IIdentifiableObject
    {
        #region Fields
        // Только для обращения из рабочей нити.
        private readonly Dictionary<Guid, T> _objectById = new Dictionary<Guid, T>();

        // Только для обращения из нити пользовательского интерфейса.
        private readonly IdentifiableObjectCollection<T> _objects = new IdentifiableObjectCollection<T>();
        #endregion

        #region Constructors
        public Repository()
        {
        }
        #endregion

        #region Properites
        public IReadOnlyCollection<TI> Objects
        {
            get { return _objects; }
        }
        #endregion

        #region Methods
        protected void Add(IReadOnlyCollection<T> objects)
        {
            foreach (var o in objects)
            {
                _objectById.Add(o.Id, o);
            }

            SynchronizationContext.Send(AddCallback, objects);
        }

        protected virtual void OnObjectsAdded(ObjectsAddedEventArgs<TI> e)
        {
            ObjectsAdded?.Invoke(this, e);
        }

        protected virtual void OnObjectsRemoved(ObjectsRemovedEventArgs<TI> e)
        {
            ObjectsRemoved?.Invoke(this, e);
        }

        protected bool TryGetObjectById(Guid id, out T result)
        {
            return _objectById.TryGetValue(id, out result);
        }

        private void AddCallback(object state)
        {
            var objects = (IReadOnlyCollection<T>)state;

            foreach (var o in objects)
            {
                _objects.Add(o);
            }

            OnObjectsAdded(new ObjectsAddedEventArgs<TI>(objects, ObjectsAdditionReason.Loaded));
        }
        #endregion

        #region Events
        public event EventHandler<ObjectsAddedEventArgs<TI>> ObjectsAdded;

        public event EventHandler<ObjectsRemovedEventArgs<TI>> ObjectsRemoved;
        #endregion
    }
}