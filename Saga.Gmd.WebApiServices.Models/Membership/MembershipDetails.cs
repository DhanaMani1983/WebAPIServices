using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Saga.Gmd.WebApiServices.Common;

namespace Saga.Gmd.WebApiServices.Models.Membership
{
    public class MembershipDetails
    {



        [JsonIgnore]
        public string CustomerId { get; set; }

        [JsonProperty(PropertyName = "ActivationId")]
        public string ActivationId { get; set; }
        [JsonProperty(PropertyName = "EncryptedActivationId")]
        public string EncryptedActivationId { get; set; }
        [JsonProperty(PropertyName = "IsEligible")]
        public bool? IsEligible { get; set; }
        [JsonProperty(PropertyName = "MembershipNo")]
        public long? MembershipNo { get; set; }
        [JsonProperty(PropertyName = "EncryptedMembershipNo")]
        public string EncryptedMembershipNo { get; set; }

        [JsonProperty(PropertyName = "ActivationStatus")]
        public string ActivationStatus { get; set; }
        [JsonProperty(PropertyName = "ActivationSource")]
        public string ActivationSource { get; set; }
        [JsonProperty(PropertyName = "ActivationDate")]
        public DateTime? ActivationDate { get; set; }

        [JsonProperty(PropertyName = "ActivationDeclineReason")]
        public string ActivationDeclineReason { get; set; }

        [JsonProperty(PropertyName = "CurrentStatus")]
        public string MembershipStatus { get; set; }

        [JsonProperty(PropertyName = "CurrentStatusReason")]
        public string MembershipStatusReason { get; set; }

        [JsonProperty(PropertyName = "IsOverride")]
        public bool? IsOverride { get; set; }

        [JsonProperty(PropertyName = "OverrideReason")]
        public string OverrideReason { get; set; }

        [JsonProperty(PropertyName = "WelcomePackChoice")]
        public string WelcomePackChoice => 
            MembershipStatus == "Activated" ? WelcomePackChoiceDbValue : "";
        // 

        [JsonIgnore]
        public string WelcomePackChoiceDbValue  { get; set; }

        [JsonProperty(PropertyName = "MembershipStartDate")]
        public DateTime? MembershipStartDate { get; set; }

        [JsonProperty(PropertyName = "MembershipExpiryDate")]
        public DateTime? MembershipExpiryDate { get; set; }

        [JsonProperty(PropertyName = "Products")]
        public string Products { get; set; }

        [JsonProperty(PropertyName = "ProductLinkedToExpiryDate")]
        public string ProductLinkedtoExpiryDate { get; set; }

        [JsonProperty(PropertyName = "MembershipLevel")]
        public string MembershipLevel { get; set; }

        [JsonProperty(PropertyName = "MembershipLevelEver")]
        public string MembershipLevelEver { get; set; }

        [JsonProperty(PropertyName = "IsHighAffinityCustomer")]
        public bool? IsHac { get; set; }

        [JsonProperty(PropertyName = "AgentName")]
        public string AgentName { get; set; }

        [JsonProperty(PropertyName = "LastContactSource")]
        public string LastContactSource { get; set; }

        [JsonProperty(PropertyName = "IsActive")]
        public bool? IsActive { get; set; }

        [JsonProperty(PropertyName = "IsNewBusiness")]
        public bool? IsNewBusiness { get; set; }

        [JsonProperty(PropertyName = "EngagementStatus")]
        public bool? IsEnagementStatus { get; set; }

        [JsonProperty(PropertyName = "CanPrompt")]
        public bool? CanPrompt { get; set; }

        [JsonProperty(PropertyName = "IsAssociateMember")]
        public bool? IsAssociateMember { get; set; }

        [JsonProperty(PropertyName = "EligibleCustomerNewName")]
        public bool? IsEligibleCustomerNewName { get; set; }

        [JsonProperty(PropertyName = "LastStatusChangeDate")]
        public DateTime? LastStatusChangeDate { get; set; }

        [JsonIgnore]
        public string Info { get; set; }

    }
}
