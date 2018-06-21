using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Saga.Gmd.WebApiServices.Api.Models.ParameterModels;

namespace Saga.Gmd.WebApiServices.Api.Models.Validators
{
    public class PermissionParameterValidator : AbstractValidator<PermissionParameter>
    {
        public PermissionParameterValidator()
        {

        }
    }
}
