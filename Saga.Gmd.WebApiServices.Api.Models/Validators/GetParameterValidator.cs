using FluentValidation;
using Saga.Gmd.WebApiServices.Common;
using System;
using Saga.Gmd.WebApiServices.Api.Models.Parameters;
using Saga.Gmd.WebApiServices.Models;

namespace Saga.Gmd.WebApiServices.Api.Models.Validators
{
    public class GetParameterValidator : AbstractValidator<GetParameters>
    {

        public GetParameterValidator()
        {
            RuleFor(x => x.IwillSend)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull()
                .WithMessage(ErrorMessages.IWillSendCanNotBeNull)
                .Must(BeValidPrameterType).WithMessage(ErrorMessages.IWilSendInvalidValue);

            RuleFor(x => x.NameAndAddress).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .When(x => x.IwillSend?.ToLower() == ParameterType.NameAndAddress.ToString().ToLower())
                .WithMessage(ErrorMessages.NameAndAddressCannotBeEmpty);

            RuleFor(x => x.KeyValue).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .WithMessage(ErrorMessages.KeyValueCannotBeEmpty)
                .When(x => x.IwillSend?.ToLower() == ParameterType.KeyValuePair.ToString().ToLower());

            //RuleFor(x => x.KeyValue)
            //    .SetValidator(new KeyValueValidator()).When(x => x.IwillSend?.ToLower() == ParameterType.KeyValuePair.ToString().ToLower());

            RuleFor(x => x.NameAndAddress)
                .SetValidator(new NameAndAddressValidator())
                .When(x => x.IwillSend?.ToLower() == ParameterType.NameAndAddress.ToString().ToLower());

            //RuleFor(x => x.ReturnMe).Cascade(CascadeMode.StopOnFirstFailure)
            //    .NotNull().WithMessage(ErrorMessages.ReturnMeCannotBeEmpty)
            //    .SetValidator(new ReturnMeValidator.ReturnMeCannotBeEmpty())
            //    .SetValidator(new ReturnMeValidator.CustomerKeysCannotBeEmpty())
            //    .SetValidator(new ReturnMeValidator());
        }

        private bool BeValidPrameterType(string value)
        {
            ParameterType type;
            var foundItem = Enum.TryParse(value, true, out type);
            return foundItem;
        }
    }
}