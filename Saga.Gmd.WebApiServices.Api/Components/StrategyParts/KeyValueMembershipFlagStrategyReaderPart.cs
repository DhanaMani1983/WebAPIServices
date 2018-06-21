using log4net;
using Saga.Gmd.WebApiServices.Api.Models.ParameterModels;
using System;
using System.Linq;
using Saga.Gmd.WebApiServices.Models;
using Saga.Gmd.WebApiServices.Common;
using Saga.Gmd.WebApiServices.DAL.Permissions;
using Saga.Gmd.WebApiServices.Models.Gdpr;
using Saga.Gmd.WebApiServices.Models.Membership;
using Saga.Gmd.WebApiServices.Models.ReturnMe;
using Saga.Gmd.WebApiServices.DAL.Customer;
using Saga.Gmd.WebApiServices.DAL.Membership;

namespace Saga.Gmd.WebApiServices.Api.Components.StrategyParts
{
    public class KeyValueMembershipFlagStrategyReaderPart : IKeyValueStrategyReaderPart
    {

        private readonly ILog _logger;
        private readonly bool _logParameterValues;

        private readonly IPermissionDataAccess _permissionsDataAccess;
        private readonly IMciRequestDataAccess _mciRequestDataAccess;
        private readonly IMembershipDataAccess _membershipDataAccess;


        private NameAndAddressParameter _nameAndAddressParameter;
        private KeyValueParameter _keyValueParameter;

        /// <summary>
        ///  Here we inject all the requirements to read Membership flags using KeyValue params
        /// </summary>
        public KeyValueMembershipFlagStrategyReaderPart(
            IPermissionDataAccess permissionsDataAccess,
            IMciRequestDataAccess mciRequestDataAccess,
            IMembershipDataAccess membershipDataAccess,
            ILog logger,
            bool logParameterValues )
        {
            _permissionsDataAccess = permissionsDataAccess;
            _mciRequestDataAccess = mciRequestDataAccess;
            _membershipDataAccess = membershipDataAccess;
            _logger = logger;
            _logParameterValues = logParameterValues;

        }

        private NameAndAddressParameter BuildWorkingNameAndAddressParameter(KeyValueParameter keyValueParameter  )
        {
            NameAndAddressParameter tempNad = new NameAndAddressParameter();

            tempNad.AccessControl = keyValueParameter.AccessControl;
            tempNad.ResponseRequestedItems = keyValueParameter.ResponseRequestedItems;
            tempNad.ReturnMe = keyValueParameter.ReturnMe;
            tempNad.NameAndAddress = new WebApiServices.Models.NameAndAddress();

            return tempNad;
        }


        /// <summary>
        /// Generate a custom MembershipPermissionFlags resposnse
        /// NB : Logic refactored from ProcessPermission.ProcessPermissionFromKeyValue() & MembershipProcess.ProcessMembershipWithKeyValue()
        /// </summary>
        /// <returns></returns>
        public object Process(KeyValueParameter keyValueParameter  )
        {
            _keyValueParameter = keyValueParameter;
            _nameAndAddressParameter = BuildWorkingNameAndAddressParameter(keyValueParameter);
            // Dummy NameAndAddress instance - Service and DataAccess classes expect one ...


            MembershipFlags membershipFlags = null;
            try
            {
                // If we have A CPC Key , use the CPC method to get the customer_id from the PermissionsId key value 
                int pKey = string.Compare(keyValueParameter.KeyValue.Key, "CPCK", StringComparison.CurrentCultureIgnoreCase) == 0 
                    ? GetCustomerIdFromPermissionId(Convert.ToInt64(keyValueParameter.KeyValue.Value)) 
                    : GetCustomerIdFromKeyValue();
                // Else use MCI customer Index lookup

                if (pKey > 0)
                {
                    try
                    {
                        // Cell services to retrieve data 
                        PermissionFull permissionsFull = GetPermissionsFull(pKey);
                        MembershipDetails membershipDetails = GetMembershipDetails(pKey);

                        // Build our cuistomised response class...
                        membershipFlags = BuildMembershipPermissionFlags(
                            permissionsFull,
                            membershipDetails
                        );

                    }
                    catch (Exception ex)
                    {
                        _logger.Error("KeyValueMembershipFlagsReadStrategyPart.Process() :" + ex.Message, ex);
                        if (_logParameterValues)
                        {
                            _logger.Error($"KeyValueMembershipFlagsReadStrategyPart.Process() :- Key={keyValueParameter.KeyValue.Key}, Value={keyValueParameter.KeyValue.Value}");
                        }
                        throw new Exception(ex.Message);
                    }

                }

            }
            catch (Exception ex)
            {
                _logger.Error("ProcessPermission Key and Value Execute :" + ex.Message, ex);
                if (_logParameterValues)
                {
                    _logger.Error($"Parameters ProcessPermissionFromKeyValue:- Key={keyValueParameter.KeyValue.Key}, Value={keyValueParameter.KeyValue.Value}");
                }

                throw new Exception(ex.Message);
            }
            return membershipFlags;

        }


        private MembershipDetails GetMembershipDetails(int pKey)
        {
            MembershipDetails details = _membershipDataAccess.GetMembershipDetails(pKey);
            return details;

        }

        private PermissionFull GetPermissionsFull(int pKey)
        {
            // Mock up a permissions parameter - the KVP request may not contain one
            ReturnMePermissions returnMePermissions = new ReturnMePermissions
            {
                Journey = "MySaga",
                ResponseParameter = "Full",
                MatchType = "",
                PermissionParameter = "All",
                Required = true
            };

            PermissionFull permissionsFull =
                _permissionsDataAccess.GetCustomerPermission(
                    returnMePermissions,
                    _nameAndAddressParameter.NameAndAddress,
                    pKey
                );
            return permissionsFull;
        }

        /// <summary>
        /// Map Permissions & Membership properties onto composite resoonse object 
        /// </summary>
        /// <param name="permissionsFull"></param>
        /// <param name="membershipDetail"></param>
        /// <returns></returns>
        private MembershipFlags BuildMembershipPermissionFlags(
            PermissionFull permissionsFull, 
            MembershipDetails membershipDetail
        )
        {
            // Find Membership category in the permissions response object 
            var membershipPermissions = permissionsFull?.PermissionCategory.FirstOrDefault( pc => 
                pc.PermissionCategoryDisplayValue == 
                EnumHelpers.GetEnumMemberStringAttributeValue<EnumStringValueAttribute>( PermissionCategoryDisplayValue.Membership )
            );

            MembershipFlags menmbershipFlags = new MembershipFlags{
                 MembershipPermissionFlags = new MembershipPermissionFlags
                 { 
                 EligibleForMembership = membershipDetail?.IsEligible ?? false,
                 MembershipStatus = membershipDetail?.MembershipStatus
                 }
            };
            if (membershipPermissions != null)
            {
                menmbershipFlags.MembershipPermissionFlags.MembershipConsentStatus = membershipPermissions.PermissionCategoryStatus;
                menmbershipFlags.MembershipPermissionFlags.MembershipPostConsentStatus = membershipPermissions.ChannelPostFlag;
                menmbershipFlags.MembershipPermissionFlags.MembershipEmailConsentStatus = membershipPermissions.ChannelEmailFlag;
                menmbershipFlags.MembershipPermissionFlags.MembershipPhoneConsentStatus = membershipPermissions.ChannelPhoneNoFlag;
                menmbershipFlags.MembershipPermissionFlags.MembershipSmsConsentStatus = membershipPermissions.ChannelSmsFlag;
            }

            return menmbershipFlags;

        }

        /// <summary>
        /// Call Index key search method on _mciRequestService to retrieve PKey
        /// </summary>
        /// <returns></returns>
        private int GetCustomerIdFromKeyValue()
        {
            var customerPKey = _mciRequestDataAccess.GetCustomerAllIndexKeys(
                _keyValueParameter.KeyValue.Key, _keyValueParameter.KeyValue.Value, SourceKey.PKEY.ToString() );

            var customerId = customerPKey.Any() ? customerPKey.First().CustomerId : 0;
            return customerId;
        }

        private int GetCustomerIdFromPermissionId(long permissionsId)
        {
            return _permissionsDataAccess.GetCustomerIdFromPermission(permissionsId);
        }


    }
}