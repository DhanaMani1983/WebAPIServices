using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Saga.Gmd.WebApiServices.Models.Permissions;
using Saga.Gmd.WebApiServices.Common;

namespace Saga.Gmd.WebApiServices.Api.Models.Validators
{
    public class PermissionValidator:AbstractValidator<Permission>
    {
        public PermissionValidator()
        {
            RuleFor(x => x.Detail).Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull().WithMessage(ErrorMessages.PermissionInvalidValue);

            RuleFor(x => x.Detail.Count).LessThanOrEqualTo(0).WithMessage(ErrorMessages.PermissionDetailsInvalidValue);
        }

    }
}
