using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Saga.Gmd.WebApiServices.Common;
using Saga.Gmd.WebApiServices.Models.Customer;
using Customerload = Saga.Gmd.WebApiServices.Api.Models.ParameterModels.Customerload;

namespace Saga.Gmd.WebApiServices.Api.Models.Validators
{
    public class CustomerloadValidator : AbstractValidator<Customerload>
    {
        public CustomerloadValidator()
        {
            RuleFor(x => x.SystemId).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().WithMessage(ErrorMessages.SystemIdCanntBeEmpty);
            RuleFor(x => x.SystemSource).NotEmpty().WithMessage(ErrorMessages.SystemSourceCannotBeEmpty);
            RuleFor(x => x.MarketingSource).NotEmpty().WithMessage(ErrorMessages.MarketingSourceCanntobempty);
            RuleFor(x => x.TransactionType).NotEmpty().WithMessage(ErrorMessages.TractionTypeCannotbeempty);
            RuleFor(x => x.TransactionBrand).NotEmpty().WithMessage(ErrorMessages.TransactionBrandCannotbempty);
            RuleFor(x => x.Gender)
                .Must( BeValidGender)
                .WithMessage(ErrorMessages.Gender).When(s => s.Gender.HasValue);
            RuleFor(x => x.Address).SetValidator(new CustomerloadAddressValidator());
        }

        public bool BeValidGender(char? value)
        {
            if (value != null)
                return (value.Value.ToString() == "F" || value.Value.ToString() == "M");
            return false;
        }
    }
}
