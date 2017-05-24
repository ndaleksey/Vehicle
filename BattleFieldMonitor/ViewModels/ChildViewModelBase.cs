using Swsu.BattleFieldMonitor.ViewModelInterfaces;
using Swsu.Common;
using Swsu.Common.Windows.DevExpress;

namespace Swsu.BattleFieldMonitor.ViewModels
{
    /// <summary>
    /// Модели представлений, которые могут содержаться в других представлениях,
    /// должны использовать этот класс в качестве базового.
    /// </summary>
    /// <typeparam name="T">Интерфейс, реализуемый моделью представления, наследованной от данной.</typeparam>
    /// <typeparam name="TParent">Интерфейс, реализуемый моделью представления, содержащей наследованную от данной.</typeparam>
    internal abstract class ChildViewModelBase<T, TParent> : ViewModelBaseExtended
        where T : class
        where TParent : class, IViewModelParent<T>
    {
        #region Fields
        private TParent _parent;
        #endregion

        #region Constructors
        protected ChildViewModelBase()
        {
        }
        #endregion

        #region Properties
        protected TParent Parent
        {
            get { return _parent; }
            private set { PropertyHelpers.Set(ref _parent, value, OnParentChanged); }
        }
        #endregion

        #region Methods               
        /// <summary>
        /// При замещении ОБЯЗАТЕЛЬНО вызывать базовый метод.
        /// </summary>
        protected virtual void OnParentChanged(TParent oldValue, TParent newValue)
        {
            var viewModel = ToDerivedViewModel();

            if (oldValue != null)
            {
                oldValue.Unregister(viewModel);
            }

            if (newValue != null)
            {
                newValue.Register(viewModel);
            }
        }

        protected override void OnParentViewModelChanged(object parentViewModel)
        {
            base.OnParentViewModelChanged(parentViewModel);
            Parent = parentViewModel as TParent;
        }

        protected virtual T ToDerivedViewModel()
        {
            return (T)(object)this;
        }
        #endregion
    }
}