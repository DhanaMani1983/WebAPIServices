using System.Linq;
using FluentValidation;
using FluentValidation.Validators;
using Saga.Gmd.WebApiServices.Api.Models.ParameterModels;
using Saga.Gmd.WebApiServices.Common;

namespace Saga.Gmd.WebApiServices.Api.Models.Validators
{
    public class MembershipDataInputValidator : AbstractValidator<MembershipDataInputModel>
    {
        public MembershipDataInputValidator()
        {
            RuleFor(x => x.ActivationId).SetValidator(new ActivationIdOrEncryptedActivationIdMustBeThere());
            RuleFor(x => x.DeclineReason).NotEmpty().When(x => x.UpdatedStatus == "Decline");
            RuleFor(x => x.AgentName).NotEmpty();
            RuleFor(x => x.OverrideMust).SetValidator(new OverrideMustSet());
        }
    }

    public class MembershipCancellationParameterModelValidator : AbstractValidator<MembershipCancellationParameterModel>
    {
        public MembershipCancellationParameterModelValidator()
        {
            RuleFor(x => x.MembershipNo).SetValidator(new MembershipNumbersMustBeThere());
            RuleFor(x => x.CancellationReason).SetValidator(new CancelReasonMustBeValid());
        }

    }

    public class MembershipNumbersMustBeThere : PropertyValidator
    {
        public MembershipNumbersMustBeThere() : base("Both MembershipNumber values can't be empty.")
        {
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            // GITCS-9 : Validate non-empty Membership No values 
            var parent = context.ParentContext.InstanceToValidate as MembershipCancellationParameterModel;
            if (parent != null)
            {
                return ((parent.MembershipNo > 0) || !string.IsNullOrEmpty(parent.EncryptedMembershipNo));
            }
            else
            {
                return false;
            }
        }
    }



    public class MembershipProductSoldParameterModelValidator : AbstractValidator<MembershipProductSoldParameterModel>
    {
        public MembershipProductSoldParameterModelValidator()
        {

            RuleFor(x => x.ActivationId).SetValidator(new ProductSoldValuesMustBePresent());

        }
    }

    public class ProductSoldValuesMustBePresent : PropertyValidator
    {
        public ProductSoldValuesMustBePresent() : base("Activation Id or Membership Id values must be specified")
        {
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            var parent = context.ParentContext.InstanceToValidate as MembershipProductSoldParameterModel;
            bool isRuleValid;
            if (parent != null &&
                (
                    !parent.MembershipNo.HasValue &&
                    string.IsNullOrEmpty(parent.EncryptedMembershipNo) &&
                    string.IsNullOrEmpty(parent.ActivationId) &&
                    string.IsNullOrEmpty(parent.EncryptedActivationId)
                )
            )
            {
                isRuleValid = false;
            }
            else
                isRuleValid = true;

            DiagnosticHelper.DebugWriteFmt("ProductSoldValuesMustBePresent.IsValid() : {0}", isRuleValid);
            return isRuleValid;
        }

    }

    public class CancelReasonMustBeValid : PropertyValidator
    {
        public CancelReasonMustBeValid() : base(
            "{ValidationMesage}"
        )
        {
        }
        protected override bool IsValid(PropertyValidatorContext context)
        {
            var inputModelToValidate = context.ParentContext.InstanceToValidate as MembershipCancellationParameterModel;


            bool isValid = (inputModelToValidate != null &&
                            inputModelToValidate.ValidMembershipCancellationReasons.Any(
                                vmo => /* vmo.ShortCode == inputModelToValidate.CancellationReason || */
                                       vmo.Description == inputModelToValidate.CancellationReason
                            ) // Passed cancel reason can be either the code or full description - must exist in the ValidMembershipCancellationReasons array
            );
            if (!isValid)
            {
                // Dynamic error message creation 
                context.MessageFormatter.AppendArgument(
                    "ValidationMesage",  // Format string name (set in base Constructor call)
                    $"Please use a valid cancel reason from the set: {string.Join(",", inputModelToValidate.ValidMembershipCancellationReasons.Select(mo => mo.Description).ToArray())}"
                    // Format string value 
                );
            }

            return isValid;

        }

    }


    public class OverrideMustSet : PropertyValidator
    {
        public OverrideMustSet() : base(
            "Please set 'OverrideReason' and OverrideFlag to 'True' to Activte Non-Eligible Customer.")
        {

        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            var parent = context.ParentContext.InstanceToValidate as MembershipDataInputModel;
            if (parent != null && (!parent.OverrideMust && string.IsNullOrEmpty(parent.OverrideReason)))
            {
                return false;
            }

            return true;
        }
    }

    public class ActivationIdOrEncryptedActivationIdMustBeThere : PropertyValidator
    {

        public ActivationIdOrEncryptedActivationIdMustBeThere() : base(
            "ActivationId or EncryptedActivationId can't be empty.")
        {
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            var parent = context.ParentContext.InstanceToValidate as MembershipDataInputModel;
            if (parent != null && (string.IsNullOrEmpty(parent.ActivationId) &&
                                   string.IsNullOrEmpty(parent.EncryptedActivationId)))
            {
                return false;
            }

            return true;
        }
    }

}