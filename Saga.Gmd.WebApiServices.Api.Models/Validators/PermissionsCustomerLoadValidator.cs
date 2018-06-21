using System;
using System.Linq;
using FluentValidation;
using FluentValidation.Validators;
using Saga.Gmd.WebApiServices.Common;
using Saga.Gmd.WebApiServices.Models.ReturnMe;
using System.Collections.Generic;
using Saga.Gmd.WebApiServices.Api.Models.ParameterModels;

namespace Saga.Gmd.WebApiServices.Api.Models.Validators
{

    public class PermissionsCustomerLoadValidator : AbstractValidator<PermissionsCustomerLoad>
    {
        public PermissionsCustomerLoadValidator()
        {
            RuleFor(c => c).Cascade(CascadeMode.StopOnFirstFailure)
                .Must(BeAValidPostType)
                .WithMessage(ErrorMessages.ValidPostType);

            RuleFor(x => x.CustomerKeyValue.Key).Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull().NotEmpty()//.WithMessage(ErrorMessages.ValidCustomerKey)
                .When(x => x.CustomerNameAndAddress == null);

            RuleFor(x => x.CustomerKeyValue.Value).Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull().NotEmpty()//.WithMessage(ErrorMessages.ValidCustomerValue)
                .When(x => x.CustomerNameAndAddress == null);

           
            RuleFor(c => c.PermissionsId).Cascade(CascadeMode.StopOnFirstFailure).NotNull().NotEmpty().WithMessage(ErrorMessages.PermissionsId);
            RuleFor(c => c.Source).Cascade(CascadeMode.StopOnFirstFailure).NotNull().NotEmpty().WithMessage(ErrorMessages.Source);
            
            RuleFor(c => c.QuestionId).Cascade(CascadeMode.StopOnFirstFailure).NotNull().NotEmpty().WithMessage(ErrorMessages.QuestionId);
            RuleFor(c => c.Journey).Cascade(CascadeMode.StopOnFirstFailure).NotNull().NotEmpty().WithMessage(ErrorMessages.Journey);
            RuleFor(c => c.JourneyType).Cascade(CascadeMode.StopOnFirstFailure).NotNull().NotEmpty().WithMessage(ErrorMessages.JourneyType);
            RuleFor(c => c.LastUpdatedDate).Cascade(CascadeMode.StopOnFirstFailure).NotNull().NotEmpty().WithMessage(ErrorMessages.LastUpdatedDate);
            RuleFor(c => c.LastUpdatedAgentName).Cascade(CascadeMode.StopOnFirstFailure).NotNull().NotEmpty().WithMessage(ErrorMessages.LastUpdatedAgentName);
            RuleFor(c => c.QuestionId).Cascade(CascadeMode.StopOnFirstFailure).NotNull().NotEmpty().WithMessage(ErrorMessages.QuestionId);

            RuleFor(model => model.PermissionCategory).Cascade(CascadeMode.StopOnFirstFailure)
                .Must(BeAValidPermissionCategory)
                .WithMessage(ErrorMessages.PermissionCategory)
                .Must(BeAValidPermissionCategotyDisplayValue)
                .WithMessage(ErrorMessages.PermissionCategoryDisplayValue)
                .Must(BeAValidPermissionCategotyStatus)
                .WithMessage(ErrorMessages.PermissionCategoryStatus);


            RuleFor(x => x.CustomerNameAndAddress.Surname).Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull().NotEmpty().WithMessage(ErrorMessages.SurnameCannotBeEmpty)
                .When(x => x.CustomerNameAndAddress != null);
            RuleFor(x => x.CustomerNameAndAddress).Cascade(CascadeMode.StopOnFirstFailure)
                .SetValidator(new NameAndAddressValidator())
                .When(x => x.CustomerNameAndAddress != null);
            RuleFor(x => x.CustomerNameAndAddress.CustomerAddress).Cascade(CascadeMode.StopOnFirstFailure)
                .SetValidator(new CustomerAddressValidator())
                .When(x => x.CustomerNameAndAddress != null);
        }

        private bool BeAValidPostType(PermissionsCustomerLoad arg)
        {
            return !string.IsNullOrWhiteSpace(arg.CustomerKeyValue.Key) || arg.CustomerNameAndAddress != null;
        }

        private bool BeAValidPermissionCategory(List<ChannelFlagsPost> flags)
        {
            return flags?.Count != 0;
        }

        private bool BeAValidPermissionCategotyStatus(List<ChannelFlagsPost> flags)
        {
            return flags.All(flag => !string.IsNullOrWhiteSpace(flag.PermissionCategoryStatus));
        }

        private bool BeAValidPermissionCategotyDisplayValue(List<ChannelFlagsPost> flags)
        {
            return flags.All(flag => !string.IsNullOrWhiteSpace(flag.PermissionCategoryDisplayValue));
        }
    }
}
