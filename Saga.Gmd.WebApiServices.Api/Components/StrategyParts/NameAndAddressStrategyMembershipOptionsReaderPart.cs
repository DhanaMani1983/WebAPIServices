using log4net;
using Saga.Gmd.WebApiServices.Api.Models.JsonModels;
using Saga.Gmd.WebApiServices.Api.Models.ParameterModels;
using Saga.Gmd.WebApiServices.DAL.Membership;
using System;
using Saga.Gmd.WebApiServices.Common;

namespace Saga.Gmd.WebApiServices.Api.Components.StrategyParts
{
    /// <summary>
    /// Reader part for membership options data - addresss DAL classes directly instead of the BLL service layer 
    /// </summary>
    public class NameAndAddressStrategyMembershipOptionsReaderPart : INameAndAddressStrategyReaderPart
    {

        private readonly IMembershipDataAccess _membershipDataAccess;
        private readonly ILog _logger;
        private readonly bool _logParameterValues;

        public NameAndAddressStrategyMembershipOptionsReaderPart(
            IMembershipDataAccess membershipDataAccess,
            ILog logger,
            bool logParameterValues
        )
        {
            _logParameterValues = logParameterValues;
            _logger = logger;
            _membershipDataAccess = membershipDataAccess;

        }

        /// <summary>
        /// Refactored from MembershipProcess.GetMembershipOptionsData()
        /// </summary>
        /// <param name="nameAndAddressParameter"></param>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public object Process(NameAndAddressParameter nameAndAddressParameter, int? customerId)
        {
            var mOptionsOutput = new MembershipOptionsDataOutput();
            try
            {
                mOptionsOutput.MembershipOptionsData.MembershipStatusData = _membershipDataAccess.GetMembershipStatus();
                mOptionsOutput.MembershipOptionsData.ActivationDeclineReason = _membershipDataAccess.GetActivationDeclineReason();
                mOptionsOutput.MembershipOptionsData.OverrideReason = _membershipDataAccess.GetOverrideReason();
                mOptionsOutput.MembershipOptionsData.FulfilmentOverride = _membershipDataAccess.GetFulfilmentOverride();
                mOptionsOutput.MembershipOptionsData.ActivationSource = _membershipDataAccess.GetActivationSource();
                mOptionsOutput.MembershipOptionsData.MembershipStatusReason = _membershipDataAccess.GetMembershipStatusReason();
                return mOptionsOutput;
            }
            catch (Exception ex)
            {
                _logger.Error("GetMembershipOptionsData : ErrorTag: " + ErrorTagProvider.ErrorTagDatabase + " -- " + ex.Message, ex);
                throw new Exception(ex.Message);
            }
        }


    }
}