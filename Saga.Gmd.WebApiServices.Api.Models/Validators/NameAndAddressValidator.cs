using FluentValidation;
using Saga.Gmd.WebApiServices.Common;
using Saga.Gmd.WebApiServices.Models;
using System;
using FluentValidation.Validators;
using Saga.Gmd.WebApiServices.Api.Models.ParameterModels;
using Saga.Gmd.WebApiServices.Models.MailingHistory;


namespace Saga.Gmd.WebApiServices.Api.Models.Validators
{

    public class NameAndAddressParameterValidator : AbstractValidator<NameAndAddressParameter>
    {
        public NameAndAddressParameterValidator()
        {
            RuleFor(x => x.NameAndAddress).SetValidator(new NameAndAddressValidator());
            RuleFor(x => x.ReturnMe).SetValidator(new NameAndAddressReturnMeCannotBeEmpty()).SetValidator(new NameAndAddressReturnMeValidator());
           
        }


      
    }


    public class NameAndAddressReturnMeCannotBeEmpty : PropertyValidator
    {
        public NameAndAddressReturnMeCannotBeEmpty() : base(ErrorMessages.ReturnMeCannotBeEmpty)
        {
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            var parent = context.ParentContext.InstanceToValidate as NameAndAddressParameter;


            if (parent != null && parent.ResponseRequestedItems.Count == 0)
            {
                return false;
            }
            return true;
        }
    }


    public class NameAndAddressValidator : AbstractValidator<NameAndAddress>
    {
        public NameAndAddressValidator()
        {
            //  RuleFor(x => x.Surname).Length(1, 30);
            RuleFor(x => x.Surname).Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull().When(x => x.MatchType == MatchType.NAME_AND_ADDRESS.ToLowerString()
                || x.MatchType == MatchType.NAME.ToLowerString()
                || x.MatchType == MatchType.NAME_AND_EMAIL.ToLowerString()
                || x.MatchType == MatchType.NAME_AND_PHONE.ToLowerString()
                || x.MatchType == MatchType.POSTCODE_AND_NAME.ToLowerString());

            RuleFor(x => x.Title)
                .Must(x => string.IsNullOrEmpty(x) || x.Length <= 30)
                .WithMessage(ErrorMessages.TitleLengthError);

            RuleFor(x => x.FirstName)
                .Must(x => string.IsNullOrEmpty(x) || x.Length <= 50)
                .WithMessage(ErrorMessages.FirstNameLengthError);

            RuleFor(x => x.Email).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().EmailAddress().Length(6, 40)
                .When(x => x.MatchType == MatchType.EMAIL.ToLowerString()
                 || x.MatchType == MatchType.NAME_AND_EMAIL.ToLowerString()
                 || x.MatchType == MatchType.POSTCODE_AND_EMAIL.ToLowerString());

            RuleFor(x => x.Dob)
            .Must(x => x == null || BeAValidDate(x.Value))
            .WithMessage(ErrorMessages.DOBIsInValid);

            RuleFor(x => x.Address).Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull().WithMessage(ErrorMessages.AddressCannotBeEmpty)
                .Must(BeAtleastOneLineOfAddress).When(x => x.MatchType == MatchType.NAME_AND_ADDRESS.ToLowerString()
                || x.MatchType == MatchType.POSTALADDRESS.ToLowerString())
                .WithMessage(ErrorMessages.ProvideAtleastOneLineOfAddress);

            RuleFor(x => x.Phone)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().Must(x => string.IsNullOrEmpty(x) || x.Length <= 15)
                .When(x => x.MatchType == MatchType.PHONE.ToLowerString()
                || x.MatchType == MatchType.NAME_AND_PHONE.ToLowerString()
                || x.MatchType == MatchType.POSTCODE_AND_PHONE.ToLowerString());

            RuleFor(x => x.Address).SetValidator(new AddressValidator());
        }

        private bool BeAtleastOneLineOfAddress(Address address)
        {
            return !(string.IsNullOrEmpty(address?.Address1)
                 && string.IsNullOrEmpty(address?.Address2)
                 && string.IsNullOrEmpty(address?.Address3)
                 && string.IsNullOrEmpty(address?.Address4));
        }

        private bool BeAValidDate(DateTime value)
        {
            return value != DateTime.MaxValue;
        }
    }
}