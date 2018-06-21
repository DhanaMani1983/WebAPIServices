using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using Saga.Gmd.WebApiServices.Common;
using Saga.Gmd.WebApiServices.Models;

namespace Saga.Gmd.WebApiServices.DAL.GmdToAef
{
    public interface IGmdToAfeDataAccess
    {
        bool Save(int customerId, bool isAfeUpdated);
        bool Update(int rowId, bool isAfeUpdated);
        List<GmdToAefModel> GetUnProcessedCustomers(); 
    }

    public class GmdToAfeDataAccess : IGmdToAfeDataAccess
    {
        private readonly ILog _logger;
        private string MciCrDbConnectionString { get; }

        public GmdToAfeDataAccess(ILog logger)
        {
            _logger = logger;
            MciCrDbConnectionString = ConfigurationManager.AppSettings["MciCrConnection"];
        }

        public bool Save(int customerId, bool isAfeUpdated)
        {

            using (var conn = new SqlConnection(MciCrDbConnectionString))
            {
                using (
                    var cmd = new SqlCommand("[dbo].ProcessedCustomerWithAFE", conn))
                {
                    try
                    {
                        cmd.Connection.Open();

                        var param = new SqlParameter("@customerid", SqlDbType.Int);
                        param.SqlValue = customerId;
                        cmd.Parameters.Add(param);

                        var param1 = new SqlParameter("@isProcessed", SqlDbType.Bit);
                        param1.SqlValue = isAfeUpdated;
                        cmd.Parameters.Add(param1);
                        cmd.CommandType = CommandType.StoredProcedure;

                       var output =  cmd.ExecuteScalar();
                    }
                    catch (SqlException ex)
                    {
                        _logger.Info("GmdToAfeDataAccess.Save (SqlException) :" + "ErrorTag: " + ErrorTagProvider.ErrorTagDatabase + " -- " + ex.Message, ex);
                        throw new Exception(string.Format(DatabaseMessage.DatabaseException, ErrorTagProvider.ErrorTagDatabase));

                    }
                    catch (Exception ex)
                    {
                        _logger.Info("GmdToAfeDataAccess.Save :" + "ErrorTag: " + ErrorTagProvider.ErrorTagDatabase + " -- " + ex.Message, ex);
                        throw new Exception(string.Format(DatabaseMessage.DatabaseException, ErrorTagProvider.ErrorTagDatabase));
                    }
                }
            }
            return true;
        }

      

        public bool Update(int rowId, bool isAfeUpdated)
        {
            using (var conn = new SqlConnection(MciCrDbConnectionString))
            {
                using (
                    var cmd = new SqlCommand("[dbo].UpdateCustomerWithAFE", conn))
                {
                    try
                    {
                        cmd.Connection.Open();

                        var param = new SqlParameter("@id", SqlDbType.Int);
                        param.SqlValue = rowId;
                        cmd.Parameters.Add(param);

                        var param1 = new SqlParameter("@isProcessed", SqlDbType.Bit);
                        param1.SqlValue = isAfeUpdated;
                        cmd.Parameters.Add(param1);
                        cmd.CommandType = CommandType.StoredProcedure;

                        var output = cmd.ExecuteScalar();
                    }
                    catch (SqlException ex)
                    {
                        _logger.Info("GmdToAfeDataAccess.Update (SqlException) :" + "ErrorTag: " + ErrorTagProvider.ErrorTagDatabase + " -- " + ex.Message, ex);
                        throw new Exception(string.Format(DatabaseMessage.DatabaseException, ErrorTagProvider.ErrorTagDatabase));

                    }
                    catch (Exception ex)
                    {
                        _logger.Info("GmdToAfeDataAccess.Update :" + "ErrorTag: " + ErrorTagProvider.ErrorTagDatabase + " -- " + ex.Message, ex);
                        throw new Exception(string.Format(DatabaseMessage.DatabaseException, ErrorTagProvider.ErrorTagDatabase));
                    }
                }
            }
            return true;
        }


        
        public List<GmdToAefModel> GetUnProcessedCustomers()
        {
            List<GmdToAefModel> customerIds = new List<GmdToAefModel>();
            using (var conn = new SqlConnection(MciCrDbConnectionString))
            {
                using (
                    var cmd = new SqlCommand("[dbo].GetUnProcessedCustomers ", conn))
                {
                    try
                    {
                        cmd.Connection.Open();


                        var ds = new DataSet();
                        var adapter = new SqlDataAdapter(cmd);
                        adapter.Fill(ds);

                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                             var model = new GmdToAefModel();
                            model.Id = int.Parse(row["Id"].ToString());
                            model.IsAfeNotified = bool.Parse(row["IsAFE_Notified"].ToString());
                            model.CreatedDate = DateTime.Parse(row["Created_date"].ToString());
                            model.CustomerId = int.Parse(row["Customer_Id"].ToString());
                            customerIds.Add(model);
                        }
                        
                    }
                    catch (SqlException ex)
                    {
                        _logger.Info("GmdToAfeDataAccess.Update (SqlException) :" + "ErrorTag: " + ErrorTagProvider.ErrorTagDatabase + " -- " + ex.Message, ex);
                        throw new Exception(string.Format(DatabaseMessage.DatabaseException, ErrorTagProvider.ErrorTagDatabase));

                    }
                    catch (Exception ex)
                    {
                        _logger.Info("GmdToAfeDataAccess.Update :" + "ErrorTag: " + ErrorTagProvider.ErrorTagDatabase + " -- " + ex.Message, ex);
                        throw new Exception(string.Format(DatabaseMessage.DatabaseException, ErrorTagProvider.ErrorTagDatabase));
                    }
                }
            }
            return customerIds;
        }
    }
}
