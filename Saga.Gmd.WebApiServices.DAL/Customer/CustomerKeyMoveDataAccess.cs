using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using log4net;
using Saga.Gmd.WebApiServices.Common;
using Saga.Gmd.WebApiServices.Models.Customer;

namespace Saga.Gmd.WebApiServices.DAL.Customer
{


    public interface ICustomerKeyMoveDataAccess
    {
        bool MoveCustomerKey(CustomerKeyMove customerKeyMove);
    }

    public class CustomerKeyMoveDataAccess:ICustomerKeyMoveDataAccess
    {
        private readonly ILog _logger;
        private readonly string _mciCrDbConnectionString;
        private readonly bool _logParameterValue;

        public CustomerKeyMoveDataAccess(ILog logger)
        {
            _logger = logger;
            _mciCrDbConnectionString = ConfigurationManager.AppSettings["MciCrConnection"];
            _logParameterValue = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableObjectDump"]);

        }

        public bool MoveCustomerKey(CustomerKeyMove customerKeyMove)
        {
            using(var conn = new SqlConnection(_mciCrDbConnectionString))
            {
                conn.Open();
                // var transaction = conn.BeginTransaction();
                try
                {
                    using (SqlCommand cmd = new SqlCommand("[dbo].[move_mci_key]", conn))
                    {
                        cmd.Parameters.AddWithValue("@p_from_source_key", customerKeyMove?.OriginalSourceKey);
                        cmd.Parameters.AddWithValue("@p_from_source_id", customerKeyMove?.OriginalSourceId);
                        cmd.Parameters.AddWithValue("@p_to_source_key", customerKeyMove?.NewSourceKey);
                        cmd.Parameters.AddWithValue("@p_to_source_id", customerKeyMove?.NewSourceId);
                        cmd.Parameters.AddWithValue("@p_key_to_move", customerKeyMove?.ToMoveSourceKey);
                        cmd.Parameters.AddWithValue("@p_id_to_move", customerKeyMove?.ToMoveSourceId);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.ExecuteScalar();
                    }
                }
                
                catch (Exception ex)
                {
                    //  transaction.Rollback();
                    _logger.Error(
                        "MoveCustomerKey : ErrorTag: " + ErrorTagProvider.ErrorTagDatabase + " -- " + ex.Message, ex);

                    if (_logParameterValue)
                    {
                        _logger.Error(
                            $"Parameters MoveCustomerKey:- OriginalSourceKey={customerKeyMove.OriginalSourceKey}, OriginalSourceId={customerKeyMove.OriginalSourceId}, NewSourceKey={customerKeyMove.NewSourceKey}, " +
                            $" NewSourceId= {customerKeyMove.NewSourceId}, ToMoveSourceKey={customerKeyMove.ToMoveSourceKey},  ToMoveSourceId={customerKeyMove.ToMoveSourceId}");
                    }

                    switch (ex.Message)
                    {
                        case "Customer Reference not found":
                        case "Source key value pair not found":
                        case "Key to move key value pair not found":
                        case "Key being moved does not belong to FROM customer":
                        case "Failed to move key":
                        {
                            throw new DataException(ex.Message);
                        }
                    }

                    throw new Exception(string.Format(ex.Message, ErrorTagProvider.ErrorTagDatabase));
                }
            }

            return true;
        }
    }
}