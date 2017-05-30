using System;
using System.Xml.Serialization;

namespace Swsu.BattleFieldMonitor.Services.Implementations.Notifications
{
    public class PrimaryKey
    {
        #region Properties
        [XmlAttribute("id")]
        public Guid Id
        {
            get;
            set;
        }
        #endregion
    }
}
