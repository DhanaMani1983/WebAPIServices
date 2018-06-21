
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Saga.Gmd.WebApiServices.DAL.Extensions
{
    public static class DalExtensions
    {
        /// <summary>
        /// Allow assignment of Sql param value in a fluent method chain
        /// </summary>
        /// <param name="param"></param>
        /// <param name="sqlValue"></param>
        /// <returns></returns>
        public static SqlParameter SetSqlValue(this SqlParameter param, object sqlValue)
        {
            param.SqlValue = sqlValue;
            return param;
        } 

        public static SqlParameter SetDirection(this SqlParameter param, ParameterDirection direction)
        {
            param.Direction = direction;
            return param;
        }

        public static SqlParameterCollection AddParameter(this SqlParameterCollection parameters, SqlParameter param)
        {
            parameters.Add(param);
            return parameters;
        }

        public static IEnumerable< KeyValuePair<string, object> > RenderParamsAsKeyValuePairs(this SqlParameterCollection parameters)
        {
            var retval = parameters.Cast<SqlParameter>()
                .Select(param => new KeyValuePair<string, object>(param.ParameterName, param.Value));

            return retval;
        }

    }
}
