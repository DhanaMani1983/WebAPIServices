using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Validators;
using Saga.Gmd.WebApiServices.Api.Models.ParameterModels;
using Saga.Gmd.WebApiServices.Common;
using Saga.Gmd.WebApiServices.Models;
using Saga.Gmd.WebApiServices.Models.MailingHistory;
using Saga.Gmd.WebApiServices.Models.ReturnMe;

namespace Saga.Gmd.WebApiServices.Api.Models.Validators
{
   public class NameAndAddressReturnMeValidator : AbstractValidator<ReturnMe>
    {

        public NameAndAddressReturnMeValidator()
        {
            RuleFor(x => x.CustomerMatch.MatchType).Must(BeValidMatchType).When(x => x.CustomerMatch.MatchType != null).WithMessage("MatchType is not valid type.");
        }

        private bool BeValidMatchType(string value)
        {
            MatchType type;
            var isValid = Enum.TryParse(value.ToUpper().Trim(), out type);
            return isValid;
        }

        public class ReturnMeCannotBeEmpty : PropertyValidator
        {
            public ReturnMeCannotBeEmpty()
                : base(ErrorMessages.ReturnMeCannotBeEmpty)
            {
            }

            protected override bool IsValid(PropertyValidatorContext context)
            {
                var parent = context.ParentContext.InstanceToValidate as KeyValueParameter;


                if (parent.ResponseRequestedItems == null ? false : parent.ResponseRequestedItems.Count == 0)
                {
                    return false;
                }
                return true;
            }
        }

        public class CustomerKeysCannotBeEmpty : PropertyValidator
        {
            public CustomerKeysCannotBeEmpty()
                : base(ErrorMessages.CustomerKeysCannotBeNullOrEmpty)
            {
            }

            protected override bool IsValid(PropertyValidatorContext context)
            {
                var parent = context.ParentContext.InstanceToValidate as NameAndAddressParameter;
                var returnMe = context.PropertyValue as ReturnMe;

                if (parent != null && (parent.ResponseRequestedItems?.Exists(a => a.Equals("customerkeys", StringComparison.OrdinalIgnoreCase)) ?? true))
                {
                    return (returnMe?.CustomerKeys?.Count > 0);
                }
                return true;
            }
        }
    }
}
