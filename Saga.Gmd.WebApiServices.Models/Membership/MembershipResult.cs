using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Saga.Gmd.WebApiServices.Models.Membership
{
    public class MembershipResult
    {   
        [JsonProperty(PropertyName = "ActivationId")]
        public int ActivationId { get; set; }

        [JsonProperty(PropertyName = "EncryptedActivationId")]
        public string EncryptedActivationId { get; set; }

        [JsonProperty(PropertyName = "IsEligible")]
        public bool IsEligible { get; set; }

        [JsonProperty(PropertyName = "MembershipNo")]
        public int MembershipNo { get; set; }

        [JsonProperty(PropertyName = "EncryptedMembershipNo")]
        public string EncryptedMembershipNo { get; set; }

        [JsonProperty(PropertyName = "CurrentStatus")]
        public string CurrentStatus { get; set; }

        [JsonProperty(PropertyName = "CurrentStatusReason")]
        public string CurrentStatusReason { get; set; }

        [JsonProperty(PropertyName = "MembershipLevel")]
        public string MembershipLevel { get; set; }

        [JsonProperty(PropertyName = "MembershipLevelEver")]
        public string MembershipLevelEver { get; set; }

        [JsonProperty(PropertyName = "HighAffinityCustomer")]
        public string HighAffinityCustomer { get; set; }

        [JsonProperty(PropertyName = "IsInactive")]
        public bool IsInactive { get; set; }

        [JsonProperty(PropertyName = "IsNewBusiness")]
        public bool IsNewBusiness { get; set; }

        [JsonProperty(PropertyName = "EngagementStatus")]
        public bool EngagementStatus { get; set; }

        [JsonProperty(PropertyName = "DoNotPrompt")]
        public bool DoNotPrompt { get; set; }

        [JsonProperty(PropertyName = "MembershipStartDate")]
        public DateTime? MembershipStartDate { get; set; }

        [JsonProperty(PropertyName = "MembershipExpiryDate")]
        public DateTime? MembershipExpiryDate { get; set; }

        [JsonProperty(PropertyName = "ProductLinkedToExpiryDate")]
        public string ProductLinkedToExpiryDate { get; set; }

        //[JsonProperty(PropertyName = "LastMembershipStatusChange")]
        //public string LastMembershipStatusChange { get; set; }

        //[JsonProperty(PropertyName = "HighValueLevel")]
        //public string HighValueLevel { get; set; }

        [JsonProperty(PropertyName = "ActivationDate")]
        public DateTime? ActivationDate { get; set; }

        [JsonProperty(PropertyName = "AgentName")]
        public string AgentName { get; set; }

        [JsonProperty(PropertyName = "LastContactSource")]
        public string LastContactSource { get; set; }

        [JsonProperty(PropertyName = "OverrideFlag")]
        public bool? OverrideFlag { get; set; }

        [JsonProperty(PropertyName = "OverrideReason")]
        public string OverrideReason { get; set; }

        [JsonProperty(PropertyName = "IsAssociateMember")]
        public bool? IsAssociateMember { get; set; }

        [JsonProperty(PropertyName = "LastStatusChangeDate")]
        public DateTime? LastStatusChangeDate { get; set; }

        [JsonProperty(PropertyName = "CurrentDateTime")]
        public DateTime? CurrentDateTime { get; set; }

        [JsonProperty(PropertyName = "Products")]
        public List<string> Products { get; set; } 

        [JsonProperty(PropertyName = "EligibleCustomerNewName")]
        public string EligibleCustomerNewName { get; set; }
    }
}
