using System.Collections.Generic;
using Newtonsoft.Json;

namespace Saga.Gmd.WebApiServices.Models.Membership
{
    public class MembershipOptionsData
    {
        public MembershipOptionsData()
        {
            MembershipStatusData = new List<string>();
          //  MembershipDeclineReason = new List<string>();
            //MembershipCanxReason = new List<string>();
            OverrideReason = new List<string>();
            ActivationDeclineReason = new List<string>();
            MembershipStatusReason = new List<string>();
            ActivationSource = new List<string>();

        }

        [JsonProperty("MembershipStatus")]
        public List<string> MembershipStatusData { get; set; }

        //[JsonProperty("MembershipDeclineReason")]
        //public List<string> MembershipDeclineReason { get; set; }

        //[JsonProperty("MembershipCanxReason")]
        //public List<string> MembershipCanxReason { get; set; }

        [JsonProperty("OverrideReason")]
        public List<string> OverrideReason { get; set; }

        [JsonProperty("FulfilmentOverride")]
        public List<string> FulfilmentOverride { get; set; }

        public List<string> ActivationDeclineReason { get; set; }

        public List<string> MembershipStatusReason { get; set; }

        public List<string> ActivationSource { get; set; }
    }

    public class MembershipOptionsDataV2 : MembershipOptionsData
    {
        public MembershipOptionsDataV2()
        {
            MembershipCancellationReason = new List<string>();
        }

        [JsonProperty("MembershipCancellationReason")]
        public List<string> MembershipCancellationReason { get; set; }

    }

}
