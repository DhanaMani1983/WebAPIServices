using System.Collections.Generic;
using Saga.Gmd.WebApiServices.Models.Membership;
using Saga.Gmd.WebApiServices.Models.Permissions;

namespace Saga.Gmd.WebApiServices.Models.ReturnMe
{
    public class ReturnMe
    {
        public ReturnMe()
        {
            Permissions = new ReturnMePermissions();
            Membership = new MembershipParam();
            CustomerMatch = new CustomerMatch();
            CustomerDetail = new CustomerDetail();
            TravelSummary = new TravelSummary();
        }
        public List<string> CustomerKeys { get; set; } 
        public ReturnMePermissions Permissions { get; set; }
        public MailingHistory.MailingHistory MailingHistory { get; set; }
        public MembershipParam Membership { get; set; }
        public CustomerDetail CustomerDetail { get; set; }
        public CustomerMatch CustomerMatch { get; set; }
        public TravelSummary TravelSummary { get; set; }
    }
}