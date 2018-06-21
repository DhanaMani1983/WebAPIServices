using System;
using FluentValidation;
using Saga.Gmd.WebApiServices.Common;
using Saga.Gmd.WebApiServices.Models;
using Saga.Gmd.WebApiServices.Models.MailingHistory;

namespace Saga.Gmd.WebApiServices.Api.Models.Validators
{
    public class AddressValidator : AbstractValidator<Address>
    {
        public AddressValidator()
        {
           
              
            //RuleFor(x => x.Postcode).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().Length(5, 8).WithMessage(ErrorMessages.PostcodeIsInValid)
            //    .When(x => x.MatchType == MatchType.NAME_AND_ADDRESS.ToLowerString() || x.MatchType == MatchType.POSTALADDRESS.ToLowerString()
            //    || x.MatchType == MatchType.POSTCODE.ToLowerString() || x.MatchType == MatchType.POSTCODE_AND_EMAIL.ToLowerString()
            //    || x.MatchType == MatchType.POSTCODE_AND_NAME.ToLowerString() || x.MatchType == MatchType.POSTCODE_AND_PHONE.ToLowerString());
            

            RuleFor(x => x.Address1)
                .Must(x => x?.Length <= 100)
                .WithMessage(ErrorMessages.Address1LengthError)
                .When(x => !string.IsNullOrEmpty(x.Address1));

            RuleFor(x => x.Address2)
               .Must(x => x?.Length <= 100)
               .WithMessage(ErrorMessages.Address2LengthError)
               .When(x => !string.IsNullOrEmpty(x.Address2));

            RuleFor(x => x.Address3)
               .Must(x => x?.Length <= 100)
               .WithMessage(ErrorMessages.Address3LengthError)
               .When(x => !string.IsNullOrEmpty(x.Address3));

            RuleFor(x => x.Address4)
               .Must(x => x?.Length <= 100)
               .WithMessage(ErrorMessages.Address4LengthError)
               .When(x => !string.IsNullOrEmpty(x.Address4));




        }
    }
}