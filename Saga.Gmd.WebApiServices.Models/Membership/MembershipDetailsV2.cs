using Newtonsoft.Json;

namespace Saga.Gmd.WebApiServices.Models.Membership
{
    public class MembershipDetailsV2 : MembershipDetails
    {
        // This is an additional V2 property ...
        [JsonProperty(PropertyName = "ActivationDeclineMarketingReason")]
        public string ActivationDeclineMarketingReason { get; set; }

    }
}
