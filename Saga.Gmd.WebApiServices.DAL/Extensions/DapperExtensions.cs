using System.Data;
using Dapper;

namespace Saga.Gmd.WebApiServices.DAL.Extensions
{
    public static class DapperExtensions
    {
        public static DynamicParameters AddParameter(this DynamicParameters _params, string name, object value, DbType? dbType, ParameterDirection? direction, int? size)
        {
            _params.Add(name: name, value: value, dbType: dbType, direction: direction, size: size);
            return _params;
        }

        public static DynamicParameters AddParameter(this DynamicParameters _params, string name, object value = null, DbType? dbType = default(DbType?), ParameterDirection? direction = default(ParameterDirection?), int? size = default(int?), byte? precision = default(byte?), byte? scale = default(byte?))
        {
            _params.Add(name: name, value: value, dbType: dbType, direction: direction, size: size, precision: precision, scale: scale );
            return _params;
        }

    }
}
