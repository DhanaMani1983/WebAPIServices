using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.ModelBinding;
using FluentValidation.Attributes;
using Newtonsoft.Json;
using Saga.Gmd.WebApiServices.Api.Models.ModelBinder;
using Saga.Gmd.WebApiServices.Api.Models.Validators;
using Saga.Gmd.WebApiServices.Models.Customer;
using Saga.Gmd.WebApiServices.Models.Security;

namespace Saga.Gmd.WebApiServices.Api.Models.ParameterModels
{


    [ModelBinder(typeof(CustomerLoadOrMatchModelBinder))]
    [Validator(typeof(CustomerLoadValidator))]
    public class CustomerLoadOrMatch
    {
        public CustomerLoadOrMatch()
        {
            Customer = new CustomerLoad();
            AccessControl = new AccessControl(); 
        }
        public CustomerLoad Customer { get; set; } 
        [JsonIgnore]
        public AccessControl AccessControl { get; set; }
    }


    
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
