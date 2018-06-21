using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Saga.Gmd.WebApiServices.Models.Customer;
using log4net;
using Saga.Gmd.WebApiServices.Common;
using System.Data.SqlClient;
using System.Data;
using log4net.Util;
using Microsoft.SqlServer.Server;
using Saga.Gmd.WebApiServices.DAL.Extensions;

namespace Saga.Gmd.WebApiServices.DAL.DataAccessHandlers
{
    public class CustomerMatchDataAccessHandler : ICustomerMatchDataAccessHandler
    {

        private readonly ILog _logger;
        private readonly bool _writeParameterValuesToLog;
        private readonly string _mciConnectionString;

        /// <summary>
        /// Supported match types
        /// </summary>
        /// <returns></returns>
        protected string[] MatchTypes => EnumHelpers.GetEnumValues<MatchType>().Select(mt => mt.ToLowerString() ).ToArray();

        /// <summary>
        /// Match types for which default rules (initial matching)
        /// </summary>
        protected string[] MatchTypesOverrideMatchRules => EnumHelpers.GetEnumValues<MatchTypeOverrideRules>().Select(mt => mt.ToLowerString() ).ToArray();

        public CustomerMatchDataAccessHandler( string mciConnectionString, ILog logger, bool writeParameterValuesToLog)
        {
            _logger = logger;
            _writeParameterValuesToLog = writeParameterValuesToLog;
            _mciConnectionString = mciConnectionString;
        }

        public  List<MatchedCustomer> MatchCustomers(CustomerMatchParameter customerMatchParameter)
        { 

            List<MatchedCustomer> customerIds = null;

            // Known specific match type which is not listed in the list of MatchTypes for which default 
            // matching rules should be applied             
            if (MatchTypes.Contains(customerMatchParameter.MatchType)
                &&
                !MatchTypesOverrideMatchRules.Contains(customerMatchParameter.MatchType)
                )
            {
                customerIds = MatchWithSpecifiedMatchType(customerMatchParameter);
            }
            else  // Unknown match type - let the Db routines decide 
            {
                customerIds = MatchWithDefaultMatchTypeRules(customerMatchParameter);
            }

            return customerIds;

        }

        /// <summary>
        /// Call new stored proc 
        /// </summary>
        /// <param name="customerMatchParameter"></param>
        /// <returns></returns>
        private List<MatchedCustomer> MatchWithDefaultMatchTypeRules(CustomerMatchParameter customerMatchParameter)
        {

            string errorLogPrefix = "CustomerMatchDataAccessHandler.MatchWithDefaultMatchTypeRules(): ErrorTag: ";
            List<MatchedCustomer> customerIds = new List<MatchedCustomer>();

            using (var conn = new SqlConnection(_mciConnectionString))
            {
                using (var cmd = new SqlCommand("dbo.match_customer_default_rules", conn))
                {
                    try
                    {
                        cmd.Connection.Open();

                        cmd.Parameters.AddParameter(new SqlParameter("@p_title",
                                customerMatchParameter.Title ?? (object) DBNull.Value))
                            .AddParameter(new SqlParameter("@p_first_name",
                                customerMatchParameter.FirstName ?? (object) DBNull.Value))
                            .AddParameter(new SqlParameter("@p_surname",
                                customerMatchParameter.Surname ?? (object) DBNull.Value))
                            .AddParameter(new SqlParameter("@p_dob",
                                PrepareDobString(customerMatchParameter.Dob) ?? (object) DBNull.Value))
                            .AddParameter(new SqlParameter("@p_postcode",
                                customerMatchParameter.Postcode ?? (object) DBNull.Value))
                            .AddParameter(new SqlParameter("@p_housename", (object) DBNull.Value)) // Available to us ?
                            .AddParameter(new SqlParameter("@p_housenumber", (object) DBNull.Value)) // 
                            .AddParameter(new SqlParameter("@p_address1",
                                customerMatchParameter.Address1 ?? (object) DBNull.Value))
                            .AddParameter(new SqlParameter("@p_address2",
                                customerMatchParameter.Address2 ?? (object) DBNull.Value))
                            .AddParameter(new SqlParameter("@p_address3",
                                customerMatchParameter.Address3 ?? (object) DBNull.Value))
                            .AddParameter(new SqlParameter("@p_address4",
                                customerMatchParameter.Address4 ?? (object) DBNull.Value))
                            .AddParameter(
                                new SqlParameter("@p_customer_id", SqlDbType.Int).SetDirection(
                                    ParameterDirection.Output));


                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.ExecuteNonQuery();

                        var customerId =
                            cmd.Parameters["@p_customer_id"].Value.ToString()
                                .ToIntOrDefault(); // Ensure we handle empty string results ...

                        if (customerId > 0)
                        {
                            customerIds.Add(new MatchedCustomer {CustomerId = customerId});
                        }

                    }
                    catch (SqlException ex)
                    {
                        LogException(errorLogPrefix, customerMatchParameter, ex);

                        throw new Exception(string.Format(DatabaseMessage.DatabaseException,
                            ErrorTagProvider.ErrorTagDatabase));
                    }

                    catch (Exception ex)
                    {
                        LogException(errorLogPrefix, customerMatchParameter, ex);

                        if (_writeParameterValuesToLog)
                        {
                            LogErrorParrameters(customerMatchParameter);

                        }

                        throw new Exception(string.Format(DatabaseMessage.DatabaseException,
                            ErrorTagProvider.ErrorTagDatabase));
                    }
                }
            }

            return customerIds;
        }

        private void LogException( string messagePrefix, CustomerMatchParameter customerMatchParameter, Exception ex)
        {
            _logger.Error(
                messagePrefix +
                ErrorTagProvider.ErrorTagDatabase + " -- " + ex.Message, ex);
            if (_writeParameterValuesToLog)
            {
                LogErrorParrameters(customerMatchParameter);

            }
        }

        private string PrepareDobString(DateTime? dobParam)
        {
            DateTime? dateOfBirth = DateTimeExtensions.GetValidDate(dobParam) ?? null;
            var dob = dateOfBirth != null ? dateOfBirth.ToReturnIsoNullDateIfValid() : null;

            return dob;
        }


        // TODO : Test refactored match calls from this handler class
        /// <summary>
        /// Call match_customers_func for a specified match type 
        /// </summary>
        /// <param name="customerMatchParameter"></param>
        /// <returns></returns>
        private List<MatchedCustomer> MatchWithSpecifiedMatchType(CustomerMatchParameter customerMatchParameter)
        {
            List<MatchedCustomer> customerIds;

            try
            {

                var matchCustomerFunc = "select customer_id from [dbo].[match_customers_func](@matchtype,@title,@firstname,@surname,@dob,@email,@phone,@postcode,@address1,@address2,@address3,@address4)";

                using (SqlConnection conn = new SqlConnection(_mciConnectionString))
                {
                    conn.Open();

                    SqlCommand command = new SqlCommand(matchCustomerFunc, conn) { CommandType = CommandType.Text };

                    command.Parameters.AddParameter(new SqlParameter("@matchtype", customerMatchParameter.MatchType ?? (object)DBNull.Value))
                                    .AddParameter(new SqlParameter("@title", customerMatchParameter.Title ?? (object)DBNull.Value))
                                    .AddParameter(new SqlParameter("@firstName", customerMatchParameter.FirstName ?? (object)DBNull.Value))
                                    .AddParameter(new SqlParameter("@surname", customerMatchParameter.Surname ?? (object)DBNull.Value))
                                    .AddParameter(new SqlParameter("@dob", PrepareDobString(customerMatchParameter.Dob) ?? (object)DBNull.Value ))
                                    .AddParameter(new SqlParameter("@email", customerMatchParameter.Email ?? (object)DBNull.Value))
                                    .AddParameter(new SqlParameter("@phone", customerMatchParameter.Phone ?? (object)DBNull.Value))
                                    .AddParameter(new SqlParameter("@postcode", customerMatchParameter.Postcode ?? (object)DBNull.Value))
                                    .AddParameter(new SqlParameter("@address1", customerMatchParameter.Address1 ?? (object)DBNull.Value))
                                    .AddParameter(new SqlParameter("@address2", customerMatchParameter.Address2 ?? (object)DBNull.Value))
                                    .AddParameter(new SqlParameter("@address3", customerMatchParameter.Address3 ?? (object)DBNull.Value))
                                    .AddParameter(new SqlParameter("@address4", customerMatchParameter.Address4 ?? (object)DBNull.Value));

                    var adapter = new SqlDataAdapter(command);
                    DataTable table = new DataTable();
                    adapter.Fill(table);
                    adapter.Dispose();

                    int?[] matchedCustomerIds = table.AsEnumerable().Select(s => s.Field<int?>("customer_id")).ToArray();

                    customerIds = BuildCustomerIdList(matchedCustomerIds);

                    return customerIds;
                }
            }
            catch (Exception ex)
            {
                // _logger.Error("CustomerMatchDataAccessHandler.MatchWithSpecifiedMatchType():" + "ErrorTag: " + ErrorTagProvider.ErrorTagDatabase + " -- " + ex.Message, ex);
                LogException("CustomerMatchDataAccessHandler.MatchWithSpecifiedMatchType(): ErrorTag: ", customerMatchParameter, ex);


                if (_writeParameterValuesToLog)
                {
                    LogErrorParrameters(customerMatchParameter);

                }

                throw new Exception(String.Format(DatabaseMessage.DatabaseException, ErrorTagProvider.ErrorTagDatabase));
            }
        }

        private void LogErrorParrameters(CustomerMatchParameter customerMatchParameter)
        {
            var matchType = customerMatchParameter.MatchType;
            var title = string.IsNullOrEmpty(customerMatchParameter.Title) ? "null" : customerMatchParameter.Title;
            var firstName = string.IsNullOrEmpty(customerMatchParameter.FirstName) ? "null" : customerMatchParameter.FirstName;
            var surName = string.IsNullOrEmpty(customerMatchParameter.Surname) ? "" : customerMatchParameter.Surname;
            var dob = PrepareDobString(customerMatchParameter.Dob);

            var email = string.IsNullOrEmpty(customerMatchParameter.Email) ? "null" : customerMatchParameter.Email;
            var phone = string.IsNullOrEmpty(customerMatchParameter.Phone) ? "null" : customerMatchParameter.Phone;
            var postcode = string.IsNullOrEmpty(customerMatchParameter.Postcode) ? "null" : customerMatchParameter.Postcode;
            var address1 = string.IsNullOrEmpty(customerMatchParameter.Address1) ? "null" : customerMatchParameter.Address1;
            var address2 = string.IsNullOrEmpty(customerMatchParameter.Address2) ? "null" : customerMatchParameter.Address2;
            var address3 = string.IsNullOrEmpty(customerMatchParameter.Address3) ? "null" : customerMatchParameter.Address3;
            var address4 = string.IsNullOrEmpty(customerMatchParameter.Address4) ? "null" : customerMatchParameter.Address4;

            _logger.Error(
                $"Parameters CustomerMatchDataAccessHandler:- Name=FirstName={firstName}, LastName={surName}, Dob={dob}, " +
                $"Address=AddresLine1={address1}, AddressLine2={address2}, AddressLine3={address3}, " +
                $"AddressLine4={address4}, PostCode={postcode}");

        }


        private static List<MatchedCustomer> BuildCustomerIdList(int?[] custIds)
        {
            var retval = custIds.Select(id => new MatchedCustomer {CustomerId = id ?? 0 }).ToList();
            retval.RemoveAll(id => id.CustomerId < 1) ;

            return retval;

        }

    }
}
