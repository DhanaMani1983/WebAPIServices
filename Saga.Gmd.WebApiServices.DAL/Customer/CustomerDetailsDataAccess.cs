using log4net;
using Saga.Gmd.WebApiServices.Common;
using Saga.Gmd.WebApiServices.DAL.Extensions;
using Saga.Gmd.WebApiServices.Models;
using Saga.Gmd.WebApiServices.Models.Customer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;

namespace Saga.Gmd.WebApiServices.DAL.Customer
{

    public interface ICustomerDetailsDataAccess
    {
        CustomerInfoDetails GetCustomerDetails(KeyValuePair<string, string> kvPair,
            AddressType addressType,
            SuppressionOptions suppressionOptions);

        // CustomerInfoDetails GetCustomerDetails(int customerId, AddressType addressType);
        List<CustomerIndexResult> GetCustomerAllIndexKeys(string sourceKey, string custRef, string targetKey);


    }
    public class CustomerDetailsDataAccess : ICustomerDetailsDataAccess
    {
        private readonly ILog _logger;
        private string MciCrDbConnectionString { get; }

        public CustomerDetailsDataAccess(ILog logger)
        {
            _logger = logger;
            MciCrDbConnectionString = ConfigurationManager.AppSettings["MciCrConnection"];
        }

        public CustomerInfoDetails GetCustomerDetails(KeyValuePair<string, string> kvPair,
            AddressType addressType,
            SuppressionOptions suppressionOptions
            )
        {

            CustomerInfoDetails infoDetails = new CustomerInfoDetails();

            using (var conn = new SqlConnection(MciCrDbConnectionString))
            {
                using (var cmd = new SqlCommand("[dbo].[GetCustomerDetails]", conn))
                {
                    try
                    {
                        cmd.Connection.Open();

                        cmd.Parameters.Add(
                           new SqlParameter("@key", SqlDbType.VarChar, 4).SetSqlValue(kvPair.Key)
                        );

                        var param1Value = "";
                        if (!string.IsNullOrWhiteSpace(kvPair.Key))
                        {
                            if (kvPair.Key.Equals(SourceKey.ACTI.ToString(), StringComparison.OrdinalIgnoreCase)
                                || kvPair.Key.Equals(SourceKey.MEMB.ToString(), StringComparison.OrdinalIgnoreCase))
                            {
                                var value = kvPair.Value;
                                if (value.Contains(" "))
                                    value = value.Replace(" ", "+");

                                param1Value = HttpUtility.UrlEncode(value);
                            }
                            else
                            {
                                param1Value = kvPair.Value;
                            }
                        }
                        else
                        {
                            param1Value = kvPair.Value;
                        }
                        cmd.Parameters.Add(new SqlParameter("@value", SqlDbType.VarChar, 40).SetSqlValue(param1Value));
                        cmd.Parameters.Add(new SqlParameter("@addressFormat", SqlDbType.VarChar, 15).SetSqlValue(addressType));

                        // Suppression parameter 
                        cmd.Parameters.Add(
                             new SqlParameter("@ignoreSuppression", SqlDbType.Bit).SetSqlValue(suppressionOptions.IgnoreSuppression.ToInt())
                        );

                        if (!IsCustomerExists(kvPair.Key, kvPair.Value, "PKEY"))
                            return null;

                        cmd.CommandType = CommandType.StoredProcedure;
                        var table = new DataTable();
                        var adapter = new SqlDataAdapter(cmd);
                        var affectedRows = adapter.Fill(table);
                        if (affectedRows == 0)
                            return null;

                        infoDetails = BuildCustomerDetails(table.Rows, addressType);
                    }
                    catch (SqlException ex)
                    {
                        _logger.Error("CustomerDetailsDataAccess: " + "ErrorTag: " + ErrorTagProvider.ErrorTagDatabase + " -- " + ex.Message, ex);
                        throw new Exception(string.Format(DatabaseMessage.DatabaseException, ErrorTagProvider.ErrorTagDatabase));
                    }
                    catch (Exception ex)
                    {
                        _logger.Error("CustomerDetailsDataAccess: " + "ErrorTag: " + ErrorTagProvider.ErrorTagDatabase + " -- " + ex.Message, ex);
                        throw new Exception(string.Format(DatabaseMessage.DatabaseException, ErrorTagProvider.ErrorTagDatabase));
                    }
                }
            }
            return infoDetails;
        }

        private CustomerInfoDetails BuildCustomerDetails(DataRowCollection rows, AddressType addressType)
        {
            CustomerInfoDetails infoDetails = new CustomerInfoDetails();
            try
            {
                foreach (DataRow row in rows)
                {
                    infoDetails.Title = row["title"]?.ToString();
                    infoDetails.FirstName = row["first_name"]?.ToString();
                    infoDetails.Surname = row["surname"]?.ToString();
                    DateTime dateVal;
                    var isValidDate = DateTime.TryParse(row["DOB"].ToString(), out dateVal);

                    if (!isValidDate)
                    {
                        infoDetails.Dob = null;
                    }
                    else
                    {
                        infoDetails.Dob = dateVal;
                    }

                    infoDetails.Gender = row["gender"]?.ToString();
                    infoDetails.SupStatus = row["suppression_status"]?.ToString();
                    infoDetails.Suffix = row["suffix"]?.ToString();
                    if (addressType == AddressType.Correspondence)
                    {
                        var correspondance = new CorrespondenceAddress
                        {
                            Address1 = row["address1"]?.ToString(),
                            Address2 = row["address2"]?.ToString(),
                            Address3 = row["address3"]?.ToString(),
                            Address4 = row["address4"]?.ToString(),
                            Address5 = row["address5"]?.ToString(),
                            Address6 = row["address6"]?.ToString(),
                            Address7 = row["address7"]?.ToString(),
                            Postcode = row["postcode"]?.ToString()
                        };
                        infoDetails.Address = correspondance;
                    }
                    else if (addressType == AddressType.Transactional)
                    {
                        var transactional = new TransactionalAddress
                        {
                            HouseNumber = row["housenumber"]?.ToString(),
                            HouseName = row["housename"]?.ToString(),
                            Street = row["street"]?.ToString(),
                            Street1 = row["street1"]?.ToString(),
                            City = row["city"]?.ToString(),
                            County = row["county"]?.ToString(),
                            Postcode = row["postcode"]?.ToString(),
                        };
                        infoDetails.Address = transactional;

                    }
                    // GN-36 Fix null salutation fields on KeyValuePair calls
                    infoDetails.Salutation = row["salutation"]?.ToString();
                    infoDetails.AddressSalutation = row["address_salutation"]?.ToString();
                    infoDetails.EmploymentStatus = row["employment_status"]?.ToString();
                    infoDetails.Occupation = row["occupation"]?.ToString();
                    infoDetails.MaritialStatus = row["marital_status"]?.ToString();
                    infoDetails.Email = row["email_address"]?.ToString();
                    infoDetails.MobilePhone = row["mobile_phone"]?.ToString();
                    infoDetails.HomePhone = row["home_phone"]?.ToString();
                    infoDetails.WorkPhone = row["work_phone"]?.ToString();
                    infoDetails.SmsPhone = row["sms_number"]?.ToString();
                    infoDetails.FullAddress = row["full_address"]?.ToString();
                }
            }
            catch (Exception ex)
            {
                _logger.Error("CustomerDetailsDataAccess - GetCustomerDetails : " + "ErrorTag: " + ErrorTagProvider.ErrorTagDatabase + " -- " + ex.Message, ex);
                throw new Exception(ex.Message);
            }
            return infoDetails;
        }

        /*
        public CustomerInfoDetails GetCustomerDetails(int customerId, AddressType addressType)
        {
            CustomerInfoDetails list = new CustomerInfoDetails();
            using (var conn = new SqlConnection(MciCrDbConnectionString))
            {
                using (var cmd = new SqlCommand("[MCC].[get_mcc_details]", conn))
                {
                    try
                    {
                        cmd.Connection.Open();
                        var param = new SqlParameter("@p_customer_id", SqlDbType.Int);
                        param.SqlValue = customerId;
                        cmd.Parameters.Add(param);

                        cmd.CommandType = CommandType.StoredProcedure;
                        var table = new DataTable();
                        var adapter = new SqlDataAdapter(cmd);
                        adapter.Fill(table);
                        list = BuildCustomerDetails(table.Rows, addressType);

                    }
                    catch (SqlException ex)
                    {
                        _logger.Error("CustomerDetailsDataAccess - GetCustomerDetails : " + "ErrorTag: " + ErrorTagProvider.ErrorTagDatabase + " -- " + ex.Message, ex);
                        throw new Exception(ex.Message);
                    }
                    catch (Exception ex)
                    {
                        _logger.Error("CustomerDetailsDataAccess - GetCustomerDetails : " + "ErrorTag: " + ErrorTagProvider.ErrorTagDatabase + " -- " + ex.Message, ex);
                        throw new Exception(ex.Message);
                    }
                }
            }
            return list;
        }
        */

        private bool IsCustomerExists(string sourceKey, string custRef, string targetKey)
        {
            bool result = true;

            try
            {
                var customerGIndexQuery = "SELECT * FROM [dbo].[gindx_func](@sourceKey,@custRef,@targetKey)";
                using (SqlConnection conn = new SqlConnection(MciCrDbConnectionString))
                {
                    conn.Open();

                    if (sourceKey.Equals(SourceKey.ACTI.ToString(), StringComparison.OrdinalIgnoreCase)
                        || sourceKey.Equals(SourceKey.MEMB.ToString(), StringComparison.OrdinalIgnoreCase))
                    {
                        
                        if (custRef.Contains(" "))
                            custRef = custRef.Replace(" ", "+");

                        custRef = HttpUtility.UrlEncode(custRef);
                    }

                    SqlCommand command = new SqlCommand(customerGIndexQuery, conn) { CommandType = CommandType.Text };

                    command.Parameters.Add(new SqlParameter("sourceKey", sourceKey));
                    command.Parameters.Add(new SqlParameter("custRef", custRef));
                    command.Parameters.Add(new SqlParameter("targetKey", targetKey));

                    

                    var adapter = new SqlDataAdapter(command);
                    DataSet ds = new DataSet();
                    adapter.Fill(ds);
                    if (ds?.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            if (string.Compare("Customer Reference not found", row[3].ToString().Trim(),
                                    StringComparison.CurrentCultureIgnoreCase) == 0)
                            {
                                result = false;
                            }
                        }

                    }
                    else
                        result = false;
                }
            }
            catch (Exception ex)
            {
                _logger.Error("GetCustomerAllIndexKeys: " + "ErrorTag: " + ErrorTagProvider.ErrorTagDatabase + " -- " + ex.Message, ex);
                throw new Exception(string.Format(DatabaseMessage.DatabaseException, ErrorTagProvider.ErrorTagDatabase));
            }
            return result;
        }

        public List<CustomerIndexResult> GetCustomerAllIndexKeys(string sourceKey, string custRef, string targetKey)
        {
            List<CustomerIndexResult> customerIndexList = null;

            try
            {
                var customerGIndexQuery = "SELECT * FROM [dbo].[gindx_func](@sourceKey,@custRef,@targetKey)";
                using (SqlConnection conn = new SqlConnection(MciCrDbConnectionString))
                {
                    conn.Open();

                    SqlCommand command = new SqlCommand(customerGIndexQuery, conn) { CommandType = CommandType.Text };

                    command.Parameters.Add(new SqlParameter("sourceKey", sourceKey));
                    command.Parameters.Add(new SqlParameter("custRef", custRef));
                    command.Parameters.Add(new SqlParameter("targetKey", targetKey));

                    var adapter = new SqlDataAdapter(command);
                    DataSet ds = new DataSet();
                    adapter.Fill(ds);
                    customerIndexList = BuildCustomerIndex(ds.Tables[0]);
                }
            }
            catch (Exception ex)
            {
                _logger.Error("GetCustomerAllIndexKeys: " + "ErrorTag: " + ErrorTagProvider.ErrorTagDatabase + " -- " + ex.Message, ex);
                throw new Exception(string.Format(DatabaseMessage.DatabaseException, ErrorTagProvider.ErrorTagDatabase));
            }
            return customerIndexList;
        }

        private List<CustomerIndexResult> BuildCustomerIndex(DataTable table)
        {
            List<CustomerIndexResult> lists = new List<CustomerIndexResult>();
            try
            {
                foreach (DataRow row in table.Rows)
                {
                    var list = new CustomerIndexResult();

                    list.CustomerId = int.Parse(row[0].ToString());
                    list.GroupCode = row[1].ToString();
                    list.SourceKey = row[2].ToString();
                    list.Keys = row[3].ToString();
                    lists.Add(list);
                }
            }
            catch (Exception ex)
            {
                _logger.Error("BuildCustomerIndex: " + "ErrorTag: " + ErrorTagProvider.ErrorTagDatabase + " -- " + ex.Message, ex);
                throw new Exception(string.Format(DatabaseMessage.DatabaseException, ErrorTagProvider.ErrorTagDatabase));
            }

            return lists;
        }


    }
}
