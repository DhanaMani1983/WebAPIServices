using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using log4net;
using Saga.Gmd.WebApiServices.Api.Models.JsonModels;
using Saga.Gmd.WebApiServices.Api.Models.ParameterModels;
using Saga.Gmd.WebApiServices.BLL.Customer;
using Saga.Gmd.WebApiServices.BLL.Membership;
using Saga.Gmd.WebApiServices.Common;
using Saga.Gmd.WebApiServices.Models;
using Saga.Gmd.WebApiServices.Models.Membership;
using WebGrease.Preprocessing;

namespace Saga.Gmd.WebApiServices.Api.Components.Membership
{
    public class MembershipProcess
    {
        private readonly KeyValueParameter _keyValue;
        private readonly NameAndAddressParameter _nameAndAddress;
        private readonly IMciRequestService _mciRequestService;
        private readonly IMembershipService _membershipService;
        private readonly ILog _logger;
        private readonly bool _logParameterValue;
        public bool CustomerFound { get; private set; }
        

        public MembershipProcess(KeyValueParameter keyValue, IMembershipService membershipService,
            IMciRequestService mciRequestService, ILog logger, NameAndAddressParameter nameAndAddress = null)
        {
            _keyValue = keyValue;
            _nameAndAddress = nameAndAddress;
            _mciRequestService = mciRequestService;
            _membershipService = membershipService;
            _logger = logger;
            _logParameterValue = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableObjectDump"]);
        }

        public object ProcessMembershipWithKeyValue()
        {
            CustomerFound = true;
            if (_keyValue.AccessControl.ModuleAccess.Find(x => x.HasAccess && x.Key == GroupCode.MEMB) == null)
            {
                NoAccessToMembership message = new NoAccessToMembership();
                message.Message = "You dont' have access for Membership data.";
                return message;
            }

            var cusKeys = new CustomerKeys();
            var membershipField = new MembershipFields();
            var membershipOutput = new MembershipOutput();

            try
            {
                var pKey = GetCustomerPersistantKeyFromKeyValue();
                if (pKey == 0)
                {
                    cusKeys.Keys.HasPartialResult = false;
                    cusKeys.Keys.Message = "No persistent key found for this customer";
                    CustomerFound = false;
                    return null;
                }

                try
                {
                    membershipOutput.MembershipData = _membershipService.GetMembershipDetails(pKey);
                }
                catch (Exception ex)
                {
                    _logger.Error("ProcessMembership From KeyValue: ErrorTag: " + ErrorTagProvider.ErrorTagDatabase + " -- " + ex.Message, ex);
                    throw new Exception(ex.Message);
                }
            }

            catch (Exception ex)
            {
                _logger.Error("ProcessMembership From KeyValue: ErrorTag: " + ErrorTagProvider.ErrorTagDatabase + " -- " + ex.Message, ex);

                if (_logParameterValue)
                {
                    _logger.Error($"Parameters MembershipForMembershipKeyValue:- Key={_keyValue.KeyValue.Key}, Value={_keyValue.KeyValue.Value}");
                }

                throw new Exception(ex.Message);
            }

            return membershipOutput;

        }


        private int GetCustomerPersistantKeyFromKeyValue()
        {
            var customerPKey = _mciRequestService.GetCustomerAllIndexKeys(_keyValue.KeyValue.Key,
                    _keyValue.KeyValue.Value, SourceKey.AFEK.ToString());

            var customerId = customerPKey.Any() ? customerPKey.First().CustomerId : 0;
            return customerId;
        }


        public MembershipDetails UpdateMembershipData(MembershipDataInput member)
        {
            string message = string.Empty;
            MembershipDetails updatedMembershipdetails = new MembershipDetails();
            try
            {
                var activation = string.IsNullOrEmpty(member.ActivationId)
                    ? member.EncryptedActivationId
                    : member.ActivationId;
                MembershipDetails membershipDetails = _membershipService.GetMembershipDetails(activation);

                if (membershipDetails.IsEligible != null && membershipDetails.IsEligible.Value)
                {

                    switch (member.OriginalStatus)
                    {

                        case Common.MembershipStatus.NotActivated:
                            switch (member.UpdatedStatus)
                            {
                                case Common.ActionMembershipStatus.Activate:
                                    _logger.Info("-----1-------");
                                    message = _membershipService.UpdateMembership(member);
                                    break;
                                case Common.ActionMembershipStatus.Decline:
                                    _logger.Info("-----2-------");
                                    message = _membershipService.DeclineMembership(member);
                                    break;

                            }
                            break;

                        case Common.MembershipStatus.Declined:
                            switch (member.UpdatedStatus)
                            {
                                case Common.ActionMembershipStatus.Activate:
                                    _logger.Info("-----3-------");
                                    message = _membershipService.UpdateMembership(member);
                                    break;
                            }
                            break;
                        case Common.MembershipStatus.Cancelled:
                            switch (member.UpdatedStatus)
                            {
                                case Common.ActionMembershipStatus.Activate:
                                    _logger.Info("----4-------");
                                    message = _membershipService.UpdateMembership(member);
                                    break;
                            }
                            break;
                        case Common.MembershipStatus.Lapsed:
                            switch (member.UpdatedStatus)
                            {
                                case Common.ActionMembershipStatus.Activate:
                                    _logger.Info("----5-------");
                                    message = _membershipService.UpdateMembership(member);
                                    break;
                            }
                            break;
                    }
                }
                else if (member.OverrideFlag != null && (membershipDetails.IsEligible != null &&
                                                         (membershipDetails.IsEligible.Value == false &&
                                                          member.OriginalStatus == Common.MembershipStatus.NotActivated &&
                                                          member.UpdatedStatus == Common.ActionMembershipStatus.Activate) &&
                                                         member.OverrideFlag.Value))
                {

                    _logger.Info("-----6-------");
                    message = _membershipService.UpdateMembership(member);

                }
                else if (membershipDetails.IsEligible != null &&
                         (membershipDetails.IsEligible.Value == false &&
                          member.OriginalStatus == Common.MembershipStatus.Declined &&
                          member.UpdatedStatus == Common.ActionMembershipStatus.Activate))
                {
                    _logger.Info("-----7-------");
                    message = _membershipService.UpdateMembership(member);
                }
                else if (membershipDetails.IsEligible != null &&
                         (membershipDetails.IsEligible.Value == false &&
                          member.OriginalStatus == Common.MembershipStatus.NotActivated &&
                          member.UpdatedStatus == Common.ActionMembershipStatus.Decline))
                {

                    member.OverrideReason = member.DeclineReason;
                    member.OverrideFlag = true;
                    _logger.Info("-----8-------");
                    message = _membershipService.DeclineMembership(member);
                }

                else if (membershipDetails.IsEligible != null &&
                         (membershipDetails.IsEligible.Value == false &&
                          member.OriginalStatus == Common.MembershipStatus.Cancelled &&
                          member.UpdatedStatus == Common.ActionMembershipStatus.Activate))
                {

                    {
                        _logger.Info("-----9-------");
                        message = _membershipService.UpdateMembership(member);


                    }
                }
                else if (membershipDetails.IsEligible != null &&
                         (membershipDetails.IsEligible.Value == false &&
                          member.OriginalStatus == Common.MembershipStatus.Lapsed &&
                          member.UpdatedStatus == Common.ActionMembershipStatus.Activate))
                {

                    {
                        _logger.Info("-----10-------");
                        message = _membershipService.UpdateMembership(member);


                    }
                }

                updatedMembershipdetails = _membershipService.GetMembershipDetails(membershipDetails.CustomerId);
                updatedMembershipdetails.Info = message;
                return updatedMembershipdetails;
            }
            catch (DatabaseException ex)
            {
                throw new DatabaseException(ex.Message);
            }
            catch (ParameterProcessException ex)
            {
                throw new PreprocessingException(ex.Message);
            }
            catch (Exception ex)
            {
                // _logger.Error("UpdateMembershipData : ErrorTag: " + ErrorTagProvider.ErrorTag + " -- " + ex.Message, ex);
                throw new Exception(ex.Message);
            }
        }
    }

}