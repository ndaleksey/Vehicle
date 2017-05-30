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

        protected abstract RepositoryDelta BaseDelta
        {
            get;
        }

        internal bool HasDelta
        {
            get { return BaseDelta != null; }
        }
        #endregion

        #region Methods
        protected abstract void EnsureDeltaCreated();

        protected internal abstract void LoadDelta(DbConnection connection);

        protected internal abstract void Reload(DbConnection connection);

        internal void ProcessNotification(Operation operation, PrimaryKey oldKey, PrimaryKey newKey)
        {
            EnsureDeltaCreated();
            BaseDelta.ProcessNotification(operation, oldKey, newKey);
        }
        #endregion
    }

    internal abstract class Repository<T, TI> : Repository, IRepository<TI>
        where T : IdentifiableObject, TI
        where TI : IIdentifiableObject
    {
        #region Fields
        private RepositoryDelta<T> _delta;

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

        protected override sealed RepositoryDelta BaseDelta
        {
            get { return _delta; }
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

        protected abstract RepositoryDelta<T> CreateDelta();

        protected override sealed void EnsureDeltaCreated()
        {
            if (_delta == null)
            {
                _delta = CreateDelta();
            }
        }

        protected internal override sealed void LoadDelta(DbConnection connection)
        {
            Debug.Assert(_delta != null);

            // Дельта не должна изменяться после начала загрузки,
            // поэтому сбрасываем текущую дельту, чтобы извещения,
            // приходящие во время загрузки ранее сформированной дельты,
            // приводили бы к созданию новой дельты.
            Helpers.Exchange(ref _delta, null).LoadDelta(connection, _objectById, SynchronizationContext);
        }

        protected virtual void OnObjectsAdded(ObjectsAddedEventArgs<TI> e)
        {
            ObjectsAdded?.Invoke(this, e);
        }

        protected virtual void OnObjectsRemoved(ObjectsRemovedEventArgs<TI> e)
        {
            ObjectsRemoved?.Invoke(this, e);
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