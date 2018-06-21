using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using Saga.Gmd.WebApiServices.DAL.Membership;
using Saga.Gmd.WebApiServices.Models.Membership;
using Saga.Gmd.WebApiServices.Common;
using Saga.Gmd.WebApiServices.Models;
using Saga.Gmd.WebApiServices.Models.Customer;

namespace Saga.Gmd.WebApiServices.BLL.Membership
{
    public class MembershipService : IMembershipService
    {
        private readonly IMembershipDataAccess _dataAccess;
        private ILog _logger;
        public MembershipService(IMembershipDataAccess dataAccess, ILog logger)
        {
            _dataAccess = dataAccess;
            _logger = logger;
        }
        public MembershipDetails GetMembershipDetails(int custRefId)
        { 
            var membershipData = _dataAccess.GetMembershipDetails(custRefId);

            return membershipData;

        }

        public List<string> GetMembershipStatus()
        {

            //get the MembershipStatus data
            var mStatusList = _dataAccess.GetMembershipStatus();
            return mStatusList;

        }

        public List<string> GetMembershipDeclineReason()
        {
            //get membershipStatusReason
            var mStatusReason = _dataAccess.GetMembershipDeclineReason();
            return mStatusReason;

        }

        public List<string> GetActivationDeclineReason()
        {
            var mStatusReason = _dataAccess.GetActivationDeclineReason();
            return mStatusReason;
        }

      

        public List<string> GetOverrideReason()
        {

            //get membershipStatusReason
            var mOverrideReason = _dataAccess.GetOverrideReason();
            return mOverrideReason;


        }
        public List<string> GetActivationSource()
        {
            //get membershipStatusReason
            var mOverrideReason = _dataAccess.GetActivationSource();
            return mOverrideReason;
        }

        public List<string> GetMembershipStatusReason()
        {
            var mOverrideReason = _dataAccess.GetMembershipStatusReason();
            return mOverrideReason;
        }

        string IMembershipService.CreateTempMembership(MembershipDataInput input)
        {
           return _dataAccess.CreateTempMembership(input);
        }

        public IDictionary<string,string> CreateMembership(MembershipParam input)
        {
             return _dataAccess.CreateMembership(input);
        }

        public List<MatchedCustomer> GetMatchedTemporaryMember(NameAndAddress p)
        {
            return _dataAccess.GetMatchedTemporaryMember(p);
        }

        public MembershipDetails GetMembershipDetails(string activationId)
        {
            return _dataAccess.GetMembershipDetails(activationId);
        }

        public List<string> GetFulfilmentOverride()
        {
            //get membershipStatusReason
            var mOverrideReason = _dataAccess.GetFulfilmentOverride();
            return mOverrideReason;
        }

        

        public string UpdateMembership(MembershipDataInput input)
        {
            return _dataAccess.UpdateMembership(input);
        }

        public string DeclineMembership(MembershipDataInput input)
        {
            return _dataAccess.DeclineMembership(input);
        }
    }
}

