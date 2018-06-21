using System;
using Newtonsoft.Json;
using Saga.Gmd.WebApiServices.Models.Security;
using Saga.Gmd.WebApiServices.Common;

namespace Saga.Gmd.WebApiServices.Models.Membership
{
     
    public class MembershipDataInput
    {

        public MembershipDataInput()
        {
            CustNameAndAddress = new NameAndAddress(); 
        }

        /// <summary>
        /// CancelationOrActivationKey - MembershipNo (for Cancellation and Decline) OR ActivationId 
        /// </summary>
        public string CancelationOrActivationKey =>
            UpdatedStatus != MembershipActionType.Cancel.ToString()
                ? string.IsNullOrEmpty(ActivationId) ? EncryptedActivationId : ActivationId
                : MembershipNo.HasValue
                    ? MembershipNo.ToString()
                    : EncryptedMembershipNo;


        // GITCS-9 : Cancellation reason 
        public string CancellationReason { get; set; }



        /// <summary>
        /// Find the appropriate membership id value for ProductSold calls
        /// </summary>
        public string ProductSoldKey =>
                string.IsNullOrEmpty( ActivationId ) 
                    ? ( string.IsNullOrEmpty( EncryptedActivationId )
                        ? ( !MembershipNo.HasValue
                            ? EncryptedMembershipNo
                            : MembershipNo.ToString()
                          )
                        : EncryptedActivationId
                        )
                    : ActivationId;
        //public KeyValuePair<string,string> CustKeyValue { get; set; }
        public NameAndAddress CustNameAndAddress { get; set; } 

        public int CustomerId { get; set; } 

        public bool? IsEligible { get; set; }
        public string Division { get; set; }
        public string ProductCode { get; set; }
        public string ActivationId { get; set; }
        public string EncryptedActivationId { get; set; }
        public long? MembershipNo { get; set; }
        public string EncryptedMembershipNo { get; set; }
        public string UpdatedStatus { get; set; }
        public string OriginalStatus { get; set; }
        public string UpdatedStatusReason { get; set; }
        public string AgentName { get; set; }
        public string SourceSystem { get; set; } 
        public bool? OverrideFlag { get; set; }
        public string OverrideReason { get; set; }
        public string DeclineReason { get; set; }
        public string FulfilmentOverride { get; set; }
        public string ActivationSource { get; set; }
        public decimal? Premium { get; set; }
        public decimal? Revenue { get; set; }
        public DateTime? TransactionDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime EndDate { get; set; }
       
    }
}
