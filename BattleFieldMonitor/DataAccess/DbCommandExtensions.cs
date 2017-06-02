using System.Data;
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

		public static DbParameter AddParameter(this DbCommand command, string name, DbType dbType, object value)
		{
			var param = command.CreateParameter();
			param.ParameterName = name;
			param.DbType = dbType;
			param.Value = value;
			command.Parameters.Add(param);
			return param;
		}
		#endregion
	}
}
