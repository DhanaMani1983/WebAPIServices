using FluentValidation;

using Saga.Gmd.WebApiServices.Models.Customer;
using Saga.Gmd.WebApiServices.Common;
using Saga.Gmd.WebApiServices.Models.MailingHistory;

namespace Saga.Gmd.WebApiServices.Api.Models.Validators
{
    public class CustomerAddressValidator : AbstractValidator<CustomerAddress>
    {
        public CustomerAddressValidator()
        {
            RuleFor(c => c).NotNull().WithMessage(ErrorMessages.Address);
            // RuleFor(c => c.HouseNumber).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().Length(0, 100);
            // RuleFor(c => c.HouseName).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().Length(0, 100);
            // RuleFor(c => c.Street).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().Length(0, 100);
            // RuleFor(c => c.Street1).Length(0, 100);
            // AE Suspend both rules : Jira GS-76 
            RuleFor(c => c.City).Length(0, 100);
            RuleFor(c => c.County).Length(0, 100).WithMessage(ErrorMessages.County);
            RuleFor(c => c.Postcode).Cascade(CascadeMode.StopOnFirstFailure).Length(0, 8).WithMessage(ErrorMessages.PostcodeRequried); 
        }
    }
}
