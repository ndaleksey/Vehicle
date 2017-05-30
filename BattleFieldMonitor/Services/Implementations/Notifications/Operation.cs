using System.Xml.Serialization;

namespace Swsu.BattleFieldMonitor.Services.Implementations.Notifications
{
    public enum Operation
    {
        [XmlEnum("INSERT")]
        Insert,

        [XmlEnum("UPDATE")]
        Update,

        [XmlEnum("DELETE")]
        Delete
    }
}
