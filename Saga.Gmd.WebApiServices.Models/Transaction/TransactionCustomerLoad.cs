using System;
using Newtonsoft.Json;
using Saga.Gmd.WebApiServices.Models.Customer;

namespace Saga.Gmd.WebApiServices.Models.Transaction
{
    public class TransactionCustomerLoad
    {
        
        public string SystemId { get; set; }
        [JsonIgnore]
        public string SystemSource { get; set; }
        [JsonIgnore]
        public string MarketingSource { get; set; }
        [JsonIgnore]
        public string TransactionType { get; set; }
        [JsonIgnore]
        public string TransactionBrand { get; set; }
        [JsonIgnore]
        public string Brand { get; set; }
        public string Title { get; set; }
        public string Forename { get; set; }
        public string Surname { get; set; }
        public string Suffix { get; set; }
        public string Gender { get; set; }
        public string EmailAddress { get; set; }
        public bool? EmailMarketingUse { get; set; }
        public bool? EmailTransactionalUse { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string TelephoneMobile { get; set; }
        public string TelephoneHome { get; set; }
        public bool? Deleted { get; set; }
        public bool? IsVerified { get; set; }
        public TransactionCusotmerAddress Address { get; set; }
        public string EmploymentStatus { get; set; }
        public string Occupation { get; set; }
        public string MaritalStatus { get; set; }
        public bool? StaffFlag { get; set; }
        public DateTime? RetirementDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }

    
    }
}