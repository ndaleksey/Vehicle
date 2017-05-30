using Swsu.Common;
using System.IO;
using System.Xml.Serialization;

namespace Swsu.BattleFieldMonitor.Services.Implementations.Notifications
{
    [XmlRoot("notification")]
    public class Notification
    {
        #region Fields
        private static XmlSerializer _serializer;
        #endregion

        #region Properties
        [XmlElement("new")]
        public PrimaryKey New
        {
            get;
            set;
        }

        [XmlElement("old")]
        public PrimaryKey Old
        {
            get;
            set;
        }

        [XmlAttribute("op")]
        public Operation Operation
        {
            get;
            set;
        }

        [XmlAttribute("tableName")]
        public string TableName
        {
            get;
            set;
        }
        #endregion

        #region Methods        
        public static Notification Parse(string source)
        {
            var serializer = PropertyHelpers.Initialized(ref _serializer, () => new XmlSerializer(typeof(Notification)));

            using (var reader = new StringReader(source))
            {
                return (Notification)serializer.Deserialize(reader);
            }
        }
        #endregion
    }
}