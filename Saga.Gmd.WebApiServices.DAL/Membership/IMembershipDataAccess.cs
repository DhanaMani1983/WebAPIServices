using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Saga.Gmd.WebApiServices.Models;
using Saga.Gmd.WebApiServices.Models.Customer;
using Saga.Gmd.WebApiServices.Models.Membership;

namespace Saga.Gmd.WebApiServices.DAL.Membership
{
    public interface IMembershipDataAccess
    {
        MembershipDetails GetMembershipDetails(int custRefKey);
        List<string> GetMembershipStatus();
        List<string> GetMembershipDeclineReason();
        List<string> GetOverrideReason();
        List<string> GetFulfilmentOverride();
        List<string> GetActivationDeclineReason();
        List<string> GetMembershipCancelReason();

        List<MembershipOptions> GetMembershipOptions(string option);

        string SetEligible(MembershipDataInput input);
        string UpdateMembership(MembershipDataInput input);
        string DeclineMembership(MembershipDataInput input);
        string CancelMembership(MembershipDataInput input);
        List<string> GetActivationSource();
        List<string> GetMembershipStatusReason();
        string CreateTempMembership(MembershipDataInput input);
        IDictionary<string, string> CreateMembership(MembershipParam input);
        List<MatchedCustomer> GetMatchedTemporaryMember(NameAndAddress p);
        MembershipDetails GetMembershipDetails(string activationId);

        TR GetMembershipDetails<T1, TR>(T1 id) where TR : MembershipDetails;

    }
}
