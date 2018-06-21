using Saga.Gmd.WebApiServices.BLL.Membership;
using Saga.Gmd.WebApiServices.Models.Membership;

namespace Saga.Gmd.WebApiServices.Api.Models.ModelBinder
{
    public class MembershipHelper
    {
        private readonly IMembershipService _membershipService;
        public MembershipHelper(IMembershipService membershipService)
        {
            _membershipService = membershipService;
        }

        public MembershipDetails GetMembershipDetails(string key)
        {
            return _membershipService.GetMembershipDetails(key);
        }
    }
}