using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Saga.Gmd.WebApiServices.BLL.Customer;
using Saga.Gmd.WebApiServices.BLL.Membership;
using Saga.Gmd.WebApiServices.Models.Customer;
using Saga.Gmd.WebApiServices.Models.Membership;

namespace Saga.Gmd.WebApiServices.Api.Components
{
    public class ProcessHelpers
    {
        private WebApiServices.Models.NameAndAddress _nameAndAddress;
        private readonly IMciRequestService _mciRequestService;
        private IMembershipService _membershipService;
        public ProcessHelpers(WebApiServices.Models.NameAndAddress nameAndAddress, IMciRequestService mciRequestService, IMembershipService membershipService)
        {
            _nameAndAddress = nameAndAddress;
            _mciRequestService = mciRequestService;
            _membershipService = membershipService;
        }

        public int? GetCustomerPersistentKeyFromNameAddress()
        {
            return _mciRequestService.GetPersistantKey(_nameAndAddress);
        }

        public int? GetMatchedTemporaryMember()
        {
            List<MatchedCustomer> output =  _membershipService.GetMatchedTemporaryMember(_nameAndAddress); 
            return output.FirstOrDefault()?.CustomerId; 
        }

        public MembershipDetails GetMembershipDetails(string activationId)
        {
            return _membershipService.GetMembershipDetails(activationId);
        }
    }
}