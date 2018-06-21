using System;
using System.Collections.Generic;
using System.Net;
using log4net;
using Saga.Gmd.WebApiServices.Api.Models.JsonModels;
using Saga.Gmd.WebApiServices.Api.Models.ParameterModels;
using Saga.Gmd.WebApiServices.Common;
using Saga.Gmd.WebApiServices.DAL.Customer;
using Saga.Gmd.WebApiServices.DAL.Membership;
using Saga.Gmd.WebApiServices.Models;

namespace Saga.Gmd.WebApiServices.Api.Components.StrategyParts
{
    /// <summary>
    /// Reader Part for Membership data - addresss DAL classes directly instead of the BLL service layer 
    /// </summary>
    public class NameAndAddressStrategyMembershipDetailsReaderPart : INameAndAddressStrategyReaderPart
    {

        private readonly IMciRequestDataAccess _mciRequestDataAccess;
        private readonly IMembershipDataAccess _membershipDataAccess;
        private readonly ILog _logger;
        private readonly bool _logParameterValues;

        public NameAndAddressStrategyMembershipDetailsReaderPart(
            IMciRequestDataAccess mciRequestDataAccess,
            IMembershipDataAccess membershipDataAccess,
            ILog logger,
            bool logParameterValues
            )
        {
            _logParameterValues = logParameterValues;
            _logger = logger;
            _mciRequestDataAccess = mciRequestDataAccess;
            _membershipDataAccess = membershipDataAccess;

        }


        /// <summary>
        /// Fetch Membership  details for customer 
        /// Refactored logic from MembershipProcess.GetMembershipFromNameAndAddress()
        /// </summary>
        /// <param name="nameAndAddressParameter">NameAndAddress to identify customer</param>
        /// <param name="customerId">Customer Id used in preference to the above if passed</param>
        /// <returns></returns>
        public object Process(NameAndAddressParameter nameAndAddressParameter, int? customerId)
        {
            if (nameAndAddressParameter.AccessControl.ModuleAccess.Find(x => x.HasAccess && x.Key == GroupCode.MEMB) == null)
            {
                NoAccessToMembership message = new NoAccessToMembership();
                message.Message = "You dont' have access for Membership data.";
                return message;
            }

            var membershipOutputData = new MembershipOutput();

            try
            {
                // Use any Customer Id passed else use NameAddr lookup
                var pkey = customerId ?? _mciRequestDataAccess.GetPersistantKey(nameAndAddressParameter.NameAndAddress);

                if (pkey > 0)
                {
                    _logger.Info("Pkey Found " + pkey);
                    nameAndAddressParameter.ReturnMe.Membership.CustomerId = pkey.Value.ToString();
                    var membershipDetails = _membershipDataAccess.GetMembershipDetails(pkey.Value.ToString());

                    membershipOutputData.MembershipData = membershipDetails;

                    if (string.IsNullOrEmpty(membershipOutputData.MembershipData.ActivationId) &&
                        string.IsNullOrEmpty(membershipOutputData.MembershipData.EncryptedActivationId))
                    {
                        IDictionary<string, string> output =
                            _membershipDataAccess.CreateMembership(nameAndAddressParameter?.ReturnMe?.Membership);

                        membershipOutputData.MembershipData.ActivationId = output["ActivationId"];
                        membershipOutputData.MembershipData =
                            _membershipDataAccess.GetMembershipDetails(membershipOutputData.MembershipData.ActivationId);
                    }
                }
                else
                {
                    return HttpStatusCode.NotFound;
                }

            }
            catch (Exception ex)
            {
                _logger.Error("GetMembershipFromNameAndAddress : ErrorTag: " + ErrorTagProvider.ErrorTagDatabase + " -- " + ex.Message, ex);

                if (_logParameterValues)
                {
                    _logger.Error(
                        $"Parameters GetMembershipFromNameAndAddress:- Name=FirstName={nameAndAddressParameter.NameAndAddress.FirstName}, LastName={nameAndAddressParameter.NameAndAddress.Surname}, Dob={nameAndAddressParameter.NameAndAddress.Dob}, " +
                        $"Address=AddresLine1={nameAndAddressParameter.NameAndAddress.Address.Address1}, AddressLine2={nameAndAddressParameter.NameAndAddress.Address.Address2}, AddressLine3={nameAndAddressParameter.NameAndAddress.Address.Address3}, " +
                        $"AddressLine4={nameAndAddressParameter.NameAndAddress.Address.Address4}, PostCode={nameAndAddressParameter.NameAndAddress.Address.Postcode}");
                }

                throw new Exception(ex.Message);
            }
            return membershipOutputData;

        }
    }
}