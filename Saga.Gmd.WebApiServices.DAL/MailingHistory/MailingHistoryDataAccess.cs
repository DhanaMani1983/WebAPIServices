using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using log4net;
using Saga.Gmd.WebApiServices.Common;
using Saga.Gmd.WebApiServices.DAL.Customer;
using Saga.Gmd.WebApiServices.Models.Customer;
using Saga.Gmd.WebApiServices.Models.MailingHistory;

namespace Saga.Gmd.WebApiServices.DAL.MailingHistory
{
    public class MailingHistoryDataAccess : IMailingHistoryDataAccess
    {
        private readonly ILog _logger;

        public string PulicationDbConnectionString { get; set; }
        public string MciCrDbConnectionString { get; set; }
        private ICustomerDataAccess _customerDataAccess;


        public MailingHistoryDataAccess(ILog logger, ICustomerDataAccess customerDataAccess)
        {
            _logger = logger;
            _customerDataAccess = customerDataAccess;
            PulicationDbConnectionString = ConfigurationManager.AppSettings["PublicationDataConnection"];
            MciCrDbConnectionString = ConfigurationManager.AppSettings["MciCrConnection"];
        }

        public List<MatchedCustomer> GetMatchingCustomers(CustomerMatchParameter p)
        {
            List<MatchedCustomer> customerIds = null;

            try
            {
                customerIds = _customerDataAccess.GetMatchingCustomers(p);
            }
            catch (SqlException ex)
            {
                _logger.Error("GetMatchingCustomers:" + "ErrorTag: " + ErrorTagProvider.ErrorTagDatabase + " -- " + ex.Message, ex);
                throw new Exception(string.Format(DatabaseMessage.DatabaseException, ErrorTagProvider.ErrorTagDatabase));
            }
            catch (Exception ex)
            {
                _logger.Error("GetMatchingCustomers:" + "ErrorTag: " + ErrorTagProvider.ErrorTagDatabase + " -- " + ex.Message, ex);
                throw new Exception(string.Format(DatabaseMessage.DatabaseException, ErrorTagProvider.ErrorTagDatabase));
            }

            return customerIds;
        }






        public List<MailingHistoryResult> GetCustomerMailingHistory(string systemCode, IEnumerable<int> customerIds,
           string product = null, DateTime? fromDate = null, DateTime? toDate = null,
           string message = null)
        {

            List<MailingHistoryResult> mailingHistoryList = new List<MailingHistoryResult>();
            DataTable table = new DataTable();
            table.Columns.Add("customer_id", typeof(int));

            foreach (var id in customerIds)
            {
                table.Rows.Add(id);
            }


            using (var conn = new SqlConnection(PulicationDbConnectionString))
            {
                using (
                    var cmd = new SqlCommand("[MH_HIST].[get_customers_mailing_history]", conn))
                {
                    try
                    {
                        cmd.Connection.Open();

                        var param = new SqlParameter("@p_system_code", SqlDbType.VarChar, 20);
                        param.SqlValue = DBNull.Value;
                        cmd.Parameters.Add(param);

                        var param1 = new SqlParameter("@p_customers", SqlDbType.Structured);
                        param1.TypeName = "customer_id_type";
                        param1.SqlValue = table;
                        cmd.Parameters.Add(param1);
                        if (!string.IsNullOrEmpty(product))
                        {
                            var param2 = new SqlParameter("@p_product", SqlDbType.VarChar, 80);
                            param2.SqlValue = product;
                            cmd.Parameters.Add(param2);
                        }

                        if (fromDate != null && fromDate.Value != DateTime.MinValue)
                        {
                            var param4 = new SqlParameter("@p_from_date", SqlDbType.DateTime);
                            param4.SqlValue = fromDate.Value == DateTime.MinValue ? (object)DBNull.Value : fromDate.Value;
                            cmd.Parameters.Add(param4);
                        }

                        if (toDate != null && toDate.Value != DateTime.MinValue)
                        {
                            var param5 = new SqlParameter("@p_to_date", SqlDbType.DateTime);
                            param5.SqlValue = toDate.Value == DateTime.MinValue ? (object)DBNull.Value : toDate.Value;
                            cmd.Parameters.Add(param5);

                        }

                        var outparam = new SqlParameter("@p_message", SqlDbType.VarChar, -1);
                        outparam.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(outparam);
                        cmd.CommandType = CommandType.StoredProcedure;


                        var ds = new DataSet();
                        var adapter = new SqlDataAdapter(cmd);
                        adapter.Fill(ds);

                        // var reader = cmd.ExecuteReader();
                        message = (string)outparam.Value;
                        mailingHistoryList = BuildMailingHistory(ds);

                        //if (mailingHistoryList.MailingHistoriesResult.Count == 0)
                        //throw new NoDataFoundException("No Mailing History found for the customer.");
                    }
                    catch (SqlException ex)
                    {

                        _logger.Info("MailingHistoryRepository.GetCustomerMailingHistory (SqlException) :" + "ErrorTag: " + ErrorTagProvider.ErrorTagDatabase + " -- " + ex.Message, ex);
                        throw new Exception(string.Format(DatabaseMessage.DatabaseException, ErrorTagProvider.ErrorTagDatabase));

                    }
                    catch (Exception ex)
                    {
                        _logger.Info("MailingHistoryRepository.GetCustomerMailingHistory:" + "ErrorTag: " + ErrorTagProvider.ErrorTagDatabase + " -- " + ex.Message, ex);
                        throw new Exception(string.Format(DatabaseMessage.DatabaseException, ErrorTagProvider.ErrorTagDatabase));

                    }

                }
            }

            return mailingHistoryList;
        }

        public List<MailingHistoryResult> BuildMailingHistory(DataSet ds)
        {
            List<MailingHistoryResult> mailHisotryList = new List<MailingHistoryResult>();
            try
            {
                var table = ds.Tables[0];
                foreach (DataRow row in table.Rows)
                {
                    MailingHistoryResult mailing = new MailingHistoryResult();
                    mailing.MailingRef = row["MAILING_REF"]?.ToString();
                    mailing.SelectRefNumber = row["SELECT_REF_NUMBER"]?.ToString();
                    if (row["MAILED_DATE"] == null)
                    {
                        mailing.MailedDate = null;
                    }
                    else
                    {
                        mailing.MailedDate = DateTime.Parse(row["MAILED_DATE"].ToString());
                    }

                    mailing.MailedSourceCode = row["MAILED_SOURCE_CODE"]?.ToString();
                    mailing.LetterCode = row["LETTER_CODE"]?.ToString();
                    mailing.Channel = row["CHANNEL"]?.ToString();
                    mailing.CompanyCode = row["COMPANY_CODE"]?.ToString();
                    mailing.SelectDescription = row["SELECT_DESCRIPTION"]?.ToString();
                    mailing.ProductCode = row["PRODUCT_CODE"]?.ToString();
                    mailHisotryList.Add(mailing);
                }


            }
            catch (Exception ex)
            {
                _logger.Info("MailingHistoryRepository.BuildMailingHistory:" + "ErrorTag: " + ErrorTagProvider.ErrorTagDatabase + " -- " + ex.Message, ex);
                throw new Exception(string.Format(DatabaseMessage.DatabaseException, ErrorTagProvider.ErrorTagDatabase));
            }
            return mailHisotryList;
        }
    }
}