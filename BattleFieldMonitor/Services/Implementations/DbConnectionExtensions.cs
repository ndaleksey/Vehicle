using System.Data.Common;

namespace Swsu.BattleFieldMonitor.Services.Implementations
{
    internal static class DbConnectionExtensions
    {
        #region Methods
        public static void Listen(this DbConnection connection, string channelName)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = $"LISTEN {channelName}";
                command.ExecuteNonQuery();
            }
        }
        #endregion
    }
}
