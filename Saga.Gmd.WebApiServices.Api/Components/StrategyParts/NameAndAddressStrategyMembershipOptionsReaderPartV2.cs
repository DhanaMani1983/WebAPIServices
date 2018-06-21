using System;
using log4net;
using Saga.Gmd.WebApiServices.Api.Models.JsonModels;
using Saga.Gmd.WebApiServices.Api.Models.ParameterModels;
using Saga.Gmd.WebApiServices.Common;
using Saga.Gmd.WebApiServices.DAL.Membership;

namespace Saga.Gmd.WebApiServices.Api.Components.StrategyParts
{
    public class NameAndAddressStrategyMembershipOptionsReaderPartV2 : INameAndAddressStrategyReaderPart
    {

        private readonly IMembershipDataAccess _membershipDataAccess;
        private readonly ILog _logger;
        private readonly bool _logParameterValues;

        public NameAndAddressStrategyMembershipOptionsReaderPartV2(
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
            var mOptionsOutput = new MembershipOptionsDataOutputV2();
            try
            {
                mOptionsOutput.MembershipOptionsData.MembershipStatusData = _membershipDataAccess.GetMembershipStatus();
                mOptionsOutput.MembershipOptionsData.ActivationDeclineReason = _membershipDataAccess.GetActivationDeclineReason();
                mOptionsOutput.MembershipOptionsData.OverrideReason = _membershipDataAccess.GetOverrideReason();
                mOptionsOutput.MembershipOptionsData.FulfilmentOverride = _membershipDataAccess.GetFulfilmentOverride();
                mOptionsOutput.MembershipOptionsData.ActivationSource = _membershipDataAccess.GetActivationSource();
                mOptionsOutput.MembershipOptionsData.MembershipStatusReason = _membershipDataAccess.GetMembershipStatusReason();

                // GITCS-9 : Cancellation reasons only on V2
                mOptionsOutput.MembershipOptionsData.MembershipCancellationReason =
                    _membershipDataAccess.GetMembershipCancelReason();


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