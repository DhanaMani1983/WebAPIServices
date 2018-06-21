using Saga.Gmd.WebApiServices.Models.Customer;

namespace Saga.Gmd.WebApiServices.Models.ReturnMe
{
    public class ReturnMeForKeyValuePair : ReturnMe
    {

        public ReturnMeForKeyValuePair()
        {
            CustomerDetails = new CustomerDetails();
            Permissions = new ReturnMePermissions();
            MembershipFlags = new ReturnMeMembershipFlags();
        }

        public CustomerDetails CustomerDetails { get; set; }

        /// <summary>
        /// requests Membership permissions response required by Discus / Thunderhead
        /// </summary>
        public ReturnMeMembershipFlags MembershipFlags { get; set; }
    }
} 