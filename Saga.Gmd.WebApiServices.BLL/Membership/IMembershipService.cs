using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Saga.Gmd.WebApiServices.Models;
using Saga.Gmd.WebApiServices.Models.Customer;
using Saga.Gmd.WebApiServices.Models.Membership;

namespace Saga.Gmd.WebApiServices.BLL.Membership
{
    public interface IMembershipService
    {
        MembershipDetails GetMembershipDetails(int id);

        List<string> GetMembershipStatus();

        List<string> GetMembershipDeclineReason();
        List<string> GetOverrideReason();

        List<string> GetFulfilmentOverride(); 
        string UpdateMembership(MembershipDataInput input);
        string DeclineMembership(MembershipDataInput input);
        List<string> GetActivationDeclineReason();
        List<string> GetActivationSource();

        List<string> GetMembershipStatusReason();
        string CreateTempMembership(MembershipDataInput input);
        IDictionary<string,string> CreateMembership(MembershipParam input);
        List<MatchedCustomer> GetMatchedTemporaryMember(NameAndAddress p);
        MembershipDetails GetMembershipDetails(string activationId);
    }
}
