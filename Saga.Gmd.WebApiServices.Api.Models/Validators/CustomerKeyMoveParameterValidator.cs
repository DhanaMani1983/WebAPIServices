using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Saga.Gmd.WebApiServices.Api.Models.ParameterModels;
using Saga.Gmd.WebApiServices.Common;

namespace Saga.Gmd.WebApiServices.Api.Models.Validators
{
    class CustomerKeyMoveParameterValidator:AbstractValidator<CustomerKeyMoveParameter>
    {
        public CustomerKeyMoveParameterValidator()
        {
            RuleFor(c => c.NewSourceId).NotEmpty().WithMessage(ErrorMessages.NewSourceId);
            RuleFor(c => c.NewSourceKey).NotEmpty().WithMessage(ErrorMessages.NewSourceKey);
            RuleFor(c => c.OriginalSourceId).NotEmpty().WithMessage(ErrorMessages.OriginalSourceId);
            RuleFor(c => c.OriginalSourceKey).NotEmpty().WithMessage(ErrorMessages.OriginalSourceKey);
            RuleFor(c => c.ToMoveSourceId).NotEmpty().WithMessage(ErrorMessages.ToMoveSourceId);
            RuleFor(c => c.ToMoveSourceKey).NotEmpty().WithMessage(ErrorMessages.ToMoveSourceKey);
        }
    }
}
