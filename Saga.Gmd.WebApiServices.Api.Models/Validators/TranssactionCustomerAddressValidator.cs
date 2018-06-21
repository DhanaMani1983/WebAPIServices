using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Saga.Gmd.WebApiServices.Common;
using Saga.Gmd.WebApiServices.Models.Customer;

namespace Saga.Gmd.WebApiServices.Api.Models.Validators
{
    public class TranssactionCustomerAddressValidator : AbstractValidator<TransactionCusotmerAddress>
    {
        public TranssactionCustomerAddressValidator()
        {
            RuleFor(c => c).NotNull().WithMessage(ErrorMessages.Address);
            // ACS Addresses do not have housename/housenumber, all incorporated in Street property.
            //RuleFor(c => c.HouseNumber).Cascade(CascadeMode.StopOnFirstFailure).Length(0, 10).NotEmpty();
            //RuleFor(c => c.HouseName).Cascade(CascadeMode.StopOnFirstFailure).Length(0, 50).NotEmpty();

            // RuleFor(x => x.Street).NotEmpty();
            // AE Suspend rule : Jira GS-76 
            RuleFor(c => c.City).Length(0,30);
            RuleFor(c => c.County).Cascade(CascadeMode.StopOnFirstFailure).Length(0, 30); 
            RuleFor(c => c.Country).Length(0, 30);  
            RuleFor(c => c.Postcode).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().Length(5, 8); 
        }
    }
}
