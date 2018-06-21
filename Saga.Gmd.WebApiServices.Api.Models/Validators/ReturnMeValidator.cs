using FluentValidation;
using FluentValidation.Validators;
using Saga.Gmd.WebApiServices.Common;
using System.Collections.Generic;
using System;
using System.Linq;
using Saga.Gmd.WebApiServices.Api.Models.ParameterModels;
using Saga.Gmd.WebApiServices.Models;
using Saga.Gmd.WebApiServices.Models.ReturnMe;

namespace Saga.Gmd.WebApiServices.Api.Models.Validators
{
    public class KeyValueReturnMeValidator : AbstractValidator<ReturnMeForKeyValuePair>
    {

        public KeyValueReturnMeValidator()
        {
            RuleFor(x => x.CustomerKeys).SetValidator(new CustomerKeysCannotBeEmpty());
            RuleFor(x => x.CustomerMatch).SetValidator(new CustomerMatchValidator()).When(x => !string.IsNullOrEmpty(x.CustomerMatch.MatchType));
        }


        public class CustomerKeysCannotBeEmpty : PropertyValidator
        {
            public CustomerKeysCannotBeEmpty()
                : base(ErrorMessages.CustomerKeysCannotBeNullOrEmpty)
            {
            }

            protected override bool IsValid(PropertyValidatorContext context)
            {
                var parent = context.ParentContext.InstanceToValidate as ReturnMeForKeyValuePair;
                if (parent?.CustomerKeys != null)
                {
                    foreach (string a in parent.CustomerKeys)
                    {
                        return !string.IsNullOrEmpty(a.Trim());
                    }
                    
                }
                return true;
            }
        }


        public class CustomerMatchValidator : AbstractValidator<CustomerMatch>
        {
            public CustomerMatchValidator()
            {
                RuleFor(x => x.MatchType).Must(BeValidMatchType).WithMessage("MatchType is not valid type.");
            }

            private bool BeValidMatchType(string value)
            {
                MatchType matchType;
                return Enum.TryParse(value, true, out matchType);

            }
        }
    }
}

