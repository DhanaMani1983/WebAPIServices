﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Saga.Gmd.WebApiServices.Models.Security;

namespace Saga.Gmd.WebApiServices.Models.Customer
{
     

    public class CustomerLoad
    { 
      
        public string SystemId { get; set; }
        public string SystemSource { get; set; }
        public string MarketingSource { get; set; }
        public string TransactionType { get; set; }
        public string TransactionBrand { get; set; } 
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
        public CustomerAddress Address { get; set; }
        public string EmploymentStatus { get; set; }
        public string Occupation { get; set; }
        public string MaritalStatus { get; set; }
        public bool? StaffFlag { get; set; }
        public DateTime? RetirementDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }

        [JsonIgnore]
        public AccessControl AccessControl { get; set; }


    }
}
