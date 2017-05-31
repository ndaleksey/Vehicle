using System.Data.Common;

namespace Swsu.BattleFieldMonitor.DataAccess
{
    internal static class DbCommandExtensions
    {
        #region Methods
        public static DbParameter AddParameter(this DbCommand command, string parameterName)
        {
            var parameter = command.CreateParameter();
            parameter.ParameterName = parameterName;
            command.Parameters.Add(parameter);
            return parameter;
        }
        #endregion
    }
}
