using System.Data;
using System.Data.Common;

namespace Saga.Gmd.WebApiServices.DAL.Infrastructure
{
    public interface IConnectionFactory
    {
        IDbConnection GetConnection( string connectionString );
    }

    public class ConnectionFactory : IConnectionFactory
    {
        public IDbConnection GetConnection(string connectionString)
        {
                Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
            // Not workomng 

                var factory = DbProviderFactories.GetFactory("System.Data.SqlClient");
                var conn = factory.CreateConnection();
                conn.ConnectionString = connectionString;
                conn.Open();
                return conn;
        }
    }
}
