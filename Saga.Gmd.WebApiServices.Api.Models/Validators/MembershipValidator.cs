using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Saga.Gmd.WebApiServices.Api.Models.JsonModels;
using Saga.Gmd.WebApiServices.Common;

namespace Saga.Gmd.WebApiServices.Api.Models.Validators
{
    public class MembershipValidator : AbstractValidator<MembershipOutput>
    {
        public MembershipValidator()
        {
            RuleFor(x => x.MembershipData)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull()
                .WithMessage(ErrorMessages.MemberObjectIsNull);
        }
    }
}
