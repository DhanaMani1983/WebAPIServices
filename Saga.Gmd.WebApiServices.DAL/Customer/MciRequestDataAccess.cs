using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization;
using log4net;
using Saga.Gmd.WebApiServices.Common;
using Saga.Gmd.WebApiServices.DAL.Extensions;
using Saga.Gmd.WebApiServices.DAL.MailingHistory;
using Saga.Gmd.WebApiServices.Models;
using Saga.Gmd.WebApiServices.Models.Customer;
using Saga.Gmd.WebApiServices.Models.Security;

namespace Saga.Gmd.WebApiServices.DAL.Customer
{
    public class MciRequestDataAccess : IMciRequestDataAccess
    {
        private readonly ILog _logger;
        private string MciCrDbConnectionString { get; }
        private readonly IMailingHistoryDataAccess _mailingHistoryDataAccess;
        public bool CanDump { get; }
        
        public MciRequestDataAccess(ILog logger, IMailingHistoryDataAccess mailingHistoryDataAccess)
        {
            _logger = logger;
            _mailingHistoryDataAccess = mailingHistoryDataAccess;
            MciCrDbConnectionString = ConfigurationManager.AppSettings["MciCrConnection"];

            bool enableDump;
            bool.TryParse(ConfigurationManager.AppSettings["EnableObjectDump"], out enableDump);
            CanDump = enableDump;
            _logger = logger;
        }

        public List<CustomerIndexResult> GetCustomerAllIndexKeys(string sourceKey, string custRef, string targetKey)
        {
            List<CustomerIndexResult> customerIndexList = null;

            try
            {
                if (!string.IsNullOrWhiteSpace(sourceKey))
                {
                    if (sourceKey.Equals(SourceKey.ACTI.ToString(), StringComparison.OrdinalIgnoreCase)
                        || sourceKey.Equals(SourceKey.MEMB.ToString(), StringComparison.OrdinalIgnoreCase))
                    {
                        if (custRef.Contains(" "))
                            custRef = custRef.Replace(" ", "+");

                        custRef = System.Web.HttpUtility.UrlEncode(custRef);
                    }
                }
                var customerGIndexQuery = "SELECT * FROM [dbo].[gindx_func](@sourceKey,@custRef,@targetKey)";
                using (SqlConnection conn = new SqlConnection(MciCrDbConnectionString))
                {
                    conn.Open();

                    SqlCommand command = new SqlCommand(customerGIndexQuery, conn) {CommandType = CommandType.Text};

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
                                return new List<CustomerIndexResult>
                                {
                                    new CustomerIndexResult() {CustomerFound = false, CustomerId = 0}
                                };
                            }
                        }

                        customerIndexList = BuildCustomerIndex(ds.Tables[0]);
                    }
                    else
                        return new List<CustomerIndexResult>
                        {
                            new CustomerIndexResult() {CustomerFound = false, CustomerId = 0}
                        };
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
                    list.CustomerFound = true;
                    lists.Add(list);
                }
            }
            catch (SqlException ex)
            {
                _logger.Error(
                    "BuildCustomerIndex : ErrorTag: " + ErrorTagProvider.ErrorTagDatabase + " -- " +
                    ex.Message, ex);
                throw new DatabaseException(string.Format(DatabaseMessage.DatabaseException,
                    ErrorTagProvider.ErrorTagDatabase));
            }
            catch (Exception ex)
            {
                _logger.Error("BuildCustomerIndex: " + "ErrorTag: " + ErrorTagProvider.ErrorTagDatabase + " -- " + ex.Message, ex);
                throw new Exception(string.Format(DatabaseMessage.DatabaseException, ErrorTagProvider.ErrorTagDatabase));
            }

            return lists;
        }

        public List<ClientScopes> GetClientScopes(string clientId)
        {
            List<ClientScopes> scopes = new List<ClientScopes>();
            try
            {
                using (var conn = new SqlConnection(MciCrDbConnectionString))
                {
                    using (var cmd = new SqlCommand("[dbo].[get_client_all_scope]", conn))
                    {
                        try
                        {
                            cmd.Connection.Open();
                            var param = new SqlParameter("@clientId", SqlDbType.VarChar, 40);
                            param.SqlValue = clientId;
                            cmd.Parameters.Add(param);

                            cmd.CommandType = CommandType.StoredProcedure;
                            var ds = new DataSet();
                            var adapter = new SqlDataAdapter(cmd);
                            adapter.Fill(ds);
                            scopes = BuildClientScopes(ds);
                            // scopes = BuildClientScopes(reader);
                        }

                        catch (SqlException ex)
                        {
                            _logger.Error("GetClientScopes: " + "ErrorTag: " + ErrorTagProvider.ErrorTagDatabase + " -- " + ex.Message, ex);
                            throw new DatabaseException(string.Format(DatabaseMessage.DatabaseException, ErrorTagProvider.ErrorTagDatabase));


                        }
                    }

                }
            }
            catch (SqlException ex)
            {
                _logger.Error("GetClientScopes: " + "ErrorTag: " + ErrorTagProvider.ErrorTagDatabase + " -- " + ex.Message, ex);
                throw new DatabaseException(string.Format(DatabaseMessage.DatabaseException, ErrorTagProvider.ErrorTagDatabase));
            }
            catch (Exception ex)
            {
                _logger.Error("GetClientScopes: " + "ErrorTag: " + ErrorTagProvider.ErrorTagDatabase + " -- " + ex.Message, ex);
                throw new Exception(string.Format(DatabaseMessage.DatabaseException, ErrorTagProvider.ErrorTagDatabase));

            }
            return scopes;
        }



        public int? GetPersistantKey(NameAndAddress nameAndAddress)
        {
            CustomerMatchParameter param = new CustomerMatchParameter();
            param.FirstName = nameAndAddress.FirstName;
            param.Surname = nameAndAddress.Surname;
            param.Dob = nameAndAddress.Dob;
            param.Title = nameAndAddress.Title;
            param.Email = nameAndAddress.Email;
            param.Phone = nameAndAddress.Phone;
            param.Address1 = nameAndAddress.Address.Address1;
            param.Address2 = nameAndAddress.Address.Address2;
            param.Address3 = nameAndAddress.Address.Address3;
            param.Address4 = nameAndAddress.Address.Address4;
            param.Postcode = nameAndAddress.Address.Postcode;

            // GICTS-3 Customer Matching on first character : Don't set a default match type
            //
            param.MatchType = nameAndAddress.MatchType;

            // changed for ACSU-113 to use passed value or default to POSTCODE_AND_NAME
            /*
            if (string.IsNullOrEmpty(param.MatchType) == true)
            {
                // default to POSTCODE_AND_NAME
                param.MatchType = MatchType.POSTCODE_AND_NAME.ToString();
            }
            else
            {
                param.MatchType = nameAndAddress.MatchType;
            }
            */

            var matchedCustomers = _mailingHistoryDataAccess.GetMatchingCustomers(param).FirstOrDefault();

            return matchedCustomers?.CustomerId;
        }

        private List<ClientScopes> BuildClientScopes(DataSet data)
        {
            List<ClientScopes> scopes = new List<ClientScopes>();
            var table = data.Tables[0];
            try
            {
                foreach (DataRow row in table.Rows)
                {
                    ClientScopes scope = new ClientScopes();
                    scope.Code = row["Code"].ToString();
                    scope.IsActive = bool.Parse(row["Active"].ToString());
                    scope.Scope = row["ScopeName"].ToString();
                    scope.SecScopeId = int.Parse(row["ScopeId"].ToString());
                    scopes.Add(scope);
                }
            }
            catch (SqlException ex)
            {
                _logger.Error("BuildClientScopes: " + "ErrorTag: " + ErrorTagProvider.ErrorTagDatabase + " -- " + ex.Message, ex);
                throw new DatabaseException(string.Format(DatabaseMessage.DatabaseException, ErrorTagProvider.ErrorTagDatabase));
            }
            catch (Exception ex)
            {
                _logger.Error("BuildClientScopes: " + "ErrorTag: " + ErrorTagProvider.ErrorTagDatabase + " -- " + ex.Message, ex);
                throw new DatabaseException(string.Format(DatabaseMessage.DatabaseException, ErrorTagProvider.ErrorTagDatabase));
            }
            return scopes;
        }

        public long SaveCustomer(Models.Customer.CustomerLoadOrMatch customerLoad)
        {
            int customerId;
            DateTime processedDatetime;
            return SaveCustomer(customerLoad,out customerId, out processedDatetime);
        }

        public long SaveCustomer(Models.Customer.CustomerLoadOrMatch customerLoad, out int customerId, out DateTime processedDatetime)
        {
            long savedId = 0;

           
            string message = DatabaseMessage.CustomerNotCreated;

            using (var conn = new SqlConnection(MciCrDbConnectionString))
            {
                using (var cmd = new SqlCommand("[dbo].[insert_customer_api_landing]", conn))
                {
                    try
                    {
                        cmd.Connection.Open();

                        var param = new SqlParameter("@systemId", SqlDbType.VarChar, 80);
                        param.SqlValue = customerLoad.Customer.SystemId;
                        cmd.Parameters.Add(param);

                        var param1 = new SqlParameter("@systemSource", SqlDbType.VarChar, 50);
                        param1.SqlValue = customerLoad.Customer.SystemSource;
                        cmd.Parameters.Add(param1);

                        var param3 = new SqlParameter("@marketingSource", SqlDbType.VarChar, 50);
                        param3.SqlValue = string.IsNullOrEmpty(customerLoad.Customer.MarketingSource) ? (object)DBNull.Value : customerLoad.Customer.MarketingSource;
                        cmd.Parameters.Add(param3);

                        var param4 = new SqlParameter("@transactionType", SqlDbType.VarChar, 50);
                        param4.SqlValue = string.IsNullOrEmpty(customerLoad.Customer.TransactionType) ? (object)DBNull.Value : customerLoad.Customer.TransactionType;
                        cmd.Parameters.Add(param4);

                        var param5 = new SqlParameter("@transactionBrand", SqlDbType.VarChar, 50);
                        param5.SqlValue = string.IsNullOrEmpty(customerLoad.Customer.TransactionBrand) ? (object)DBNull.Value : customerLoad.Customer.TransactionBrand;
                        cmd.Parameters.Add(param5);

                        var param6 = new SqlParameter("@title", SqlDbType.VarChar, 30);
                        param6.SqlValue = string.IsNullOrEmpty(customerLoad.Customer.Title) ? (object)DBNull.Value : customerLoad.Customer.Title;
                        cmd.Parameters.Add(param6);

                        var param7 = new SqlParameter("@foreName", SqlDbType.VarChar, 50);
                        param7.SqlValue = string.IsNullOrEmpty(customerLoad.Customer.Forename) ? (object)DBNull.Value : customerLoad.Customer.Forename;
                        cmd.Parameters.Add(param7);

                        var param8 = new SqlParameter("@surName", SqlDbType.VarChar, 50);
                        param8.SqlValue = string.IsNullOrEmpty(customerLoad.Customer.Surname) ? (object)DBNull.Value : customerLoad.Customer.Surname;
                        cmd.Parameters.Add(param8);

                        var param9 = new SqlParameter("@suffix", SqlDbType.VarChar, 30);
                        param9.SqlValue = string.IsNullOrEmpty(customerLoad.Customer.Suffix) ? (object)DBNull.Value : customerLoad.Customer.Suffix;
                        cmd.Parameters.Add(param9);

                        var param10 = new SqlParameter("@gender", SqlDbType.VarChar, 20);
                        param10.SqlValue = string.IsNullOrEmpty(customerLoad.Customer.Gender) ? (object)DBNull.Value : customerLoad.Customer.Gender;
                        cmd.Parameters.Add(param10);

                        var param11 = new SqlParameter("@emailAddress", SqlDbType.VarChar, 50);
                        param11.SqlValue = string.IsNullOrEmpty(customerLoad.Customer.EmailAddress) ? (object)DBNull.Value : customerLoad.Customer.EmailAddress;
                        cmd.Parameters.Add(param11);

                        var param12 = new SqlParameter("@EmailMarketingUse", SqlDbType.Bit, 1);
                        param12.SqlValue = customerLoad.Customer.EmailMarketingUse.HasValue ? (object)customerLoad.Customer.EmailMarketingUse : DBNull.Value;
                        cmd.Parameters.Add(param12);

                        var param13 = new SqlParameter("@emailTransactionalUse", SqlDbType.Bit, 1);
                        param13.SqlValue = customerLoad.Customer.EmailTransactionalUse.HasValue ? (object)customerLoad.Customer.EmailTransactionalUse : DBNull.Value;
                        cmd.Parameters.Add(param13);

                        var param14 = new SqlParameter("@dateofbirth", SqlDbType.Date);
                        param14.SqlValue = customerLoad.Customer.DateOfBirth.HasValue ? (object)customerLoad.Customer.DateOfBirth.Value : DBNull.Value;
                        cmd.Parameters.Add(param14);

                        var param15 = new SqlParameter("@telephoneMobile", SqlDbType.VarChar, 15);
                        param15.SqlValue = string.IsNullOrEmpty(customerLoad.Customer.TelephoneMobile) ? (object)DBNull.Value : customerLoad.Customer.TelephoneMobile;
                        cmd.Parameters.Add(param15);

                        var param16 = new SqlParameter("@telephoneHome", SqlDbType.VarChar, 15);
                        param16.SqlValue = string.IsNullOrEmpty(customerLoad.Customer.TelephoneHome) ? (object)DBNull.Value : customerLoad.Customer.TelephoneHome;
                        cmd.Parameters.Add(param16);

                        var param17 = new SqlParameter("@deleted", SqlDbType.Bit, 1);
                        param17.SqlValue = customerLoad.Customer.Deleted.HasValue ? (object)customerLoad.Customer.Deleted.Value : DBNull.Value;
                        cmd.Parameters.Add(param17);

                        if (customerLoad.Customer.Address != null)
                        {
                            var param19 = new SqlParameter("@housename", SqlDbType.VarChar, 50);
                            param19.SqlValue = string.IsNullOrEmpty(customerLoad.Customer.Address.HouseName)
                                ? (object)DBNull.Value
                                : customerLoad.Customer.Address.HouseName;
                            cmd.Parameters.Add(param19);

                            var param20 = new SqlParameter("@housenumber", SqlDbType.VarChar, 10);
                            param20.SqlValue = string.IsNullOrEmpty(customerLoad.Customer.Address.HouseNumber)
                                ? (object)DBNull.Value
                                : customerLoad.Customer.Address.HouseNumber;
                            cmd.Parameters.Add(param20);

                            var param20A = new SqlParameter("@street", SqlDbType.VarChar, 100);
                            param20A.SqlValue = string.IsNullOrEmpty(customerLoad.Customer.Address.Street)
                                ? (object)DBNull.Value
                                : customerLoad.Customer.Address.Street;
                            cmd.Parameters.Add(param20A);

                            var param20B = new SqlParameter("@locality", SqlDbType.VarChar, 100);
                            param20B.SqlValue = string.IsNullOrEmpty(customerLoad.Customer.Address.Street1)
                                ? (object)DBNull.Value
                                : customerLoad.Customer.Address.Street1;
                            cmd.Parameters.Add(param20B);

                            var param20C = new SqlParameter("@city", SqlDbType.VarChar, 100);
                            param20C.SqlValue = string.IsNullOrEmpty(customerLoad.Customer.Address.City)
                                ? (object)DBNull.Value
                                : customerLoad.Customer.Address.City;
                            cmd.Parameters.Add(param20C);

                            var param20D = new SqlParameter("@county", SqlDbType.VarChar, 100);
                            param20D.SqlValue = string.IsNullOrEmpty(customerLoad.Customer.Address.County)
                                ? (object)DBNull.Value
                                : customerLoad.Customer.Address.County;
                            cmd.Parameters.Add(param20D);

                            var param20E = new SqlParameter("@country", SqlDbType.VarChar, 100);
                            param20E.SqlValue = string.IsNullOrEmpty(customerLoad.Customer.Address.Country)
                                ? (object)DBNull.Value
                                : customerLoad.Customer.Address.Country;
                            cmd.Parameters.Add(param20E);

                            var param22 = new SqlParameter("@postcode", SqlDbType.VarChar, 8);
                            param22.SqlValue = string.IsNullOrEmpty(customerLoad.Customer.Address.Postcode)
                                ? (object)DBNull.Value
                                : customerLoad.Customer.Address.Postcode;
                            cmd.Parameters.Add(param22);
                        }
                        //else
                        //{
                        //    //this block is just for postman or fiddler testing, if request didn't send address block then, app should not break.
                        //    var param19 = new SqlParameter("@housename", SqlDbType.VarChar, 100);
                        //    param19.SqlValue = DBNull.Value;
                        //    cmd.Parameters.Add(param19);

                        //    var param20 = new SqlParameter("@housenumber", SqlDbType.VarChar, 100);
                        //    param20.SqlValue = DBNull.Value;
                        //    cmd.Parameters.Add(param20);

                        //    var param21 = new SqlParameter("@street", SqlDbType.VarChar, 100);
                        //    param21.SqlValue = DBNull.Value;
                        //    cmd.Parameters.Add(param21);

                        //    var param21A = new SqlParameter("@street1", SqlDbType.VarChar, 100);
                        //    param21A.SqlValue = DBNull.Value;
                        //    cmd.Parameters.Add(param21A);

                        //    var param21B = new SqlParameter("@city", SqlDbType.VarChar, 100);
                        //    param21B.SqlValue = DBNull.Value;
                        //    cmd.Parameters.Add(param21B);

                        //    var param21C = new SqlParameter("@county", SqlDbType.VarChar, 100);
                        //    param21C.SqlValue = DBNull.Value;
                        //    cmd.Parameters.Add(param21C);

                        //    var param21D = new SqlParameter("@country", SqlDbType.VarChar, 100);
                        //    param21D.SqlValue = DBNull.Value;
                        //    cmd.Parameters.Add(param21D); 

                        //    var param22 = new SqlParameter("@postcode", SqlDbType.VarChar, 8);
                        //    param22.SqlValue = DBNull.Value;
                        //    cmd.Parameters.Add(param22);
                        //}

                        var param23 = new SqlParameter("@employmentStatus", SqlDbType.VarChar, 30);
                        param23.SqlValue = string.IsNullOrEmpty(customerLoad.Customer.EmploymentStatus) ? (object)DBNull.Value : customerLoad.Customer.EmploymentStatus;
                        cmd.Parameters.Add(param23);

                        var param24 = new SqlParameter("@occupation", SqlDbType.VarChar, 30);
                        param24.SqlValue = string.IsNullOrEmpty(customerLoad.Customer.Occupation) ? (object)DBNull.Value : customerLoad.Customer.Occupation;
                        cmd.Parameters.Add(param24);

                        var param25 = new SqlParameter("@maritalStatus", SqlDbType.VarChar, 30);
                        param25.SqlValue = string.IsNullOrEmpty(customerLoad.Customer.MaritalStatus) ? (object)DBNull.Value : customerLoad.Customer.MaritalStatus;
                        cmd.Parameters.Add(param25);

                        var param26 = new SqlParameter("@staffFlag", SqlDbType.Bit, 1);
                        param26.SqlValue = customerLoad.Customer.StaffFlag.HasValue ? (object)customerLoad.Customer.StaffFlag.Value : DBNull.Value;
                        cmd.Parameters.Add(param26);

                        var param27 = new SqlParameter("@retirementDate", SqlDbType.Date);
                        param27.SqlValue = customerLoad.Customer.RetirementDate.HasValue ? (object)customerLoad.Customer.RetirementDate.Value : DBNull.Value;
                        cmd.Parameters.Add(param27);

                        var param28 = new SqlParameter("@isVerified", SqlDbType.Bit, 1);
                        param28.SqlValue = customerLoad.Customer.IsVerified.HasValue ? (object)customerLoad.Customer.IsVerified.Value : DBNull.Value;
                        cmd.Parameters.Add(param28);

                        var param29 = new SqlParameter("@createdDate", SqlDbType.DateTime);
                        param29.SqlValue = DateTime.UtcNow;
                        cmd.Parameters.Add(param29);

                        var param30 = new SqlParameter("@updatedDate", SqlDbType.DateTime);
                        param30.SqlValue = customerLoad.Customer.UpdatedDate.HasValue ? (object)customerLoad.Customer.UpdatedDate.Value : DBNull.Value;
                        cmd.Parameters.Add(param30);

                        var param31 = new SqlParameter("@deletedDate", SqlDbType.DateTime);
                        param31.SqlValue = customerLoad.Customer.DeletedDate.HasValue ? (object)customerLoad.Customer.DeletedDate.Value : DBNull.Value;
                        cmd.Parameters.Add(param31);

                        var param32 = new SqlParameter("@custRefStatus", SqlDbType.VarChar, 50);

                        // Sid Martin says leave value as null. The possible values are determined by systemSource and need to match with MCI_INDEX_STATUS
                        //if (customerLoad.Customer.IsVerified != null && (customerLoad.Customer.IsVerified.Value && !customerLoad.Customer.Deleted.GetValueOrDefault()))
                        //{
                        //    param32.SqlValue = "Active-Verified";
                        //}

                        //else if (!customerLoad.Customer.IsVerified.GetValueOrDefault() && !customerLoad.Customer.Deleted.GetValueOrDefault())
                        //{
                        //    param32.SqlValue = "Active-Unverified";
                        //}
                        //else if (customerLoad.Customer.IsVerified.GetValueOrDefault() && customerLoad.Customer.Deleted.GetValueOrDefault())
                        //{
                        //    param32.SqlValue = "Inactive-Unverified";
                        //}
                        //else
                        //{
                        //    param32.SqlValue = "Inactive-Unverified";
                        //}

                        param32.SqlValue = (object)DBNull.Value;

                        cmd.Parameters.Add(param32);

                        cmd.CommandType = CommandType.StoredProcedure;

                        var result = cmd.ExecuteScalar();

                        savedId = Convert.ToInt64((decimal)result);
                        int customeridOutput = 0;
                        DateTime processeddateOutput = DateTime.MinValue;
                                                  
                        if (savedId > 0)
                        {
                            message = DatabaseMessage.CustomerCreated;

                            ProcessCustomerLoad(savedId, customerLoad, out customeridOutput, out processeddateOutput);
                        }

                        customerId = customeridOutput;
                        processedDatetime = processeddateOutput;
                    }
                    catch (SqlException ex)
                    {

                        var d = ex.Data.Keys;
                        _logger.Error("ErrorTag: " + ErrorTagProvider.ErrorTagDatabase + " -- " + ex.Message, ex);
                        DumpModel.Dump(cmd.CommandText, _logger, CanDump);
                        DumpModel.Dump(customerLoad, _logger, CanDump);
                        throw new Exception(string.Format(DatabaseMessage.DatabaseException, ErrorTagProvider.ErrorTagDatabase));

                    }
                    catch (Exception ex)
                    {

                        if (ex.Message == "The SqlParameter is already contained by another SqlParameterCollection")
                        {
                            _logger.Error("ErrorTag: " + ErrorTagProvider.ErrorTagDatabase + "-- A L E R T >>>>>>>>>>>> USER TRYING TO HACK THE DATABASE ! A C T I O N   R E Q U I R E D !!<<<<<<<<<<<<<<<", ex);
                        }

                        _logger.Error("ErrorTag: " + ErrorTagProvider.ErrorTagDatabase + " -- " + ex.Message, ex);
                        DumpModel.Dump(cmd.CommandText, _logger, CanDump);
                        DumpModel.Dump(customerLoad, _logger, CanDump);
                        throw new DatabaseException(string.Format(DatabaseMessage.DatabaseException, ErrorTagProvider.ErrorTagDatabase));
                    }
                }
            }

            return savedId;
        }

        /// <summary>
        /// Invoke a stored procedure that processes the newly landed customer record 
        /// </summary>
        /// <param name="landedRecordId"></param>
        /// <param name="customerLoad"></param>
        /// <param name="customerId"></param>
        /// <param name="processedDatetime"></param>
        /// <returns></returns>
        public bool ProcessCustomerLoad(long landedRecordId, Models.Customer.CustomerLoadOrMatch customerLoad, out int customerId, out DateTime processedDatetime)
        {
            int rowsAffected = 0;

            using (var conn = new SqlConnection(MciCrDbConnectionString))
            {
                using (var cmd = new SqlCommand("[dbo].[mci_customer_api_load]", conn))
                {
                    try
                    {
                        cmd.Connection.Open();

                        // Marital status parameter cross-wiring defect : replace named param binding with fluent assignment below...
                        // GS-105 
                        cmd.Parameters
                            .AddParameter(new SqlParameter("@p_recordid", SqlDbType.Int).SetSqlValue(landedRecordId))
                            .AddParameter(new SqlParameter("@p_systemid", SqlDbType.VarChar, 80).SetSqlValue(customerLoad.Customer.SystemId))
                            .AddParameter(new SqlParameter("@p_systemSource", SqlDbType.VarChar, 50).SetSqlValue(customerLoad.Customer.SystemSource) )
                            .AddParameter(new SqlParameter("@p_custRefStatus", SqlDbType.VarChar, 50).SetSqlValue((object)DBNull.Value))
                            .AddParameter(new SqlParameter("@p_brand", SqlDbType.VarChar, 50).SetSqlValue(string.IsNullOrEmpty(customerLoad.Customer.TransactionBrand) ? (object)DBNull.Value : customerLoad.Customer.TransactionBrand ))
                            .AddParameter(new SqlParameter("@p_title", SqlDbType.VarChar, 30).SetSqlValue(string.IsNullOrEmpty(customerLoad.Customer.Title) ? (object)DBNull.Value : customerLoad.Customer.Title ))
                            .AddParameter(new SqlParameter("@p_forename", SqlDbType.VarChar, 50).SetSqlValue(string.IsNullOrEmpty(customerLoad.Customer.Forename) ? (object)DBNull.Value : customerLoad.Customer.Forename))
                            .AddParameter(new SqlParameter("@p_surname", SqlDbType.VarChar, 50).SetSqlValue(string.IsNullOrEmpty(customerLoad.Customer.Surname) ? (object)DBNull.Value : customerLoad.Customer.Surname))
                            .AddParameter(new SqlParameter("@p_dob", SqlDbType.Date).SetSqlValue( (object)customerLoad.Customer.DateOfBirth ??  DBNull.Value ))
                            .AddParameter(new SqlParameter("@p_suffix", SqlDbType.VarChar, 30).SetSqlValue( string.IsNullOrEmpty(customerLoad.Customer.Suffix) ? (object)DBNull.Value : customerLoad.Customer.Suffix ))
                            .AddParameter(new SqlParameter("@p_gender", SqlDbType.VarChar, 20).SetSqlValue(customerLoad.Customer.Gender))
                            .AddParameter(new SqlParameter("@p_maritalStatus", SqlDbType.VarChar, 30).SetSqlValue(string.IsNullOrEmpty(customerLoad.Customer.MaritalStatus) ? (object)DBNull.Value : customerLoad.Customer.MaritalStatus))
                            .AddParameter(new SqlParameter("@p_employmentstatus", SqlDbType.VarChar, 30).SetSqlValue(string.IsNullOrEmpty(customerLoad.Customer.EmploymentStatus) ? (object)DBNull.Value : customerLoad.Customer.EmploymentStatus))
                            .AddParameter(new SqlParameter("@p_occupation", SqlDbType.VarChar, 30).SetSqlValue(string.IsNullOrEmpty(customerLoad.Customer.Occupation) ? (object)DBNull.Value : customerLoad.Customer.Occupation))
                            .AddParameter(new SqlParameter("@p_emailAddress", SqlDbType.VarChar, 50).SetSqlValue(string.IsNullOrEmpty(customerLoad.Customer.EmailAddress) ? (object)DBNull.Value : customerLoad.Customer.EmailAddress ))
                            .AddParameter(new SqlParameter("@p_marketinguse", SqlDbType.Bit, 1).SetSqlValue( (object)customerLoad.Customer.EmailMarketingUse ?? DBNull.Value ))
                            .AddParameter(new SqlParameter("@p_telephonemobile", SqlDbType.VarChar, 15).SetSqlValue(string.IsNullOrEmpty(customerLoad.Customer.TelephoneMobile) ? (object)DBNull.Value : customerLoad.Customer.TelephoneMobile ))
                            .AddParameter(new SqlParameter("@p_telephonehome", SqlDbType.VarChar, 15).SetSqlValue(string.IsNullOrEmpty(customerLoad.Customer.TelephoneHome) ? (object)DBNull.Value : customerLoad.Customer.TelephoneHome));
                            if (customerLoad.Customer.Address != null)
                            {
                                cmd.Parameters
                                    .AddParameter(new SqlParameter("@p_postcode", SqlDbType.VarChar, 8).SetSqlValue(string.IsNullOrEmpty(customerLoad.Customer.Address.Postcode) ? (object)DBNull.Value : customerLoad.Customer.Address.Postcode))
                                    .AddParameter(new SqlParameter("@p_housename", SqlDbType.VarChar, 50).SetSqlValue(string.IsNullOrEmpty(customerLoad.Customer.Address.HouseName) ? (object)DBNull.Value : customerLoad.Customer.Address.HouseName))
                                    .AddParameter(new SqlParameter("@p_housenumber", SqlDbType.VarChar, 10).SetSqlValue(string.IsNullOrEmpty(customerLoad.Customer.Address.HouseNumber) ? (object)DBNull.Value : customerLoad.Customer.Address.HouseNumber ))
                                    .AddParameter(new SqlParameter("@p_street", SqlDbType.VarChar, 100).SetSqlValue(string.IsNullOrEmpty(customerLoad.Customer.Address.Street) ? (object)DBNull.Value : customerLoad.Customer.Address.Street))
                                    .AddParameter(new SqlParameter("@p_locality", SqlDbType.VarChar, 100).SetSqlValue(string.IsNullOrEmpty(customerLoad.Customer.Address.Street1) ? (object)DBNull.Value : customerLoad.Customer.Address.Street1 ))
                                    .AddParameter(new SqlParameter("@p_city", SqlDbType.VarChar, 100).SetSqlValue(string.IsNullOrEmpty(customerLoad.Customer.Address.City) ? (object)DBNull.Value : customerLoad.Customer.Address.City))
                                    .AddParameter(new SqlParameter("@p_county", SqlDbType.VarChar, 100).SetSqlValue(string.IsNullOrEmpty(customerLoad.Customer.Address.County) ? (object)DBNull.Value : customerLoad.Customer.Address.County ))
                                    .AddParameter(new SqlParameter("@p_country", SqlDbType.VarChar, 100).SetSqlValue(string.IsNullOrEmpty(customerLoad.Customer.Address.Country) ? (object)DBNull.Value : customerLoad.Customer.Address.Country ));
                            }

                        cmd.Parameters.AddParameter(new SqlParameter("@p_customerId", SqlDbType.Int).SetDirection(ParameterDirection.Output)  )
                            .AddParameter(new SqlParameter("@p_processeddate", SqlDbType.DateTime).SetDirection(ParameterDirection.Output)  );

                        cmd.CommandType = CommandType.StoredProcedure;

                        var result = cmd.ExecuteNonQuery();

                        customerId = (int) cmd.Parameters["@p_customerId"].Value;
                        processedDatetime = (DateTime) cmd.Parameters["@p_processeddate"].Value;

                        rowsAffected = Convert.ToInt32((int)result);
                    }

                    catch (SqlException ex)
                    {
                        var d = ex.Data.Keys;

                        _logger.Error("ErrorTag: " + ErrorTagProvider.ErrorTagDatabase + " -- " + ex.Message, ex);

                        throw new DatabaseException(string.Format(DatabaseMessage.DatabaseException, ErrorTagProvider.ErrorTagDatabase));
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message == "The SqlParameter is already contained by another SqlParameterCollection")
                        {
                            _logger.Error("ErrorTag: " + ErrorTagProvider.ErrorTagDatabase + "-- A L E R T >>>>>>>>>>>> USER TRYING TO HACK THE DATABASE ! A C T I O N   R E Q U I R E D !!<<<<<<<<<<<<<<<", ex);
                        }

                        _logger.Error("ErrorTag: " + ErrorTagProvider.ErrorTagDatabase + " -- " + ex.Message, ex);

                        throw new DatabaseException(string.Format(DatabaseMessage.DatabaseException, ErrorTagProvider.ErrorTagDatabase));
                    }
                }
            }

            return rowsAffected > 0;
        }











    }
}
