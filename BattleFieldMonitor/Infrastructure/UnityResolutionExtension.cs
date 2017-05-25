using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using System;
using System.Configuration;
using System.Windows.Markup;

namespace Swsu.BattleFieldMonitor.Infrastructure
{
    internal class UnityResolutionExtension : MarkupExtension
    {
        #region Fields
        private static readonly UnityContainer _container;
        #endregion

        #region Constructors
        public UnityResolutionExtension()
        {
        }

        public UnityResolutionExtension(Type objectType)
        {
            ObjectType = objectType;
        }

        public UnityResolutionExtension(Type objectType,string objectName)
        {
            ObjectType = objectType;
            ObjectName = objectName;
        }

        static UnityResolutionExtension()
        {
            _container = new UnityContainer();

            if (ConfigurationManager.GetSection("unity") == null)
            {
                return;
            }

            _container.LoadConfiguration();
        }
        #endregion

        #region Properties
        public string ObjectName
        {
            get;
            set;
        }

        public Type ObjectType
        {
            get;
            set;
        }
        #endregion

        #region Methods
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (ObjectType == null)
            {
                return null;
            }

            return _container.Resolve(ObjectType, ObjectName);
        }
        #endregion
    }
}
