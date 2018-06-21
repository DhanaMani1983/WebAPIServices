using FluentValidation;
using Saga.Gmd.WebApiServices.Api.Models.ParameterModels;
using Saga.Gmd.WebApiServices.Common;
 

namespace Saga.Gmd.WebApiServices.Api.Models.Validators
{
    
    public class CustomerLoadValidator : AbstractValidator<CustomerLoad>
    {
        public CustomerLoadValidator()
        {
            RuleFor(x => x.Gender).Matches("^[a-zA-Z-/\\s]+$").Length(0, 20); // Allow no gender for ACS transactions
            RuleFor(x => x.SystemId).Cascade(CascadeMode.StopOnFirstFailure).NotNull().NotEmpty().WithMessage(ErrorMessages.SystemIdCanntBeEmpty);
            RuleFor(x => x.MarketingSource).NotEmpty().WithMessage(ErrorMessages.MarketingSourceCanntobempty);
            RuleFor(x => x.TransactionType).NotEmpty().WithMessage(ErrorMessages.TractionTypeCannotbeempty);
            RuleFor(x => x.TransactionBrand).NotEmpty().WithMessage(ErrorMessages.TransactionBrandCannotbempty);
            RuleFor(x => x.SystemSource).Matches("^[a-zA-Z]+$").NotEmpty().WithMessage(ErrorMessages.SystemSourceCannotBeEmpty);
            RuleFor(x => x.Address).SetValidator(new CustomerAddressValidator());
          //RuleFor(x => x.Title).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().Length(2,30);
          //RuleFor(x => x.Surname).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().Length(1,50);
            RuleFor(x => x.Forename).Cascade(CascadeMode.StopOnFirstFailure).Length(0,50).When(x => !string.IsNullOrEmpty(x.Forename));
            RuleFor(x => x.EmploymentStatus).Length(0, 50);
          //RuleFor(x => x.EmailAddress).EmailAddress().Length(0, 50);
            RuleFor(x => x.TelephoneHome).Length(0, 15);
            RuleFor(x => x.TelephoneMobile).Length(0, 15);
            RuleFor(x => x.Occupation).Length(0, 50);
            RuleFor(x => x.MaritalStatus).Length(0,50).When(x => !string.IsNullOrEmpty(x.MaritalStatus));
            RuleFor(x => x.SystemId).Cascade(CascadeMode.StopOnFirstFailure).NotNull().NotNull();

        } 
    }


}
