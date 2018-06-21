using System;
using System.Collections.Generic;
using Saga.Gmd.WebApiServices.Models.Customer;
using Saga.Gmd.WebApiServices.Models.Permissions;
using System.Configuration;
using log4net;
using Saga.Gmd.WebApiServices.Common;
using Saga.Gmd.WebApiServices.Models.Gdpr;
using Newtonsoft.Json;
using Saga.Gmd.WebApiServices.Models.ReturnMe;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using Saga.Gmd.WebApiServices.Models;
using Saga.Gmd.WebApiServices.Models.MailingHistory;
using Saga.Gmd.WebApiServices.DAL.Customer;

namespace Saga.Gmd.WebApiServices.DAL.Permissions
{
    public class PermissionDataAccess : IPermissionDataAccess
    {
        private string PulicationDbConnectionString { get; set; }
        private string MciCrDbConnectionString { get; set; }

        private string CpcDbConnectionString { get; }

        private readonly ILog _logger;
        private readonly IMciRequestDataAccess _mciRequestDataAccess;
        private readonly bool _logParameterValue;

        public PermissionDataAccess(ILog logger, IMciRequestDataAccess mciRequestDataAccess)
        {
            _mciRequestDataAccess = mciRequestDataAccess;
            PulicationDbConnectionString = ConfigurationManager.AppSettings["PublicationDataConnection"];
            MciCrDbConnectionString = ConfigurationManager.AppSettings["MciCrConnection"];
            CpcDbConnectionString = ConfigurationManager.AppSettings["CPCConnection"];
            _logger = logger;
            _logParameterValue = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableObjectDump"]);
        }


        #region PostPermissions

        public string PostCustomerPermission(Models.Gdpr.PermissionsCustomerLoadModel permission)
        {
            //Post the permission data to the database and return true or false; 
            //As per discusston with John /Sam, there will always be a record creted for 
            //permissions and that can be located using the permisisonkey passed within 
            //permission object
            var message = DatabaseMessage.PermissionsNotCreated;
            if (!CheckForValidPermissionKey(permission.PermissionsId))
            {
                message = DatabaseMessage.NotValidPermissionsId;
            }
            else if (PendingLoadGeneralInsert(permission))
            {
                message = DatabaseMessage.PermissionsCreated;
            }
            return message;
        }



        public int GetCustomerIdFromPermission(long permissionsId)
        {
            using (var conn = new SqlConnection(CpcDbConnectionString))
            {
                conn.Open();
                try
                {
                    var sqltext = "Select PkAllocated from dbo.ResponsePermissionsGeneral Where PermissionsId = @permissionsId";

                    SqlCommand command = new SqlCommand(sqltext, conn) { CommandType = CommandType.Text };

                    command.Parameters.Add(new SqlParameter("permissionsId", SqlDbType.BigInt, 8) { SqlValue = permissionsId });

                    var adapter = new SqlDataAdapter(command);
                    var table = new DataTable();
                    adapter.Fill(table);
                    adapter.Dispose();

                    if (table?.Rows.Count > 0)
                    {
                        return Convert.ToInt32(table.Rows[0][0]);
                    }
                }
                catch (SqlException ex)
                {
                    _logger.Error(
                        "CheckForValidPermissionKey:" + "ErrorTag: " + ErrorTagProvider.ErrorTagDatabase + " -- " +
                        ex.Message, ex);
                    throw new Exception(String.Format(DatabaseMessage.DatabaseException,
                        ErrorTagProvider.ErrorTagDatabase));
                }
                catch (Exception ex)
                {
                    _logger.Error(
                        "CheckForValidPermissionKey:" + "ErrorTag: " + ErrorTagProvider.ErrorTagDatabase + " -- " +
                        ex.Message, ex);
                    throw new Exception(String.Format(DatabaseMessage.DatabaseException,
                        ErrorTagProvider.ErrorTagDatabase));
                }
            }
            return 0;

        }



        private bool CheckForValidPermissionKey(long permissionsId)
        {
            return GetCustomerIdFromPermission(permissionsId) > 0;
        }


        private bool PendingLoadGeneralInsert(PermissionsCustomerLoadModel permissions)
        {
            using (var conn = new SqlConnection(CpcDbConnectionString))
            {
                conn.Open();
                // var transaction = conn.BeginTransaction();
                try
                {

                    //DumpModel.Dump(permissions, _logger, true);

                    DateTime? dateOfBirth = Common.DateTimeExtensions.GetValidDate(permissions.CustomerNameAndAddress.Dob);
                    var dob = dateOfBirth == null ? null : dateOfBirth.ToReturnIsoNullDateIfValid();

                    int pkAllocated = 0;
                    var cmd = new SqlCommand("[dbo].[procPendingLoadGeneralInsert]", conn);
                    string matchType = MatchType.NAME_AND_ADDRESS.ToString();

                    cmd.Parameters.AddWithValue("@match_type", matchType);
                    cmd.Parameters.AddWithValue("@title", permissions.CustomerNameAndAddress?.Title);
                    cmd.Parameters.AddWithValue("@surname", permissions.CustomerNameAndAddress?.Surname);
                    cmd.Parameters.AddWithValue("@first_name", permissions.CustomerNameAndAddress?.FirstName);
                    cmd.Parameters.AddWithValue("@dob", dob);
                    cmd.Parameters.AddWithValue("@email", permissions.CustomerNameAndAddress?.Email);
                    cmd.Parameters.AddWithValue("@phone", permissions.CustomerNameAndAddress?.Phone);

                    cmd.Parameters.AddWithValue("@postcode",
                        string.IsNullOrWhiteSpace(permissions.CustomerNameAndAddress?.Address?.Postcode)
                            ? permissions.CustomerNameAndAddress?.CustomerAddress?.Postcode
                            : permissions.CustomerNameAndAddress?.Address?.Postcode);

                    cmd.Parameters.AddWithValue("@address1", BuildAddress1(permissions.CustomerNameAndAddress?.Address, permissions.CustomerNameAndAddress?.CustomerAddress));

                    cmd.Parameters.AddWithValue("@key", permissions.CustomerKeyValue.Key ?? string.Empty);
                    cmd.Parameters.AddWithValue("@Value", permissions.CustomerKeyValue.Value ?? string.Empty);

                    cmd.Parameters.AddWithValue("@PermissionsId", permissions.PermissionsId);
                    cmd.Parameters.AddWithValue("@Source", permissions.Source ?? string.Empty);
                    var hacParameter = new SqlParameter("@HAC", SqlDbType.Bit) { SqlValue = (object)permissions.Hac ?? DBNull.Value };
                    cmd.Parameters.Add(hacParameter);
                    cmd.Parameters.AddWithValue("@ReConsentRequiredCore", (object)permissions.ReConsentRequiredCore ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@JourneyType", permissions.JourneyType ?? string.Empty);
                    cmd.Parameters.AddWithValue("@Journey", permissions.Journey ?? string.Empty);
                    cmd.Parameters.AddWithValue("@LastUpdatedDate", (object)permissions.LastUpdatedDate ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@LastUpdatedAgentName", permissions.LastUpdatedAgentName ?? string.Empty);
                    cmd.Parameters.AddWithValue("@WordingID", permissions.QuestionId ?? string.Empty);
                    cmd.Parameters.AddWithValue("@ChannelPostAddressValue", GetCustomerAddress(permissions?.ChannelPostalAddress));
                    cmd.Parameters.AddWithValue("@ChannelEmailAddressValue", permissions.ChannelEmailAddress ?? string.Empty);
                    cmd.Parameters.AddWithValue("@ChannelPhoneNoValue", permissions.ChannelPhoneNo ?? string.Empty);
                    cmd.Parameters.AddWithValue("@ChannelSMSNoValue", permissions.ChannelSmsNo ?? string.Empty);
                    cmd.Parameters.AddWithValue("@ChannelPostAddressList", string.Empty);
                    cmd.Parameters.AddWithValue("@ChannelEmailAddressList", string.Empty);
                    cmd.Parameters.AddWithValue("@ChannelPhoneNoList", string.Empty);
                    cmd.Parameters.AddWithValue("@ChannelSMSNoList", string.Empty);
                    cmd.Parameters.AddWithValue("@CurrentDateTime", DateTime.Now);

                    var outputPkAllocated = new SqlParameter("@PKAllocated", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(outputPkAllocated);

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteScalar();

                    if (outputPkAllocated.Value != null)
                    {
                        int.TryParse(outputPkAllocated.Value.ToString(), out pkAllocated);
                    }

                    if (permissions.PermissionsId > 0 && pkAllocated > 0)
                    {
                        foreach (var category in permissions.PermissionCategory)
                        {


                            var categoryCommand =
                                new SqlCommand("[dbo].[procPendingLoadCompanyInsert2]", conn);


                            categoryCommand.Parameters.AddWithValue("@PermissionsID", permissions.PermissionsId);
                            categoryCommand.Parameters.AddWithValue("@PKAllocated", pkAllocated);
                            categoryCommand.Parameters.AddWithValue("@PermissionCategoryDisplayValue", category.PermissionCategoryDisplayValue);
                            categoryCommand.Parameters.AddWithValue("@PermissionCategoryStatus", category.PermissionCategoryStatus);
                            categoryCommand.Parameters.AddWithValue("@LastUpdatedDate", category.LastUpdatedDate);
                            categoryCommand.Parameters.AddWithValue("@ChannelPostStatus", category.ChannelPostFlag);
                            categoryCommand.Parameters.AddWithValue("@ChannelPostStatusCapturedDate", DBNull.Value);
                            categoryCommand.Parameters.AddWithValue("@ChannelPostStatusExpiryDate", DBNull.Value);
                            categoryCommand.Parameters.AddWithValue("@ChannelEmailStatus", category.ChannelEmailFlag);
                            categoryCommand.Parameters.AddWithValue("@ChannelEmailStatusCapturedDate", DBNull.Value);
                            categoryCommand.Parameters.AddWithValue("@ChannelEmailStatusExpiryDate", DBNull.Value);
                            categoryCommand.Parameters.AddWithValue("@ChannelPhoneStatus", category.ChannelPhoneNoFlag);
                            categoryCommand.Parameters.AddWithValue("@ChannelPhoneStatusCapturedDate", DBNull.Value);
                            categoryCommand.Parameters.AddWithValue("@ChannelPhoneStatusExpiryDate", DBNull.Value);
                            categoryCommand.Parameters.AddWithValue("@ChannelSMSStatus", category.ChannelSmsFlag);
                            categoryCommand.Parameters.AddWithValue("@ChannelSMSStatusCapturedDate", DBNull.Value);
                            categoryCommand.Parameters.AddWithValue("@ChannelSMSStatusExpiryDate", DBNull.Value);
                            categoryCommand.Parameters.AddWithValue("@ReconsentRequired", category.ReConsentRequired);


                            categoryCommand.CommandType = CommandType.StoredProcedure;
                            categoryCommand.ExecuteScalar();
                        }

                        using (var permissionUpdateCommand = new SqlCommand("[dbo].[procPermissionsCategoryDetailUpdate2]", conn))
                        {
                            permissionUpdateCommand.Parameters.AddWithValue("@PermissionsID", permissions.PermissionsId);
                            permissionUpdateCommand.Parameters.AddWithValue("@PKAllocated", pkAllocated);
                            permissionUpdateCommand.CommandType = CommandType.StoredProcedure;
                            permissionUpdateCommand.ExecuteScalar();
                        }

                        conn.Close();

                        return true;
                    }
                    else
                    {
                        conn.Close();
                    }

                }
                catch (SqlException ex)
                {
                    //   transaction.Rollback();
                    _logger.Error(
                        "Insert PendingLoadGeneralInsert : ErrorTag: " + ErrorTagProvider.ErrorTagDatabase + " -- " + ex.Message, ex);
                    throw new Exception(string.Format(DatabaseMessage.DatabaseException, ErrorTagProvider.ErrorTagDatabase));
                }
                catch (Exception ex)
                {
                    //  transaction.Rollback();
                    _logger.Error(
                        "Insert PendingLoadGeneralInsert : ErrorTag: " + ErrorTagProvider.ErrorTagDatabase + " -- " + ex.Message, ex);
                    throw new Exception(string.Format(DatabaseMessage.DatabaseException, ErrorTagProvider.ErrorTagDatabase));
                }
            }

            return false;
        }

        private string GetCustomerAddress(CustomerAddress customerAddress)
        {
            if (customerAddress == null)
            {
                return string.Empty;
            }

            StringBuilder address = new StringBuilder();

            if (string.IsNullOrWhiteSpace(customerAddress.HouseName))
            {
                address.Append(customerAddress.HouseName);
                address.Append(", ");
            }
            if (string.IsNullOrWhiteSpace(customerAddress.HouseNumber))
            {
                address.Append(customerAddress.HouseNumber);
                address.Append(", ");
            }
            if (string.IsNullOrWhiteSpace(customerAddress.Street))
            {
                address.Append(customerAddress.Street);
                address.Append(", ");
            }
            if (string.IsNullOrWhiteSpace(customerAddress.Street1))
            {
                address.Append(customerAddress.Street1);
                address.Append(", ");
            }
            if (string.IsNullOrWhiteSpace(customerAddress.City))
            {
                address.Append(customerAddress.City);
                address.Append(", ");
            }
            if (string.IsNullOrWhiteSpace(customerAddress.Country))
            {
                address.Append(customerAddress.Country);
                address.Append(", ");
            }
            if (string.IsNullOrWhiteSpace(customerAddress.County))
            {
                address.Append(customerAddress.County);
                address.Append(", ");
            }
            if (string.IsNullOrWhiteSpace(customerAddress.Postcode))
            {
                address.Append(customerAddress.Postcode);
            }

            return address.ToString().Trim().TrimEnd(',').Trim();
        }

        private string GetCustomerAddress(Address customerAddress)
        {
            StringBuilder address = new StringBuilder();

            if (string.IsNullOrWhiteSpace(customerAddress.Address1))
            {
                address.Append(customerAddress.Address1);
                address.Append(", ");
            }
            if (string.IsNullOrWhiteSpace(customerAddress.Address2))
            {
                address.Append(customerAddress.Address2);
                address.Append(", ");
            }
            if (string.IsNullOrWhiteSpace(customerAddress.Address3))
            {
                address.Append(customerAddress.Address3);
                address.Append(", ");
            }
            if (string.IsNullOrWhiteSpace(customerAddress.Address4))
            {
                address.Append(customerAddress.Address4);
                address.Append(", ");
            }
            if (string.IsNullOrWhiteSpace(customerAddress.Postcode))
            {
                address.Append(customerAddress.Postcode);
                address.Append(", ");
            }
            if (string.IsNullOrWhiteSpace(customerAddress.MatchType))
            {
                address.Append(customerAddress.MatchType);
            }

            return address.ToString().Trim().TrimEnd(',').Trim();
        }

        #endregion


        #region GetPermissions

        private int GetCustomerIdFromKeyValue(KeyValue keyValue)
        {
            var customerKey = _mciRequestDataAccess.GetCustomerAllIndexKeys(keyValue.Key,
                keyValue.Value, SourceKey.PKEY.ToString());
            List<int> customerIds = new List<int>();
            foreach (var key in customerKey)
            {
                customerIds.Add(key.CustomerId);
            }

            return customerIds.First();
        }

        /// <summary>
        /// Permissions reader 
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="nameAndAddress"></param>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public dynamic GetCustomerPermission(ReturnMePermissions parameter, NameAndAddress nameAndAddress,
            int customerId)
        {
            return GetCustomerPermission_Worker(parameter, nameAndAddress, null  /* key value */, customerId );
        }

        /// <summary>
        /// Name & address / Key Value - based permissions reader
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="nameAndAddress"></param>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public dynamic GetCustomerPermission(ReturnMePermissions parameter, NameAndAddress nameAndAddress,
            KeyValue keyValue )
        {
            return GetCustomerPermission_Worker(parameter, nameAndAddress, keyValue);
        }

        /// <summary>
        /// Worker method to accept optional customer_id param to override Name and Address matching used by Db routines
        /// If a valid Customer_Id is passed, then an empty name and address param can ba passed
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="nameAndAddress"></param>
        /// <param name="keyValue"></param>
        /// <param name="customer_id"></param>
        /// <returns></returns>
        private dynamic GetCustomerPermission_Worker(ReturnMePermissions parameter, NameAndAddress nameAndAddress, KeyValue keyValue, int customer_id = 0)
        // public dynamic GetCustomerPermission(ReturnMePermissions parameter, NameAndAddress nameAndAddress, KeyValue keyValue)
        {
            try
            {
                //Get data from database

                // int customer_Id = 0;
                // Resolve customer_id from keyValue  
                if (keyValue != null)
                {
                    customer_id = string.Compare("CPCK", keyValue.Key, StringComparison.CurrentCultureIgnoreCase) == 0 
                        ? GetCustomerIdFromPermission(Convert.ToInt64(keyValue.Value)) : GetCustomerIdFromKeyValue(keyValue);
                }

                JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All, Formatting = Formatting.Indented };

                GdprResponseType responseType;
                object deserializedData = null;
                bool isValidRequest = Enum.TryParse(parameter.ResponseParameter, out responseType);
                if (isValidRequest)
                {
                    object data = null;

                    // Name & Address Matching in this block (in dbo.ResponsePermissionsGeneralGet() )
                    if (string.Equals(parameter.ResponseParameter, GdprResponseType.Full.ToString(), StringComparison.OrdinalIgnoreCase))
                    {
                        var dsFull = ResponsePermissionsGeneralGet(parameter, nameAndAddress, customer_id);
                        if (dsFull?.Tables.Count == 0 || dsFull?.Tables[0].Rows.Count == 0)
                        {
                            return new DatabaseException(ErrorMessages.CustomerNotFound);
                        }
                        data = BuildPermissionsFull(dsFull, parameter);
                        string serialized = JsonConvert.SerializeObject(data, settings);
                        deserializedData = JsonConvert.DeserializeObject<PermissionFull>(serialized, settings);

                    }
                    else if (string.Equals(parameter.ResponseParameter, GdprResponseType.Specified.ToString(), StringComparison.OrdinalIgnoreCase))
                    {
                        var dsSpecified = ResponsePermissionsGeneralGet(parameter, nameAndAddress, customer_id);
                        if (dsSpecified?.Tables.Count == 0)
                        {
                            return new DatabaseException(ErrorMessages.CustomerNotFound);
                        }
                        data = BuildPermissionSpecified(dsSpecified, parameter);
                        string serialized1 = JsonConvert.SerializeObject(data, settings);
                        deserializedData =
                            JsonConvert.DeserializeObject<PermissionSpecified>(serialized1, settings);

                    }
                    else if (string.Equals(parameter.ResponseParameter, GdprResponseType.Summary.ToString(), StringComparison.OrdinalIgnoreCase))
                    {

                        DataSet summaryPermissions = SummaryPermissionsGet(parameter, nameAndAddress, customer_id);
                        int PKAllocated = 0;
                        if (summaryPermissions.Tables.Count == 0)
                        {
                            return new DatabaseException(ErrorMessages.CustomerSummaryPermissionsNotFound);

                        }
                        else if (summaryPermissions.Tables[0].Columns.Count <= 1)
                        {
                            return new DatabaseException(ErrorMessages.CustomerNotFound);
                        }
                        else
                        {
                            int.TryParse(summaryPermissions.Tables[0].Rows[0][0].ToString(), out PKAllocated);
                        }

                        //var dsSummary = ResponsePermissionsGeneralGet(parameter, nameAndAddress);
                        data = BuildPermissionsSummary(summaryPermissions);
                        var serialized2 = JsonConvert.SerializeObject(data, settings);
                        deserializedData =
                            JsonConvert.DeserializeObject<PermissionSummary>(serialized2, settings);

                        if (summaryPermissions.Tables.Count > 0)
                        {
                            PendingEnquiryInsert(PKAllocated, parameter, nameAndAddress);
                        }

                    }
                    else
                    {
                        throw new ArgumentException("Invalid request Type.");
                    }
                }

                return deserializedData;
            }
            catch (InvalidCastException invalidCastException)
            {
                _logger.Error(
                    "GetCustomerPermission : ErrorTag: " + ErrorTagProvider.ErrorTagDatabase + " -- " +
                    invalidCastException.Message, invalidCastException);
                throw new Exception(string.Format(DatabaseMessage.DatabaseException, ErrorTagProvider.ErrorTagDatabase));
            }
            catch (Exception ex)
            {
                _logger.Error(
                    "GetCustomerPermission : ErrorTag: " + ErrorTagProvider.ErrorTagDatabase + " -- " + ex.Message, ex);
                throw new Exception(string.Format(DatabaseMessage.DatabaseException, ErrorTagProvider.ErrorTagDatabase));
            }
        }


        private long PendingEnquiryInsert(int PKAllocated, ReturnMePermissions permission, NameAndAddress nameAndAddress)
        {
            long rtnPermissionId = 0;

            var conn = new SqlConnection(CpcDbConnectionString);
            {
                var cmd = new SqlCommand("[dbo].[procPendingEnquiryInsert]", conn);
                {
                    try
                    {
                        DateTime? dateOfBirth = Common.DateTimeExtensions.GetValidDate(nameAndAddress.Dob);
                        var dob = dateOfBirth == null ? null : dateOfBirth.ToReturnIsoNullDateIfValid();

                        cmd.Connection.Open();
                        cmd.Parameters.AddWithValue("@PKAllocated", PKAllocated);
                        cmd.Parameters.AddWithValue("@Title", nameAndAddress.Title ?? string.Empty);
                        cmd.Parameters.AddWithValue("@Surname", nameAndAddress.Surname ?? string.Empty);
                        cmd.Parameters.AddWithValue("@FirstName", nameAndAddress.FirstName ?? string.Empty);
                        cmd.Parameters.AddWithValue("@DOB", dob);
                        cmd.Parameters.AddWithValue("@Email", nameAndAddress.Email ?? string.Empty);
                        cmd.Parameters.AddWithValue("@Product", nameAndAddress.Product ?? string.Empty);
                        cmd.Parameters.AddWithValue("@Phone", nameAndAddress.Phone ?? string.Empty);
                        cmd.Parameters.AddWithValue("@HouseNo", nameAndAddress.CustomerAddress.HouseNumber ?? string.Empty);
                        cmd.Parameters.AddWithValue("@HouseName", nameAndAddress.CustomerAddress.HouseName ?? string.Empty);
                        cmd.Parameters.AddWithValue("@Street", nameAndAddress.CustomerAddress.Street ?? nameAndAddress.Address.Address1 ?? string.Empty);
                        cmd.Parameters.AddWithValue("@Street1", nameAndAddress.CustomerAddress.Street1 ?? string.Empty);
                        cmd.Parameters.AddWithValue("@City", nameAndAddress.CustomerAddress.City ?? nameAndAddress.Address.Address2 ?? string.Empty);
                        cmd.Parameters.AddWithValue("@County", nameAndAddress.CustomerAddress.County ?? nameAndAddress.Address.Address3 ?? string.Empty);
                        cmd.Parameters.AddWithValue("@Country", nameAndAddress.CustomerAddress.Country ?? nameAndAddress.Address.Address4 ?? string.Empty);
                        cmd.Parameters.AddWithValue("@Postcode", nameAndAddress.CustomerAddress.Postcode ?? nameAndAddress.Address.Postcode ?? string.Empty);
                        cmd.Parameters.AddWithValue("@Brand", nameAndAddress.Brand ?? string.Empty);
                        cmd.Parameters.AddWithValue("@Journey", permission.Journey ?? string.Empty);
                        cmd.Parameters.AddWithValue("@ResponseParameter", permission.ResponseParameter ?? string.Empty);
                        cmd.Parameters.AddWithValue("@PermissionParameter", permission.PermissionParameter ?? string.Empty);

                        SqlParameter outputPermissionsId = new SqlParameter("@PermissionsId", SqlDbType.BigInt) { Direction = ParameterDirection.Output };
                        cmd.Parameters.Add(outputPermissionsId);

                        cmd.CommandType = CommandType.StoredProcedure;

                        var rtnObject = cmd.ExecuteNonQuery();


                        if (outputPermissionsId.Value != null)
                        {
                            long.TryParse(outputPermissionsId.Value.ToString(), out rtnPermissionId);
                        }

                    }
                    catch (Exception ex)
                    {
                        _logger.Error(
                            "Insert PendingEnquiry : ErrorTag: " + ErrorTagProvider.ErrorTagDatabase + " -- " + ex.Message, ex);

                        if (_logParameterValue)
                        {
                            _logger.Error(
                                $"Parameters PendingEnquiryInsert:- Title={nameAndAddress.Title ?? string.Empty}, FirstName={nameAndAddress.FirstName ?? string.Empty}, LastName={nameAndAddress.Surname ?? string.Empty}, Dob={nameAndAddress.Dob}, " +
                                $" Email={nameAndAddress.Email ?? string.Empty}, Product={nameAndAddress.Product ?? string.Empty}, Phone={nameAndAddress.Phone ?? string.Empty}, HouseNo={nameAndAddress.CustomerAddress.HouseNumber ?? string.Empty} " +
                                $"Address=AddresLine1={nameAndAddress.Address.Address1 ?? string.Empty}, AddressLine2={nameAndAddress.Address.Address2 ?? string.Empty}, AddressLine3={nameAndAddress.Address.Address3 ?? string.Empty}, " +
                                $"AddressLine4={nameAndAddress.Address.Address4 ?? string.Empty}, PostCode={nameAndAddress.Address.Postcode ?? string.Empty}");
                        }


                        throw new Exception(string.Format(DatabaseMessage.DatabaseException, ErrorTagProvider.ErrorTagDatabase));
                    }
                }
            }

            return rtnPermissionId;
        }

        private DataSet SummaryPermissionsGet(ReturnMePermissions permission, NameAndAddress nameAndAddress, int customer_Id)
        {
            using (var conn = new SqlConnection(CpcDbConnectionString))
            {
                using (var cmd = new SqlCommand("[dbo].[procSummaryPermissionsGet]", conn))
                {
                    try
                    {
                        DateTime? dateOfBirth = Common.DateTimeExtensions.GetValidDate(nameAndAddress.Dob);
                        var dob = dateOfBirth == null ? null : dateOfBirth.ToReturnIsoNullDateIfValid();

                        cmd.Connection.Open();
                        //cmd.Parameters.AddWithValue("@p_match_type", MatchType.NAME_AND_ADDRESS.ToString());
                        cmd.Parameters.AddWithValue("@p_match_type", MatchType.POSTCODE_AND_NAME.ToString());
                        cmd.Parameters.AddWithValue("@p_title", nameAndAddress.Title);
                        cmd.Parameters.AddWithValue("@p_surname", nameAndAddress.Surname);
                        cmd.Parameters.AddWithValue("@p_first_name", nameAndAddress.FirstName);
                        cmd.Parameters.AddWithValue("@p_dob", dob ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@p_email", nameAndAddress.Email);
                        cmd.Parameters.AddWithValue("@p_phone", nameAndAddress.Phone);
                        cmd.Parameters.AddWithValue("@p_postcode", nameAndAddress.Address.Postcode ?? nameAndAddress.CustomerAddress.Postcode);
                        cmd.Parameters.AddWithValue("@p_address1", BuildAddress1(nameAndAddress.Address, nameAndAddress.CustomerAddress));
                        cmd.Parameters.AddWithValue("@p_address2", nameAndAddress.Address.Address2 ?? (nameAndAddress.CustomerAddress.City ?? ""));
                        cmd.Parameters.AddWithValue("@p_address3", nameAndAddress.Address.Address3 ?? (nameAndAddress.CustomerAddress.County ?? ""));
                        cmd.Parameters.AddWithValue("@p_address4", nameAndAddress.Address.Address4 ?? (nameAndAddress.CustomerAddress.Country ?? ""));
                        cmd.Parameters.AddWithValue("@Response_Parameter", permission.ResponseParameter);
                        cmd.Parameters.AddWithValue("@Permission_Parameter", permission.PermissionParameter);
                        cmd.Parameters.AddWithValue("@Journey", permission.Journey);
                        cmd.Parameters.AddWithValue("@Customer_Id", customer_Id);

                        cmd.CommandType = CommandType.StoredProcedure;

                        var ds = new DataSet();
                        var adapter = new SqlDataAdapter(cmd);
                        adapter.Fill(ds);

                        return ds;
                    }
                    catch (Exception ex)
                    {
                        _logger.Error(
                            "SummaryPermissionsGet : ErrorTag: " + ErrorTagProvider.ErrorTagDatabase + " -- " +
                            ex.Message, ex);

                        if (_logParameterValue)
                        {
                            _logger.Error(
                                $"Parameters SummaryPermissionsGet:- Name=FirstName={nameAndAddress.FirstName}, LastName={nameAndAddress.Surname}, Dob={nameAndAddress.Dob}, " +
                                $" Address=AddresLine1={BuildAddress1(nameAndAddress.Address, nameAndAddress.CustomerAddress)}, AddressLine2={nameAndAddress.Address.Address2 ?? (nameAndAddress.CustomerAddress.City ?? "")}, AddressLine3={nameAndAddress.Address.Address3 ?? (nameAndAddress.CustomerAddress.County ?? "")}, " +
                                $" AddressLine4={nameAndAddress.Address.Address4 ?? (nameAndAddress.CustomerAddress.Country ?? "")}, PostCode={nameAndAddress.Address.Postcode ?? nameAndAddress.CustomerAddress.Postcode}" +
                                $" ResponseParameter={permission.ResponseParameter}, PermissionParameter={permission.PermissionParameter}, Journey={permission.Journey}, CustomerId={customer_Id}");
                        }

                        throw new Exception(string.Format(DatabaseMessage.DatabaseException,
                            ErrorTagProvider.ErrorTagDatabase));
                    }
                }
            }
        }

        private DataSet ResponsePermissionsGeneralGet(ReturnMePermissions permission, NameAndAddress nameAndAddress, int customer_Id)
        {
            using (var conn = new SqlConnection(CpcDbConnectionString))
            {
                using (var cmd = new SqlCommand("[dbo].[procResponsePermissionsGeneralGet]", conn))
                {
                    try
                    {
                        DateTime? dateOfBirth = Common.DateTimeExtensions.GetValidDate(nameAndAddress.Dob);
                        var dob = dateOfBirth == null ? null : dateOfBirth.ToReturnIsoNullDateIfValid();

                        cmd.Connection.Open();

                        //cmd.Parameters.AddWithValue("@p_match_type", MatchType.NAME_AND_ADDRESS.ToString());
                        cmd.Parameters.AddWithValue("@p_match_type", MatchType.POSTCODE_AND_NAME.ToString());
                        cmd.Parameters.AddWithValue("@p_title", nameAndAddress.Title);
                        cmd.Parameters.AddWithValue("@p_surname", nameAndAddress.Surname);
                        cmd.Parameters.AddWithValue("@p_first_name", nameAndAddress.FirstName);
                        cmd.Parameters.AddWithValue("@p_dob", dob ?? (object)DBNull.Value);// == null || nameAndAddress.Dob == DateTime.MinValue ? "default" : nameAndAddress.Dob.ToReturnIsoNullDateIfValid()); 
                        cmd.Parameters.AddWithValue("@p_email", nameAndAddress.Email);
                        cmd.Parameters.AddWithValue("@p_phone", nameAndAddress.Phone);
                        cmd.Parameters.AddWithValue("@p_postcode", nameAndAddress.Address.Postcode ?? nameAndAddress.CustomerAddress.Postcode);
                        cmd.Parameters.AddWithValue("@p_address1", BuildAddress1(nameAndAddress.Address, nameAndAddress.CustomerAddress));
                        cmd.Parameters.AddWithValue("@p_address2", nameAndAddress.Address.Address2 ?? (nameAndAddress.CustomerAddress.City ?? ""));
                        cmd.Parameters.AddWithValue("@p_address3", nameAndAddress.Address.Address3 ?? (nameAndAddress.CustomerAddress.County ?? ""));
                        cmd.Parameters.AddWithValue("@p_address4", nameAndAddress.Address.Address4 ?? (nameAndAddress.CustomerAddress.Country ?? ""));
                        cmd.Parameters.AddWithValue("@Response_Parameter", permission.ResponseParameter);
                        // GITCS-187 : Handle null parameter values to avoid null param dbExceptions
                        cmd.Parameters.AddWithValue("@Permission_Parameter", permission.PermissionParameter ?? "");
                        cmd.Parameters.AddWithValue("@Journey", permission.Journey ?? "");
                        cmd.Parameters.AddWithValue("@Customer_Id", customer_Id);

                        cmd.CommandType = CommandType.StoredProcedure;

                        var ds = new DataSet();
                        var adapter = new SqlDataAdapter(cmd);
                        adapter.Fill(ds);

                        return ds;
                    }
                    catch (Exception ex)
                    {
                        _logger.Error(
                            "ResponsePermissionsGeneralGet : ErrorTag: " + ErrorTagProvider.ErrorTagDatabase + " -- " +
                            ex.Message, ex);

                        if (_logParameterValue)
                        {
                            _logger.Error(
                                $"Parameters ResponsePermissionsGeneralGet:- Name=FirstName={nameAndAddress.FirstName}, LastName={nameAndAddress.Surname}, Dob={nameAndAddress.Dob}, " +
                                $" Address=AddresLine1={ BuildAddress1(nameAndAddress.Address, nameAndAddress.CustomerAddress)}, AddressLine2={nameAndAddress.Address.Address2 ?? (nameAndAddress.CustomerAddress.City ?? "")}, AddressLine3={nameAndAddress.Address.Address3 ?? (nameAndAddress.CustomerAddress.County ?? "")}, " +
                                $" AddressLine4={nameAndAddress.Address.Address4 ?? (nameAndAddress.CustomerAddress.Country ?? "")}, PostCode={nameAndAddress.Address.Postcode ?? nameAndAddress.CustomerAddress.Postcode}" +
                                $" ResponseParameter={permission.ResponseParameter}, PermissionParameter={permission.PermissionParameter}, Journey={permission.Journey}, CustomerId={customer_Id}");
                        }

                        throw new Exception(string.Format(DatabaseMessage.DatabaseException,
                            ErrorTagProvider.ErrorTagDatabase));
                    }
                }
            }
        }

        private string BuildAddress1(Address address, CustomerAddress customerAddress)
        {
            if (!string.IsNullOrWhiteSpace(address.Address1))
            {
                return address.Address1;
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                if (!string.IsNullOrWhiteSpace(customerAddress.HouseName))
                {
                    sb.Append(customerAddress.HouseName);
                    sb.Append(", ");
                }
                else if (!string.IsNullOrWhiteSpace(customerAddress.HouseNumber))
                {
                    sb.Append(customerAddress.HouseNumber);
                    sb.Append(", ");
                }
                if (!string.IsNullOrWhiteSpace(customerAddress.Street))
                {
                    sb.Append(customerAddress.Street);
                    sb.Append(", ");
                }
                if (!string.IsNullOrWhiteSpace(customerAddress.Street1))
                {
                    sb.Append(customerAddress.Street1);
                }
                return sb.ToString().Trim().TrimEnd(','); ;
            }
        }

        private CustomerAddress BuildCustomerAddress(string addressLine)
        {
            CustomerAddress customerAddress = new CustomerAddress();

            string[] address = addressLine.Split(',');
            if (address.Length > 0)
            {
                if (!string.IsNullOrWhiteSpace(address[address.Length - 1]))
                {
                    customerAddress.Postcode = address[address.Length - 1].Trim();
                }
            }
            if (address.Length > 0)
            {
                if (!string.IsNullOrWhiteSpace(address[0]))
                {
                    customerAddress.Street = address[0].Trim();
                }
            }
            if (address.Length > 1)
            {
                if (!string.IsNullOrWhiteSpace(address[1]))
                {
                    customerAddress.Street1 = address[1].Trim();
                }
            }
            if (address.Length > 2)
            {
                if (!string.IsNullOrWhiteSpace(address[2]))
                {
                    customerAddress.City = address[2].Trim();
                }
            }
            if (address.Length > 3)
            {
                if (!string.IsNullOrWhiteSpace(address[3]))
                {
                    customerAddress.County = address[3].Trim();
                }
            }


            return customerAddress;
        }

        private List<CustomerAddress> BuildCustomerAddressList(string addressLines)
        {
            return addressLines.Split('\t').Select(BuildCustomerAddress).ToList();
        }

        private Address BuildAddress(string addressLine)
        {
            Address customerAddress = new Address();

            string[] address = addressLine.Split(',');
            if (address.Length > 0)
            {
                customerAddress.Postcode = address[address.Length - 1];
            }
            if (address.Length > 1)
            {
                customerAddress.Address1 = address[address.Length - 2];
            }
            if (address.Length > 2)
            {
                customerAddress.Address2 = address[address.Length - 3];
            }
            if (address.Length > 3)
            {
                customerAddress.Address3 = address[address.Length - 4];
            }
            if (address.Length > 4)
            {
                customerAddress.Address4 = address[address.Length - 5];
            }

            return customerAddress;
        }

        /// <summary>
        /// Calls [dbo].[procResponsePermissionsCompanyGet]
        /// </summary>
        /// <param name="pkAllocated"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        private DataTable ResponsePermissionsCompany(long pkAllocated, ReturnMePermissions parameter)
        {
            using (var conn = new SqlConnection(CpcDbConnectionString))
            {
                using (var cmd = new SqlCommand("[dbo].[procResponsePermissionsCompanyGet]", conn))
                {
                    try
                    {
                        cmd.Connection.Open();

                        cmd.Parameters.AddWithValue("@PKAllocated", pkAllocated);
                        cmd.Parameters.AddWithValue("@PermissionParameter", parameter.PermissionParameter ?? "");
                        cmd.Parameters.AddWithValue("@ResponseParameter", parameter.ResponseParameter);
                        cmd.Parameters.AddWithValue("@Journey", parameter.Journey ?? "");
                        cmd.CommandType = CommandType.StoredProcedure;

                        var ds = new DataSet();
                        var adapter = new SqlDataAdapter(cmd);
                        adapter.Fill(ds);

                        if (ds.Tables.Count > 0)
                        {
                            return ds.Tables[0];
                        }
                        else
                        {
                            return null;
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.Error(
                            "ResponsePermissionsCompany : ErrorTag: " + ErrorTagProvider.ErrorTagDatabase + " -- " +
                            ex.Message, ex);

                        if (_logParameterValue)
                        {
                            _logger.Error(
                                $"Parameters ResponsePermissionsCompany:- PKAllocated:- {pkAllocated}, PermissionParameter:- {parameter.PermissionParameter}, ResponseParameter:- {parameter.ResponseParameter}");
                        }

                        throw new Exception(string.Format(DatabaseMessage.DatabaseException,
                            ErrorTagProvider.ErrorTagDatabase));
                    }
                }
            }
        }

        private PermissionSummary BuildPermissionsSummary(DataSet dsSummary)
        {
            if (dsSummary.Tables.Count == 0)
            {
                return new PermissionSummary();
            }
            var summary = new PermissionSummary();
            var table = dsSummary.Tables[0];
            try
            {
                foreach (DataRow row in table.Rows)
                {
                    summary.Source = row["Source"]?.ToString() ?? string.Empty;
                    summary.Hac = row["HAC"].ToString().ToBool();
                    summary.ReConsentRequiredCore = row["ReConsentRequiredCore"].ToString().ToBool();
                    summary.LastUpdatedDate = row["LastUpdatedDate"] == DBNull.Value
                        ? (DateTime?)null
                        : DateTime.Parse(row["LastUpdatedDate"].ToString());
                }
            }
            catch (InvalidCastException invalidCastException)
            {
                _logger.Error(
                    "BuildPermissionsSummary : ErrorTag: " + ErrorTagProvider.ErrorTagDatabase + " -- " +
                    invalidCastException.Message, invalidCastException);
                throw new Exception(string.Format(DatabaseMessage.DatabaseException, ErrorTagProvider.ErrorTagDatabase));
            }
            catch (Exception ex)
            {
                _logger.Error(
                    "BuildPermissionsSummary : ErrorTag: " + ErrorTagProvider.ErrorTagDatabase + " -- " + ex.Message, ex);
                throw new Exception(string.Format(DatabaseMessage.DatabaseException, ErrorTagProvider.ErrorTagDatabase));
            }
            return summary;
        }

        private PermissionFull BuildPermissionsFull(DataSet dsFull, ReturnMePermissions parameter)
        {
            PermissionFull permissionFull = new PermissionFull();
            var table = dsFull.Tables[0];
            try
            {
                int pkAllocated = 0;
                foreach (DataRow row in table.Rows)
                {
                    permissionFull.PermissionId = row["PermissionsID"].ToString().ToIntParsed();
                    pkAllocated = row["PKAllocated"].ToString().ToIntParsed();
                    permissionFull.Source = row["Source"]?.ToString() ?? string.Empty;
                    permissionFull.Hac = row["HAC"].ToString().ToBool();
                    permissionFull.ReConsentRequiredCore = row["ReConsentRequiredCore"].ToString().ToBool();
                    permissionFull.Journey = row["Journey"]?.ToString() ?? string.Empty;
                    permissionFull.JourneyType = row["JourneyType"]?.ToString() ?? string.Empty;
                    permissionFull.LastUpdatedDate = row["LastUpdatedDate"] == DBNull.Value ? (DateTime?)null : DateTime.Parse(row["LastUpdatedDate"].ToString());
                    permissionFull.LastUpdatedAgentName = row["LastUpdatedAgentName"]?.ToString() ?? string.Empty;
                    permissionFull.ChannelEmailAddress = row["ChannelEmailAddressValue"]?.ToString() ?? string.Empty;
                    permissionFull.ChannelPhoneNo = row["ChannelPhoneNoValue"]?.ToString() ?? string.Empty;
                    permissionFull.ChannelSmsNo = row["ChannelSMSNoValue"]?.ToString() ?? string.Empty;

                    //Have to find the data
                    permissionFull.ChannelPostalAddress =
                        BuildCustomerAddress(row["ChannelPostAddressValue"]?.ToString() ?? string.Empty);
                    permissionFull.ChannelPostalAddressList = BuildCustomerAddressList(row["ChannelPostAddressList"]?.ToString() ?? string.Empty);
                    permissionFull.ChannelEmailList = (row["ChannelEmailAddressList"]?.ToString() ?? string.Empty).Trim().TrimEnd(',').Split(',').ToList();
                    permissionFull.ChannelPhoneNoList = (row["ChannelPhoneNoList"]?.ToString() ?? string.Empty).Trim().TrimEnd(',').Split(',').ToList();
                    permissionFull.ChannelSmsNoList = (row["ChannelSMSNoList"]?.ToString() ?? string.Empty).Trim().TrimEnd(',').Split(',').ToList();

                }

                if (pkAllocated > 0)
                {
                    DataTable dt = ResponsePermissionsCompany(pkAllocated, parameter);
                    permissionFull.PermissionCategory = new List<ChannelFlags>();
                    if (dt != null)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            ChannelFlags ch = new ChannelFlags
                            {
                                PermissionCategoryDisplayValue = row["PermissionCategoryDisplayValue"]?.ToString() ?? string.Empty,
                                PermissionCategoryStatus = row["PermissionCategoryStatus"]?.ToString() ?? string.Empty,
                                ChannelPostFlag = row["ChannelPostStatus"]?.ToString() ?? string.Empty,
                                ChannelEmailFlag = row["ChannelEmailStatus"]?.ToString() ?? string.Empty,
                                ChannelPhoneNoFlag = row["ChannelPhoneStatus"]?.ToString() ?? string.Empty,
                                ChannelSmsFlag = row["ChannelSMSStatus"]?.ToString() ?? string.Empty,
                                ReConsentRequired = row["ReconsentRequired"]?.ToString().ToBoolParsed(),
                                LastUpdatedDate = row["StatusCapturedDate"] == null ? (DateTime?)null : row["StatusCapturedDate"].ToString().ToDateParsed()
                            };
                            permissionFull.PermissionCategory.Add(ch);
                        }
                    }
                }
            }
            catch (InvalidCastException invalidCastException)
            {
                _logger.Error(
                    "BuildPermissionsSummary : ErrorTag: " + ErrorTagProvider.ErrorTagDatabase + " -- " +
                    invalidCastException.Message, invalidCastException);
                throw new Exception(string.Format(DatabaseMessage.DatabaseException, ErrorTagProvider.ErrorTagDatabase));
            }
            catch (Exception ex)
            {
                _logger.Error(
                    "BuildPermissionsSummary : ErrorTag: " + ErrorTagProvider.ErrorTagDatabase + " -- " + ex.Message, ex);
                throw new Exception(string.Format(DatabaseMessage.DatabaseException, ErrorTagProvider.ErrorTagDatabase));
            }
            return permissionFull;
        }

        private PermissionSpecified BuildPermissionSpecified(DataSet dsSpecified, ReturnMePermissions permission)
        {
            PermissionSpecified permissionSpecified = new PermissionSpecified();
            var table = dsSpecified.Tables[0];
            try
            {
                int pkAllocated = 0;
                foreach (DataRow row in table.Rows)
                {
                    permissionSpecified.PermissionId = row["PermissionsID"].ToString().ToIntParsed();
                    pkAllocated = row["PKAllocated"].ToString().ToIntParsed();
                    permissionSpecified.Source = row["Source"]?.ToString() ?? string.Empty;
                    permissionSpecified.Hac = row["HAC"].ToString().ToBool();
                    permissionSpecified.ReConsentRequiredCore = row["ReConsentRequiredCore"].ToString().ToBool();
                    permissionSpecified.Journey = row["Journey"]?.ToString() ?? string.Empty;
                    permissionSpecified.JourneyType = row["JourneyType"]?.ToString() ?? string.Empty;
                    permissionSpecified.LastUpdatedDate = row["LastUpdatedDate"] == DBNull.Value ? (DateTime?)null : DateTime.Parse(row["LastUpdatedDate"].ToString());
                    permissionSpecified.LastUpdatedAgentName = row["LastUpdatedAgentName"]?.ToString() ?? string.Empty;
                    permissionSpecified.ChannelEmailAddress = row["ChannelEmailAddressValue"]?.ToString() ?? string.Empty;
                    permissionSpecified.ChannelPhoneNo = row["ChannelPhoneNoValue"]?.ToString() ?? string.Empty;
                    permissionSpecified.ChannelSmsNo = row["ChannelSMSNoValue"]?.ToString() ?? string.Empty;

                    //Have to fix the data
                    permissionSpecified.ChannelPostalAddress =
                        BuildCustomerAddress(row["ChannelPostAddressValue"]?.ToString() ?? string.Empty);
                    permissionSpecified.ChannelPostalAddressList = BuildCustomerAddressList(row["ChannelPostAddressList"]?.ToString() ?? string.Empty);
                    permissionSpecified.ChannelEmailList = (row["ChannelEmailAddressList"]?.ToString() ?? string.Empty).Trim().TrimEnd(',').Split(',').ToList();
                    permissionSpecified.ChannelPhoneNoList = (row["ChannelPhoneNoList"]?.ToString() ?? string.Empty).Trim().TrimEnd(',').Split(',').ToList();
                    permissionSpecified.ChannelSmsNoList = (row["ChannelSMSNoList"]?.ToString() ?? string.Empty).Trim().TrimEnd(',').Split(',').ToList();
                }

                if (pkAllocated > 0)
                {
                    permissionSpecified = LoadSpecifiedCorePermissions(pkAllocated, permissionSpecified, permission);
                }
            }
            catch (InvalidCastException invalidCastException)
            {
                _logger.Error(
                    "BuildPermissionsSummary : ErrorTag: " + ErrorTagProvider.ErrorTagDatabase + " -- " +
                    invalidCastException.Message, invalidCastException);
                throw new Exception(string.Format(DatabaseMessage.DatabaseException, ErrorTagProvider.ErrorTagDatabase));
            }
            catch (Exception ex)
            {
                _logger.Error(
                    "BuildPermissionsSummary : ErrorTag: " + ErrorTagProvider.ErrorTagDatabase + " -- " + ex.Message, ex);
                throw new Exception(string.Format(DatabaseMessage.DatabaseException, ErrorTagProvider.ErrorTagDatabase));
            }
            return permissionSpecified;
        }

        private PermissionSpecified LoadSpecifiedCorePermissions(long pkAllocated, PermissionSpecified permissionSpecified, ReturnMePermissions permission)
        {

            DataTable dt = ResponsePermissionsCompany(pkAllocated, permission);
            permissionSpecified.PermissionCategory = new List<ChannelFlags>();

            if (dt != null)
            {
                foreach (DataRow row in dt.Rows)
                {
                    ChannelFlags ch = new ChannelFlags
                    {
                        PermissionCategoryDisplayValue = row["PermissionCategoryDisplayValue"]?.ToString() ?? string.Empty,
                        PermissionCategoryStatus = row["PermissionCategoryStatus"]?.ToString() ?? string.Empty,
                        ChannelPostFlag = row["ChannelPostStatus"]?.ToString() ?? string.Empty,
                        ChannelEmailFlag = row["ChannelEmailStatus"]?.ToString() ?? string.Empty,
                        ChannelPhoneNoFlag = row["ChannelPhoneStatus"]?.ToString() ?? string.Empty,
                        ChannelSmsFlag = row["ChannelSMSStatus"]?.ToString() ?? string.Empty,
                        ReConsentRequired = row["ReconsentRequired"]?.ToString().ToBoolParsed(),
                        LastUpdatedDate = row["StatusCapturedDate"] == null ? (DateTime?)null : row["StatusCapturedDate"].ToString().ToDateParsed()
                    };
                    permissionSpecified.PermissionCategory.Add(ch);
                }
            }

            return permissionSpecified;
        }

        #endregion
    }
}