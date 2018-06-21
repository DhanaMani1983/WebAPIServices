using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Saga.Gmd.WebApiServices.Common;

namespace Saga.Gmd.WebApiServices.DAL.Transaction
{
    public class TransactionDataAccess : ITransactionDataAccess
    {
      

        public string QuantumConnectionString { get; set; }

        private readonly ILog _logger;

        public TransactionDataAccess(ILog logger)
        {
            _logger = logger;
            QuantumConnectionString = ConfigurationManager.AppSettings["QuantumConnection"];
        }

        public int Save(Models.Transaction.TransactionParamter transaction)
        {
            int savedId;

            var payLoad = JsonConvert.SerializeObject(transaction, Formatting.None);

            using (var conn = new SqlConnection(QuantumConnectionString))
            {
                using (var cmd = new SqlCommand("[dbo].[enq_transaction]", conn))
                { 
                    try
                    {
                        cmd.Connection.Open();

                        var param = new SqlParameter("@feeder_system", SqlDbType.VarChar, 30);
                        param.SqlValue = transaction.FeederSystem; // was hard coded to "SID" changed to pick up detail from parameter;
                        cmd.Parameters.Add(param);

                        var param1 = new SqlParameter("@data_source", SqlDbType.VarChar, 30);
                        param1.SqlValue = transaction.SourceSystem; // was hard coded to "SID" changed to pick up detail from parameter;
                        cmd.Parameters.Add(param1);

                        var param2 = new SqlParameter("@trans_id", SqlDbType.BigInt);
                        Int64 seqNo;
                        var isValid = Int64.TryParse(transaction.SeqNo, out seqNo);
                        param2.SqlValue = seqNo;
                        cmd.Parameters.Add(param2);

                        var param3 = new SqlParameter("@data", SqlDbType.VarChar, -1);
                        param3.SqlValue = payLoad;
                        cmd.Parameters.Add(param3);

                        var param4 = new SqlParameter("@priority", SqlDbType.SmallInt); 
                        param4.SqlValue = 0;
                        cmd.Parameters.Add(param4);

                        cmd.CommandType = CommandType.StoredProcedure;

                        // the commented code on the line below will only return the 'rows affected' and not the id of the new row
                        // savedId = cmd.ExecuteNonQuery();

                        var result = cmd.ExecuteScalar();

                        string id = result + "";

                        if (int.TryParse(id, out savedId) == false)
                        {
                            // result is probably an error message!
                            throw new Exception(result as string);
                        }

                    }
                    catch (SqlException ex)
                    {
                        _logger.Error("TransactionDataAccess ErrorTag: " + ErrorTagProvider.ErrorTagDatabase + " -- " + ex.Message, ex);
                        throw new Exception(string.Format(DatabaseMessage.DatabaseException, ErrorTagProvider.ErrorTagDatabase));
                    }
                } 
            }
            return savedId;
        }

    }
}
