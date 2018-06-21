using log4net;
using Saga.Gmd.WebApiServices.Common;
using Saga.Gmd.WebApiServices.Models.Membership;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using Saga.Gmd.WebApiServices.DAL.Extensions;
using Saga.Gmd.WebApiServices.DAL.Infrastructure;
using Saga.Gmd.WebApiServices.Models;
using Saga.Gmd.WebApiServices.Models.Customer;


namespace Saga.Gmd.WebApiServices.DAL.Membership
{
    public class MembershipDataAccess : DataAccessBase, IMembershipDataAccess
    {
        public string MembershipConnection { get; set; }
        public string MciCrDbConnectionString { get; set; }

        public bool CanDump { get; }
        private readonly ILog _logger;
        private readonly IConnectionFactory _connectionFactory;

        public MembershipDataAccess(
            ILog logger, 
            IConnectionFactory connectionFactory)
        {
            MembershipConnection = ConfigurationManager.AppSettings["MembershipConnection"];
            MciCrDbConnectionString = ConfigurationManager.AppSettings["MciCrConnection"];
            bool enableDump;
            bool.TryParse(ConfigurationManager.AppSettings["EnableObjectDump"], out enableDump);
            CanDump = enableDump;
            _logger = logger;
            _connectionFactory = connectionFactory;
        }

        public MembershipDetails GetMembershipDetails(int custRefKey)
        {

            MembershipDetails membershipDetails;

            using (var conn = new SqlConnection(MembershipConnection))
            {
                using (
                    var cmd = new SqlCommand("[dbo].[get_membership_details]", conn))
                {
                    try
                    {
                        cmd.Connection.Open();

                        var param = new SqlParameter("@p_id", SqlDbType.Int);
                        param.SqlValue = custRefKey;
                        cmd.Parameters.Add(param);
                        cmd.CommandType = CommandType.StoredProcedure;

                        var ds = new DataSet();
                        var adapter = new SqlDataAdapter(cmd);
                        adapter.Fill(ds);

                        membershipDetails = BuildMembershipDetails(ds);
                    }
                    catch (SqlException ex)
                    {
                        _logger.Error(
                            "GetMembershipDetails : ErrorTag: " + ErrorTagProvider.ErrorTagDatabase + " -- " +
                            ex.Message, ex);
                        throw new Exception(string.Format(DatabaseMessage.DatabaseException,
                            ErrorTagProvider.ErrorTagDatabase));
                    }
                    catch (Exception ex)
                    {
                        _logger.Error("GetMembershipDetails : ErrorTag: " + ErrorTagProvider.ErrorTagDatabase + " -- Message : " + ex.Message + " -- " + ex);

                        if(CanDump)
                            _logger.Error($"Parameters GetMembershipDetails:- CustomerReferenceKey={custRefKey}");

                        DumpModel.Dump(custRefKey, _logger, CanDump);
                        _logger.Info("--------------------------------");
                        throw new Exception(string.Format("Sorry, Something went wrong ! Please contact GMD team with '" + ErrorTagProvider.ErrorTagDatabase + "' code for more information."));
                    }
                }

            }
            return membershipDetails;
        }

        private MembershipDetails BuildMembershipDetails(DataSet ds)
        {
            MembershipDetails details = new MembershipDetails();
            var table = ds.Tables[0];
            try
            {
                foreach (DataRow row in table.Rows)
                {
                    details.CustomerId= row["customer_id"]?.ToString() ?? string.Empty;
                    details.ActivationId = row["activation_id"]?.ToString() ?? string.Empty;
                    details.EncryptedActivationId = row["encrypted_activation_id"]?.ToString() ?? string.Empty;

                    details.IsEligible = row["is_eligible"].ToString().ToBoolParsed();

                    details.MembershipNo = row["membership_no"] != DBNull.Value
                        ? long.Parse(row["membership_no"].ToString())
                        : (long?)null;
                    details.EncryptedMembershipNo = row["encrypted_membership_no"]?.ToString() ?? string.Empty;
                    details.ActivationStatus = row["activation_status"]?.ToString() ?? string.Empty;
                    details.ActivationSource = row["activation_source"]?.ToString() ?? string.Empty;
                    details.ActivationDate = row["activation_date"] == DBNull.Value
                        ? (DateTime?)null
                        : DateTime.Parse(row["activation_date"].ToString());
                    details.ActivationDeclineReason = row["activation_decline_reason"]?.ToString() ?? string.Empty;
                    details.MembershipStatus = row["membership_status"]?.ToString() ?? string.Empty;
                    details.MembershipStatusReason = row["membership_status_reason"]?.ToString() ?? string.Empty;
                    details.IsOverride = row["override_flag"]?.ToString().ToBoolParsed();
                    details.OverrideReason = row["override_reason"]?.ToString() ?? string.Empty;

                    // GITCS-1 
                    details.WelcomePackChoiceDbValue = row["welcome_pack_choice"]?.ToString() ?? string.Empty;
                    details.MembershipStartDate = row["membership_start_date"] == DBNull.Value
                        ? (DateTime?)null
                        : DateTime.Parse(row["membership_start_date"].ToString());
                    details.MembershipExpiryDate = row["membership_expiry_date"] == DBNull.Value
                        ? (DateTime?)null
                        : DateTime.Parse(row["membership_expiry_date"].ToString());
                    details.Products = row["products"]?.ToString() ?? string.Empty;
                    details.ProductLinkedtoExpiryDate = row["product_linkedto_expiry_date"]?.ToString() ?? string.Empty;
                    details.MembershipLevel = row["membership_level"]?.ToString() ?? string.Empty;
                    details.MembershipLevelEver = row["membership_level_ever"]?.ToString() ?? string.Empty;
                    details.IsHac = row["HAC_flag"].ToString().ToBoolParsed();
                    details.AgentName = row["agent_name"]?.ToString() ?? string.Empty;
                    details.LastContactSource = row["last_contact_source"]?.ToString() ?? string.Empty;
                    details.IsActive = row["active_flag"]?.ToString().ToBoolParsed();
                    details.IsNewBusiness = row["new_business_flag"]?.ToString().ToBoolParsed();
                    details.IsEnagementStatus = row["engagement_status_flag"]?.ToString().ToBoolParsed();
                    details.CanPrompt = row["prompt_flag"]?.ToString().ToBoolParsed();
                    details.IsAssociateMember = row["associate_member_flag"]?.ToString().ToBoolParsed();
                    details.IsEligibleCustomerNewName = row["eligible_customer_new_name_flag"]?.ToString().ToBoolParsed();
                    details.LastStatusChangeDate = row["last_status_change_date"] == DBNull.Value
                        ? (DateTime?)null
                        : DateTime.Parse(row["last_status_change_date"].ToString());

                }
            }
            catch (InvalidCastException ex)
            {

                _logger.Error("BuildMembershipDetails : ErrorTag: " + ErrorTagProvider.ErrorTagDatabase + " -- Message : " + ex.Message + " -- " + ex);
                DumpModel.Dump(ds, _logger, CanDump);
                _logger.Info("--------------------------------");
                throw new Exception(string.Format("Sorry, Something went wrong ! Please contact GMD team with '" + ErrorTagProvider.ErrorTagDatabase + "' code for more information."));


            }
            catch (Exception ex)
            {
                _logger.Error("BuildMembershipDetails : ErrorTag: " + ErrorTagProvider.ErrorTagDatabase + " -- Message : " + ex.Message + " -- " + ex);
                DumpModel.Dump(ds, _logger, CanDump);
                _logger.Info("--------------------------------");
                throw new Exception(string.Format("Sorry, Something went wrong ! Please contact GMD team with '" + ErrorTagProvider.ErrorTagDatabase + "' code for more information."));
            }
            return details;
        }

        public List<MembershipOptions> GetMembershipOptions(string option)
        {
            List<MembershipOptions> membershipOptions = new List<MembershipOptions>();
            List<string> options = new List<string>();
            using (var conn = new SqlConnection(MembershipConnection))
            {
                using (
                    var cmd = new SqlCommand("[dbo].[get_membership_options]", conn))
                {
                    try
                    {
                        cmd.Connection.Open();

                        var param = new SqlParameter("@codeList", SqlDbType.VarChar, 50);
                        param.SqlValue = option;
                        cmd.Parameters.Add(param);
                        cmd.CommandType = CommandType.StoredProcedure;

                        var ds = new DataSet();
                        var adapter = new SqlDataAdapter(cmd);
                        adapter.Fill(ds);

                        var table = ds.Tables[0];
                        foreach (DataRow row in table.Rows)
                        {
                            var optionItem = new MembershipOptions();
                            optionItem.CodeList = row["CODE_LIST"]?.ToString() ?? string.Empty;
                            optionItem.Shortcode = row["SHORT_CODE"]?.ToString() ?? string.Empty;
                            optionItem.Description = row["CODE_DESCRIPTION"]?.ToString() ?? string.Empty;
                            membershipOptions.Add(optionItem);
                        }
                    }
                    //catch (SqlException ex)
                    //{
                    //    _logger.Error("GetMembershipOptions : ErrorTag: " + ErrorTagProvider.ErrorTagDatabase + " -- Message : " + ex.Message + " -- " + ex);
                    //    DumpModel.Dump(option, _logger, CanDump);
                    //    _logger.Info("--------------------------------");
                    //    throw new Exception(string.Format("Sorry, Something went wrong ! Please contact GMD team with '" + ErrorTagProvider.ErrorTagDatabase + "' code for more information."));
                    //}
                    catch (Exception ex)
                    {

                        _logger.Error("GetMembershipOptions : ErrorTag: " + ErrorTagProvider.ErrorTagDatabase + " -- Message : " + ex.Message + " -- " + ex);

                        if (CanDump)
                            _logger.Error($"Parameters GetMembershipOptions:- Option={option}");


                        DumpModel.Dump(option, _logger, CanDump);
                        _logger.Info("--------------------------------");
                        throw new Exception(string.Format("Sorry, Something went wrong ! Please contact GMD team with '" + ErrorTagProvider.ErrorTagDatabase + "' code for more information."));
                    }
                }
            }
            return membershipOptions;
        }

        public List<string> GetMembershipStatus()
        {
            return GetOptions("MEMBERSHIP_STATUS");
        }

        public List<string> GetMembershipDeclineReason()
        {
            return GetOptions("ACTIVATION_DECLINE_REASON");
        }

        public List<string> GetActivationDeclineReason()
        {
            return GetOptions("ACTIVATION_DECLINE_REASON");
        }

        public List<string> GetOverrideReason()
        {
            return GetOptions("OVERRIDE_REASON");
        }

        public List<string> GetMembershipStatusReason()
        {
            return GetOptions("MEMBERSHIP_STATUS_REASON");
        }

        public List<string> GetMembershipCancelReason()
        {
            return GetOptions("MEMBERSHIP_CANCEL_REASON");
        }

        public List<string> GetFulfilmentOverride()
        {
            return GetOptions("WELCOME_PACK_CHOICE");
        }

        public List<string> GetActivationSource()
        {
            return GetOptions("ACTIVATION_SOURCE");
        }

        private List<string> GetOptions(string optionName)
        {
            List<MembershipOptions> options = GetMembershipOptions(optionName);

            List<string> optionsList = new List<string>();
            foreach (var item in options)
            {
                optionsList.Add(item.Description);
            }

            return optionsList;
        }


        public string DeclineMembership(MembershipDataInput input)
        {
            string message = "Member not found!";

            using (var conn = new SqlConnection(MembershipConnection))
            {
                using (
                    var cmd = new SqlCommand("[dbo].[decline_membership]", conn))
                {
                    try
                    {
                        cmd.Connection.Open();

                        var param = new SqlParameter("@p_activation_id", SqlDbType.VarChar);
                        param.SqlValue = string.IsNullOrEmpty(input.ActivationId) ? input.EncryptedActivationId : input.ActivationId;
                        cmd.Parameters.Add(param);

                        var param1 = new SqlParameter("@p_agent_name", SqlDbType.VarChar, 100);
                        param1.SqlValue = input.AgentName;
                        cmd.Parameters.Add(param1);

                        var param2 = new SqlParameter("@p_source_system", SqlDbType.VarChar, 4);
                        param2.SqlValue = input.SourceSystem;
                        cmd.Parameters.Add(param2);

                        var param3 = new SqlParameter("@p_activation_source", SqlDbType.VarChar, 50);
                        param3.SqlValue = input.ActivationSource;
                        cmd.Parameters.Add(param3);

                        var param4 = new SqlParameter("@p_decline_reason", SqlDbType.VarChar, 50);
                        param4.SqlValue = input.DeclineReason;
                        cmd.Parameters.Add(param4);

                        var errorMessage = new SqlParameter("@p_message", SqlDbType.VarChar, 1000);
                        errorMessage.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(errorMessage);

                        cmd.CommandType = CommandType.StoredProcedure;


                        int count = cmd.ExecuteNonQuery();
                        message = count > 0 ? "Membership data updated successfully" : errorMessage.SqlValue?.ToString();
                    }
                    //catch (SqlException ex)
                    //{
                    //    _logger.Error("DeclineMembership : ErrorTag: " + ErrorTagProvider.ErrorTagDatabase + " -- Message : " + ex.Message + " ---" + message + " -- " + ex);
                    //    DumpModel.Dump(input, _logger, CanDump);
                    //    _logger.Info("--------------------------------");
                    //    throw new Exception(string.Format("Sorry, Something went wrong ! Please contact GMD team with '" + ErrorTagProvider.ErrorTagDatabase + "' code for more information."));
                    //}
                    catch (Exception ex)
                    {
                        _logger.Error("DeclineMembership : ErrorTag: " + ErrorTagProvider.ErrorTagDatabase + " -- Message : " + ex.Message + " ---" + message + " -- " + ex);

                        if (CanDump)
                            _logger.Error($"Parameters DeclineMembership:- ActivationId={input.ActivationId}, EncryptedId={input.EncryptedActivationId}, " +
                                          $"AgentName={input.AgentName},SourceSystem={input.SourceSystem},ActivationSource={input.ActivationSource}");

                        DumpModel.Dump(input, _logger, CanDump);

                        _logger.Info("--------------------------------");
                        throw new Exception(string.Format("Sorry, Something went wrong ! Please contact GMD team with '" + ErrorTagProvider.ErrorTagDatabase + "' code for more information."));
                    }
                }
            }

            return message;
        }


        public string CancelMembership(MembershipDataInput input)
        {
            string message = "Member not found!";

            using (IDbConnection db = _connectionFactory.GetConnection(MembershipConnection))
            {
                try
                {
                    string sProc = "[dbo].[cancel_membership]";
                    DynamicParameters _params = new DynamicParameters()
                        .AddParameter("@p_membership_no", dbType: DbType.String, value: input.CancelationOrActivationKey, size: 50)
                        .AddParameter("@p_cancellation_reason", dbType: DbType.String, value: input.CancellationReason, size: 50)
                        .AddParameter("@p_message", dbType: DbType.String, direction: ParameterDirection.Output, size: 1000);
                    /*
                    cmd.Parameters.AddParameter(new SqlParameter("@p_membership_no", SqlDbType.VarChar, 50).SetSqlValue(input.CancelationOrActivationKey))
                        .AddParameter(new SqlParameter("@p_cancellation_reason", SqlDbType.VarChar, 50).SetSqlValue(null))  // Accept the proc default value 
                        .AddParameter(new SqlParameter("@p_message", SqlDbType.VarChar, 1000).SetDirection(ParameterDirection.Output));
                        */

                    int count = db.Execute(sProc, _params, commandType: CommandType.StoredProcedure);
                    if (count > 0)
                        message = "Membership cancelled successfully";

                    var procMessage = _params.Get<string>("@p_message");
                    if (!string.IsNullOrEmpty(procMessage) && procMessage.ToLower() != "null")
                    {
                        throw new Exception(procMessage);
                    }

                }
                catch (Exception ex)
                {
                    _logger.Error("CancelMembership : ErrorTag: " + ErrorTagProvider.ErrorTagDatabase + " -- " + ex.Message, ex);

                    if (CanDump)
                        _logger.Error(
                            $"Parameters CancelMembership:- MembershipNo={input.MembershipNo}, EncryptedMembershipNo={input.EncryptedMembershipNo} "
                        );


                    DumpModel.Dump(input, _logger, CanDump);
                    _logger.Info("--------------------------------");
                    throw new Exception(string.Format("Sorry, Something went wrong ! Please contact GMD team with '" + ErrorTagProvider.ErrorTagDatabase + "' code for more information."));
                }
            }
            return message;

            /* Old ADO Block 
            using (var conn = new SqlConnection(MembershipConnection))
            {
                using (
                    var cmd = new SqlCommand("[dbo].[cancel_membership]", conn))
                {
                    try
                    {
                        cmd.Connection.Open();

                        cmd.Parameters.AddParameter( new SqlParameter("@p_membership_no", SqlDbType.VarChar, 50).SetSqlValue(input.CancelationOrActivationKey))
                            .AddParameter( new SqlParameter("@p_cancellation_reason", SqlDbType.VarChar, 50).SetSqlValue( null ))  // Accept the proc default value 
                            .AddParameter( new SqlParameter("@p_message", SqlDbType.VarChar, 1000).SetDirection(ParameterDirection.Output));

                        cmd.CommandType = CommandType.StoredProcedure;


                        int count = cmd.ExecuteNonQuery();
                        if (count > 0)
                            message = "Membership cancelled successfully";

                        var procMessage = cmd.Parameters["@p_message"].Value.ToString();
                        if (!string.IsNullOrEmpty(procMessage) && procMessage.ToLower() != "null")
                        {
                            throw new Exception(procMessage);
                        }
                    }
                    //catch (SqlException ex)
                    //{

                    //    _logger.Error("UpdateMembership : ErrorTag: " + ErrorTagProvider.ErrorTagDatabase + " -- Message : " + ex.Message + " -- " + errorMessage, ex);
                    //    DumpModel.Dump(input, _logger, CanDump);
                    //    _logger.Info("--------------------------------");
                    //    throw new Exception(string.Format("Sorry, Something went wrong ! Please contact GMD team with '" + ErrorTagProvider.ErrorTagDatabase + "' code for more information."));
                    //}

                    catch (Exception ex)
                    {
                        _logger.Error("CancelMembership : ErrorTag: " + ErrorTagProvider.ErrorTagDatabase + " -- " + ex.Message, ex);

                        if (CanDump)
                            _logger.Error(
                                $"Parameters CancelMembership:- MembershipNo={input.MembershipNo}, EncryptedMembershipNo={input.EncryptedMembershipNo} " 
                          );


                        DumpModel.Dump(input, _logger, CanDump);
                        _logger.Info("--------------------------------");
                        throw new Exception(string.Format("Sorry, Something went wrong ! Please contact GMD team with '" + ErrorTagProvider.ErrorTagDatabase + "' code for more information."));
                    }
                }
                return message;
            }
            */
        }

        public string CreateTempMembership(MembershipDataInput input)
        {
            SqlParameter outputCusomterId = null;
            string _errorMessage = string.Empty;
            using (var conn = new SqlConnection(MembershipConnection))
            {
                using (
                    var cmd = new SqlCommand("[dbo].[add_temporary_member]", conn))
                {
                    try
                    {
                        cmd.Connection.Open();

                        var param = new SqlParameter("@p_agent_name", SqlDbType.VarChar, 50);
                        param.SqlValue = input.AgentName;
                        cmd.Parameters.Add(param);

                        if (!string.IsNullOrEmpty(input.OverrideReason))
                        {
                            var param1 = new SqlParameter("@p_override_reason", SqlDbType.VarChar, 50);
                            param1.SqlValue = input.OverrideReason;
                            cmd.Parameters.Add(param1);
                        }

                        var param2 = new SqlParameter("@p_division", SqlDbType.VarChar, 25);
                        param2.SqlValue = input.Division;
                        cmd.Parameters.Add(param2);

                        var param3 = new SqlParameter("@p_product_code", SqlDbType.VarChar, 20);
                        param3.SqlValue = input.ProductCode;
                        cmd.Parameters.Add(param3);

                        if (!string.IsNullOrEmpty(input.SourceSystem))
                        {
                            var param4 = new SqlParameter("@p_source_system", SqlDbType.VarChar, 6);
                            param4.SqlValue = input.SourceSystem;
                            cmd.Parameters.Add(param4);
                        }
                        if (!string.IsNullOrEmpty(input.ActivationSource))
                        {
                            var param5 = new SqlParameter("@p_activation_source", SqlDbType.VarChar, 50);
                            param5.SqlValue = input.ActivationSource;
                            cmd.Parameters.Add(param5);
                        }

                        if (!string.IsNullOrEmpty(input.FulfilmentOverride))
                        {
                            var param6 = new SqlParameter("@p_welcome_pack_choice", SqlDbType.VarChar, 50);
                            param6.SqlValue = input.FulfilmentOverride;
                            cmd.Parameters.Add(param6);
                        }

                        if (input.Premium != null && input.Premium > 0)
                        {
                            var param7 = new SqlParameter("@p_premium", SqlDbType.Decimal);
                            param7.SqlValue = input.Premium;
                            cmd.Parameters.Add(param7);
                        }

                        if (input.Revenue != null && input.Revenue > 0)
                        {
                            var param8 = new SqlParameter("@p_revenue", SqlDbType.Decimal);
                            param8.SqlValue = input.Revenue;
                            cmd.Parameters.Add(param8);
                        }

                        if (input.TransactionDate != null)
                        {
                            var param9 = new SqlParameter("@p_transaction_date", SqlDbType.DateTime);
                            param9.SqlValue = input.TransactionDate?.ToReturnIsoDateIfValid();
                            cmd.Parameters.Add(param9);
                        }

                        if (input.StartDate != null)
                        {
                            var param10 = new SqlParameter("@p_start_date", SqlDbType.DateTime);
                            param10.SqlValue = input.StartDate?.ToReturnIsoDateIfValid();
                            cmd.Parameters.Add(param10);
                        }

                        var param11 = new SqlParameter("@p_end_date", SqlDbType.DateTime);
                        param11.SqlValue = DateTime.Parse(input.EndDate.ToString("yyyy-MM-dd HH:mm:ss"));
                        cmd.Parameters.Add(param11);

                        if (!string.IsNullOrEmpty(input.CustNameAndAddress.Title))
                        {
                            var param12 = new SqlParameter("@p_title", SqlDbType.VarChar, 30);
                            param12.SqlValue = input.CustNameAndAddress.Title;
                            cmd.Parameters.Add(param12);
                        }
                        if (!string.IsNullOrEmpty(input.CustNameAndAddress.FirstName))
                        {
                            var param13 = new SqlParameter("@p_first_name", SqlDbType.VarChar, 50);
                            param13.SqlValue = input.CustNameAndAddress.FirstName;
                            cmd.Parameters.Add(param13);
                        }
                        if (!string.IsNullOrEmpty(input.CustNameAndAddress.Surname))
                        {
                            var param14 = new SqlParameter("@p_surname", SqlDbType.VarChar, 50);
                            param14.SqlValue = input.CustNameAndAddress.Surname;
                            cmd.Parameters.Add(param14);
                        }

                        if (input.CustNameAndAddress.Dob != null)
                        {
                            var param15 = new SqlParameter("@p_dob", SqlDbType.DateTime);
                            param15.SqlValue = input.CustNameAndAddress.Dob?.ToReturnIsoDateIfValid();
                            cmd.Parameters.Add(param15);
                        }

                        if (input.CustNameAndAddress.Address.Postcode != null)
                        {
                            var param15 = new SqlParameter("@p_postcode", SqlDbType.VarChar, 8);
                            param15.SqlValue = input.CustNameAndAddress.Address.Postcode;
                            cmd.Parameters.Add(param15);
                        }

                        if (!(string.IsNullOrEmpty(input.CustNameAndAddress.Address.Address1)))
                        {
                            var param16 = new SqlParameter("@p_address1", SqlDbType.VarChar, 100);
                            param16.SqlValue = input.CustNameAndAddress.Address.Address1;
                            cmd.Parameters.Add(param16);
                        }

                        if (!string.IsNullOrEmpty(input.CustNameAndAddress.Address.Address2))
                        {
                            var param16 = new SqlParameter("@p_address2", SqlDbType.VarChar, 100);
                            param16.SqlValue = input.CustNameAndAddress.Address.Address2;
                            cmd.Parameters.Add(param16);
                        }
                        if (!string.IsNullOrEmpty(input.CustNameAndAddress.Address.Address3))
                        {
                            var param17 = new SqlParameter("@p_address3", SqlDbType.VarChar, 100);
                            param17.SqlValue = input.CustNameAndAddress.Address.Address3;
                            cmd.Parameters.Add(param17);
                        }
                        if (!string.IsNullOrEmpty(input.CustNameAndAddress.Address.Address4))
                        {
                            var param18 = new SqlParameter("@p_address4", SqlDbType.VarChar, 100);
                            param18.SqlValue = input.CustNameAndAddress.Address.Address4;
                            cmd.Parameters.Add(param18);
                        }
                        if (!string.IsNullOrEmpty(input.CustNameAndAddress.Email))
                        {
                            var param18 = new SqlParameter("@p_email", SqlDbType.VarChar, 50);
                            param18.SqlValue = input.CustNameAndAddress.Email;
                            cmd.Parameters.Add(param18);
                        }
                        if (!string.IsNullOrEmpty(input.CustNameAndAddress.Phone))
                        {
                            var param19 = new SqlParameter("@p_phone", SqlDbType.VarChar, 20);
                            param19.SqlValue = input.CustNameAndAddress.Phone;
                            cmd.Parameters.Add(param19);
                        }

                        var message = new SqlParameter("@p_message", SqlDbType.VarChar, 1000);
                        message.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(message);

                        outputCusomterId = new SqlParameter("@p_customer_id", SqlDbType.Int);
                        outputCusomterId.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(outputCusomterId);


                        cmd.CommandType = CommandType.StoredProcedure;
                        var count = cmd.ExecuteNonQuery();

                        if (count == -1)
                        {
                            if (!string.IsNullOrEmpty(message.SqlValue.ToString()) && message.SqlValue.ToString().ToLower() != "null")
                            {
                                _errorMessage = message.SqlValue.ToString();
                                throw new ActivationException(message.SqlValue.ToString());
                            }
                        }


                        if (count > 0)
                            return "Membership data updated successfully";
                        else
                            return _errorMessage;
                    }
                    //catch (SqlException ex)
                    //{

                    //    _logger.Error("CreateTempMembership : ErrorTag: " + ErrorTagProvider.ErrorTagDatabase + " -- Message : " + ex.Message + " ---" + _errorMessage + " -- " + ex);
                    //    DumpModel.Dump(input, _logger, CanDump);
                    //    _logger.Info("--------------------------------");
                    //    throw new Exception(string.Format("Sorry, Something went wrong ! Please contact GMD team with '" + ErrorTagProvider.ErrorTagDatabase + "' code for more information."));
                    //}

                    catch (Exception ex)
                    {
                        _logger.Error("CreateTempMembership : ErrorTag: " + ErrorTagProvider.ErrorTagDatabase + " -- Message : " + ex.Message + " ---" + _errorMessage + " -- " + ex);

                        if (CanDump)
                            _logger.Error($"Parameters CreateTempMembership:- AgentName={input.AgentName}, OverrideReason={input.OverrideReason}, " +
                                          $" Division={input.Division}, ProductionCode={input.ProductCode}, SourceSystem={input.SourceSystem}, ActivationSource={input.ActivationSource}," +
                                          $" FulfilmentOverride ={input.FulfilmentOverride}, Premium={input.Premium}, Revenue={input.Revenue}, TransactionDate={input.TransactionDate}" +
                                          $" StartDate={input.StartDate}, EndDate={input.EndDate}, Title={input.CustNameAndAddress.Title}, FirstName={input.CustNameAndAddress.FirstName}" +
                                          $" SurName={input.CustNameAndAddress.Surname}, Dob={input.CustNameAndAddress.Dob}, PostCode={input.CustNameAndAddress.Address.Postcode}" +
                                          $" Address1={input.CustNameAndAddress.Address.Address1}, Address2={input.CustNameAndAddress.Address.Address2}, Address3={input.CustNameAndAddress.Address.Address3}" +
                                          $" Address4={input.CustNameAndAddress.Address.Address4}, Email={input.CustNameAndAddress.Email}, Phone={input.CustNameAndAddress.Phone}");

                        DumpModel.Dump(input, _logger, CanDump);
                        _logger.Info("--------------------------------");
                        throw new Exception(string.Format("Sorry, Something went wrong ! Please contact GMD team with '" + ErrorTagProvider.ErrorTagDatabase + "' code for more information."));
                    }

                }
            }
        }

        public string SetEligible(MembershipDataInput input)
        {
            string message;
            using (IDbConnection db = _connectionFactory.GetConnection(MembershipConnection))
            {
                try
                {
                    string sProc = "[dbo].[set_membership_eligible]";
                    DynamicParameters _params = new DynamicParameters();
                    _params.AddParameter(name: "@p_id", dbType: DbType.String, value: input.ProductSoldKey, size: 50, direction: ParameterDirection.Input)
                           .AddParameter(name: "@p_agent_name", dbType: DbType.String, value: input.AgentName, size: 100, direction: ParameterDirection.Input)
                           .AddParameter(name: "@p_message", dbType: DbType.String, direction: ParameterDirection.Output, size: 1000, value: null);

                    db.Execute(sProc, _params, commandType: CommandType.StoredProcedure);
                    message = _params.Get<string>("@p_message");


                }
                catch (SqlException ex)
                {
                    _logger.Error(
                        "SetEligible : ErrorTag: " + ErrorTagProvider.ErrorTagDatabase + " -- " +
                        ex.Message, ex);
                    throw new Exception(string.Format(DatabaseMessage.DatabaseException,
                        ErrorTagProvider.ErrorTagDatabase));
                }
                catch (Exception ex)
                {
                    _logger.Error("SetEligible : ErrorTag: " + ErrorTagProvider.ErrorTagDatabase + " -- Message : " + ex.Message + " -- " + ex);

                    if (CanDump)
                        _logger.Error($"Parameters SelEligible:- Id={input.ProductSoldKey}");

                    DumpModel.Dump(input, _logger, CanDump);
                    _logger.Info("--------------------------------");
                    throw new Exception(string.Format("Sorry, Something went wrong ! Please contact GMD team with '" + ErrorTagProvider.ErrorTagDatabase + "' code for more information."));
                }

            }
            return message;
        }


        public IDictionary<string, string> CreateMembership(MembershipParam input)
        {
            string _message = string.Empty;
            IDictionary<string, string> outputIds = new Dictionary<string, string>();
            using (var conn = new SqlConnection(MembershipConnection))
            {
                using (
                    var cmd = new SqlCommand("[dbo].[add_new_member]", conn))
                {
                    try
                    {
                        cmd.Connection.Open();

                        var param5 = new SqlParameter("@p_customer_id", SqlDbType.Int);
                        param5.SqlValue = input.CustomerId;
                        cmd.Parameters.Add(param5);

                        var message = new SqlParameter("@p_message", SqlDbType.VarChar, 1000);
                        message.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(message);

                        var activationIdOutput = new SqlParameter("@p_activation_id", SqlDbType.VarChar, 50);
                        activationIdOutput.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(activationIdOutput);

                        var encryptedActivationIdOutput = new SqlParameter("@p_encrypted_activation_id", SqlDbType.VarChar, 50);
                        encryptedActivationIdOutput.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(encryptedActivationIdOutput);


                        cmd.CommandType = CommandType.StoredProcedure;
                        var count = cmd.ExecuteNonQuery();

                        outputIds.Add("ActivationId", activationIdOutput.SqlValue.ToString());
                        outputIds.Add("EncryptedActivationId", encryptedActivationIdOutput.SqlValue.ToString());


                        _message = message.SqlValue.ToString();
                        if (!string.IsNullOrEmpty(_message) && _message.ToLower() != "null")
                        {
                            throw new Exception(_message);
                        }


                        return outputIds;
                    }
                    //catch (SqlException ex)
                    //{

                    //    _logger.Error("CreateMembership : ErrorTag: " + ErrorTagProvider.ErrorTagDatabase + " -- Message : " + ex.Message + " -- " + ex);
                    //    DumpModel.Dump(input, _logger, CanDump);
                    //    _logger.Info("--------------------------------");
                    //    throw new Exception(string.Format("Sorry, Something went wrong ! Please contact GMD team with '" + ErrorTagProvider.ErrorTagDatabase + "' code for more information."));

                    //}
                    catch (ActivationException ex)
                    {
                        if (CanDump)
                            _logger.Error($"Parameters CreateMembership:- CustomerId={input.CustomerId}");

                        throw new ActivationException(ex.Message);
                    }
                    catch (Exception ex)
                    {
                        _logger.Error("CreateMembership : ErrorTag: " + ErrorTagProvider.ErrorTagDatabase + " -- Message : " + ex.Message + " -- " + ex);

                        if (CanDump)
                            _logger.Error($"Parameters CreateMembership:- CustomerId={input.CustomerId}");

                        DumpModel.Dump(input, _logger, CanDump);
                        _logger.Info("--------------------------------");
                        throw new Exception(string.Format("Sorry, Something went wrong ! Please contact GMD team with '" + ErrorTagProvider.ErrorTagDatabase + "' code for more information."));
                    }
                }
            }
        }

        public string UpdateMembership(MembershipDataInput input)
        {
            string message = "Member not found!";
            string errorMessage = string.Empty;

            using (var conn = new SqlConnection(MembershipConnection))
            {
                using (
                    var cmd = new SqlCommand("[dbo].[activate_membership]", conn))
                {
                    try
                    {
                        cmd.Connection.Open();

                        var param = new SqlParameter("@p_activation_id", SqlDbType.VarChar, 50);
                        param.SqlValue = string.IsNullOrEmpty(input.ActivationId) ? input.EncryptedActivationId : input.ActivationId;
                        cmd.Parameters.Add(param);

                        var param1 = new SqlParameter("@p_agent_name", SqlDbType.VarChar, 100);
                        param1.SqlValue = input.AgentName;
                        cmd.Parameters.Add(param1);

                        var param2 = new SqlParameter("@p_source_system", SqlDbType.VarChar, 4);
                        param2.SqlValue = input.SourceSystem;
                        cmd.Parameters.Add(param2);

                        var param3 = new SqlParameter("@p_activation_source", SqlDbType.VarChar, 50);
                        param3.SqlValue = input.ActivationSource;
                        cmd.Parameters.Add(param3);

                        var param4 = new SqlParameter("@p_override_flag", SqlDbType.Bit);
                        param4.SqlValue = input.OverrideFlag;
                        cmd.Parameters.Add(param4);

                        var param5 = new SqlParameter("@p_override_reason", SqlDbType.VarChar, 50);
                        param5.SqlValue = input.OverrideReason;
                        cmd.Parameters.Add(param5);

                        var param6 = new SqlParameter("@p_welcome_pack_choice", SqlDbType.VarChar, 50);
                        param6.SqlValue = input.FulfilmentOverride;
                        cmd.Parameters.Add(param6);

                        var param7 = new SqlParameter("@p_message", SqlDbType.VarChar, 1000);
                        param7.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(param7);

                        cmd.CommandType = CommandType.StoredProcedure;


                        int count = cmd.ExecuteNonQuery();
                        if (count > 0)
                            message = "Membership data updated successfully";
                        if (!string.IsNullOrEmpty(param7.SqlValue.ToString()) && param7.SqlValue.ToString().ToLower() != "null")
                        {
                            errorMessage = param7.SqlValue.ToString();
                            throw new Exception(param7.SqlValue.ToString());
                        }
                    }
                    //catch (SqlException ex)
                    //{

                    //    _logger.Error("UpdateMembership : ErrorTag: " + ErrorTagProvider.ErrorTagDatabase + " -- Message : " + ex.Message + " -- " + errorMessage, ex);
                    //    DumpModel.Dump(input, _logger, CanDump);
                    //    _logger.Info("--------------------------------");
                    //    throw new Exception(string.Format("Sorry, Something went wrong ! Please contact GMD team with '" + ErrorTagProvider.ErrorTagDatabase + "' code for more information."));
                    //}

                    catch (Exception ex)
                    {
                        _logger.Error("UpdateMembership : ErrorTag: " + ErrorTagProvider.ErrorTagDatabase + " -- " + ex.Message, ex);

                        if (CanDump)
                            _logger.Error($"Parameters UpdateMembership:- ActivationId={input.ActivationId}, EncryptedId={input.EncryptedActivationId}, " +
                                          $" AgentName={input.AgentName},SourceSystem={input.SourceSystem},ActivationSource={input.ActivationSource}" +
                                          $" OverrideFlag={input.OverrideFlag}, OverrideReason={input.OverrideReason}, FulfilmentOverride= {input.FulfilmentOverride}");


                        DumpModel.Dump(input, _logger, CanDump);
                        _logger.Info("--------------------------------");
                        throw new Exception(string.Format("Sorry, Something went wrong ! Please contact GMD team with '" + ErrorTagProvider.ErrorTagDatabase + "' code for more information."));
                    }
                }
                return message;
            }
        }

        public List<MatchedCustomer> GetMatchedTemporaryMember(NameAndAddress p)
        {
            DateTime? dateOfBirth = Common.DateTimeExtensions.GetValidDate(p.Dob);

            var title = string.IsNullOrEmpty(p.Title) ? "null" : p.Title;
            var firstName = string.IsNullOrEmpty(p.FirstName) ? "null" : p.FirstName;
            var surName = string.IsNullOrEmpty(p.Surname) ? "null" : p.Surname;
            //var dob = p.Dob == null || p.Dob == DateTime.MinValue
            //    ? null
            //    : p.Dob.Value.ToReturnIsoDateIfValid();
            var dob = dateOfBirth == null ? null : dateOfBirth.ToReturnIsoNullDateIfValid();
            var email = string.IsNullOrEmpty(p.Email) ? "null" : p.Email;
            var phone = string.IsNullOrEmpty(p.Phone) ? "nul" : p.Phone;
            var postcode = string.IsNullOrEmpty(p.Address.Postcode) ? "null" : p.Address.Postcode;
            var address1 = string.IsNullOrEmpty(p.Address.Address1) ? "null" : p.Address.Address1;
            var address2 = string.IsNullOrEmpty(p.Address.Address2) ? "null," : p.Address.Address2;
            var address3 = string.IsNullOrEmpty(p.Address.Address3) ? "null," : p.Address.Address3;
            var address4 = string.IsNullOrEmpty(p.Address.Address4) ? "null" : p.Address.Address4;

            try
            {
                var matchCustomerFunc = "select customer_id from [dbo].[match_temporary_members_func](@title,@firstname,@surname,@dob,@email,@phone,@postcode,@address1,@address2,@address3,@address4)";
             
                using (SqlConnection conn = new SqlConnection(MembershipConnection))
                {
                    conn.Open();

                    SqlCommand command = new SqlCommand(matchCustomerFunc, conn) {CommandType = CommandType.Text};

                    //command.Parameters.Add(new SqlParameter("title", title));
                    //command.Parameters.Add(new SqlParameter("firstName", firstName));
                    //command.Parameters.Add(new SqlParameter("surname", surName));
                    //command.Parameters.Add(new SqlParameter("dob", dob??(object)DBNull.Value));
                    //command.Parameters.Add(new SqlParameter("email", email));
                    //command.Parameters.Add(new SqlParameter("phone", phone));
                    //command.Parameters.Add(new SqlParameter("postcode", postcode));
                    //command.Parameters.Add(new SqlParameter("address1", address1));
                    //command.Parameters.Add(new SqlParameter("address2", address2));
                    //command.Parameters.Add(new SqlParameter("address3", address3));
                    //command.Parameters.Add(new SqlParameter("address4", address4));

                    //command.Parameters.Add(new SqlParameter("@matchtype", p.MatchType ?? (object)DBNull.Value));
                    command.Parameters.Add(new SqlParameter("@title", p.Title ?? (object)DBNull.Value));
                    command.Parameters.Add(new SqlParameter("@firstName", p.FirstName ?? (object)DBNull.Value));
                    command.Parameters.Add(new SqlParameter("@surname", p.Surname ?? (object)DBNull.Value));
                    command.Parameters.Add(new SqlParameter("@dob", dob ?? (object)DBNull.Value));
                    command.Parameters.Add(new SqlParameter("@email", p.Email ?? (object)DBNull.Value));
                    command.Parameters.Add(new SqlParameter("@phone", p.Phone ?? (object)DBNull.Value));
                    command.Parameters.Add(new SqlParameter("@postcode", p.Address.Postcode ?? (object)DBNull.Value));
                    command.Parameters.Add(new SqlParameter("@address1", p.Address.Address1 ?? (object)DBNull.Value));
                    command.Parameters.Add(new SqlParameter("@address2", p.Address.Address2 ?? (object)DBNull.Value));
                    command.Parameters.Add(new SqlParameter("@address3", p.Address.Address3 ?? (object)DBNull.Value));
                    command.Parameters.Add(new SqlParameter("@address4", p.Address.Address4 ?? (object)DBNull.Value));

                    var adapter = new SqlDataAdapter(command);
                    DataTable table = new DataTable();
                    adapter.Fill(table);
                    adapter.Dispose();
                    var customerIds = BuildCustomerIdList(table);
                    return customerIds;
                }
            }
            catch (Exception ex)
            {

                _logger.Error("GetMatchedTemporaryMember : ErrorTag: " + ErrorTagProvider.ErrorTagDatabase + " -- Message : " + ex.Message + " -- " + ex);


                if (CanDump)
                    _logger.Error($"Parameters GetMatchedTemporaryMember:- Title={title}, FirstName={firstName}, Surname={surName}, Dob={dob}, Email={email}" +
                                  $" Phone={phone}, Postcode={postcode}, Address1= {address1}, Address2={address2}, address3={address3}, address4={address4}");

                DumpModel.Dump(p, _logger, CanDump);
                _logger.Info("--------------------------------");
                throw new Exception(string.Format("Sorry, Something went wrong ! Please contact GMD team with '" + ErrorTagProvider.ErrorTagDatabase + "' code for more information."));
            }

        }

        private static List<MatchedCustomer> BuildCustomerIdList(DataTable table)
        {
            List<MatchedCustomer> ids = new List<MatchedCustomer>();

            var rows = table.Rows;

            foreach (DataRow row in rows)
            {
                if (row["customer_id"] != DBNull.Value)
                {
                    var matchResult =
                        new MatchedCustomer { CustomerId = Convert.ToInt32(row["customer_id"].ToString()) };
                    ids.Add(matchResult);
                }

            }
            return ids;
        }

        /// <summary>
        /// Re-implement with generic return type - for when V2 response is required
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="TR"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public TR GetMembershipDetails<T1, TR>(T1 id) where TR : MembershipDetails
        {
            TR membershipDetails = null;

            dynamic id_value = id;

            using (IDbConnection db = _connectionFactory.GetConnection(MembershipConnection))
            {
                try
                {
                    string readSp = "[dbo].[get_membership_details]";

                    // Custom property mapper used by Dapper here will depend on 
                    // the TR type requested (see DapperConfig.cs). 
                    // membershipdetails will be assigned an instance of this type 
                    membershipDetails = db.QueryFirst<TR>(
                        readSp,
                        new { p_id = id_value },
                        commandType: CommandType.StoredProcedure
                    );

                }
                catch (SqlException ex)
                {
                    _logger.Error(
                        "GetMembershipDetails : ErrorTag: " + ErrorTagProvider.ErrorTagDatabase + " -- " +
                        ex.Message, ex);
                    throw new Exception(string.Format(DatabaseMessage.DatabaseException,
                        ErrorTagProvider.ErrorTagDatabase));
                }
                catch (Exception ex)
                {
                    _logger.Error("GetMembershipDetails : ErrorTag: " + ErrorTagProvider.ErrorTagDatabase + " -- Message : " + ex.Message + " -- " + ex);

                    if (CanDump)
                        _logger.Error($"Parameters GetMembershipDetails:- Id={id}");

                    DumpModel.Dump(id, _logger, CanDump);
                    _logger.Info("--------------------------------");
                    throw new Exception(string.Format("Sorry, Something went wrong ! Please contact GMD team with '" + ErrorTagProvider.ErrorTagDatabase + "' code for more information."));
                }

            }
            return membershipDetails;

        }



        public MembershipDetails GetMembershipDetails(string activationId)
        {
            MembershipDetails membershipDetails;

            using (var conn = new SqlConnection(MembershipConnection))
            {
                using (
                    var cmd = new SqlCommand("[dbo].[get_membership_details]", conn))
                {
                    try
                    {
                        cmd.Connection.Open();

                        var param = new SqlParameter("@p_id", SqlDbType.VarChar);
                        param.SqlValue = activationId;
                        cmd.Parameters.Add(param);
                        cmd.CommandType = CommandType.StoredProcedure;

                        var ds = new DataSet();
                        var adapter = new SqlDataAdapter(cmd);
                        adapter.Fill(ds);
                        adapter.Dispose();

                        membershipDetails = BuildMembershipDetails(ds);
                    }
                    catch (Exception ex)
                    {
                        _logger.Error("GetMembershipDetails : ErrorTag: " + ErrorTagProvider.ErrorTagDatabase + " -- Message : " + ex.Message + " -- " + ex);

                        if (CanDump)
                            _logger.Error($"Parameters GetMembershipDetails:- ActivationId={activationId}");

                        DumpModel.Dump(activationId, _logger, CanDump);
                        _logger.Info("--------------------------------");
                        throw new Exception(string.Format("Sorry, Something went wrong ! Please contact GMD team with '" + ErrorTagProvider.ErrorTagDatabase + "' code for more information."));
                    }
                }

            }
            return membershipDetails;
        }
    }
}

