using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.Attributes;
using Saga.Gmd.WebApiServices.Api.Models.ModelBinder;
using Saga.Gmd.WebApiServices.Api.Models.Validators;
using System.Web.Http.ModelBinding;

namespace Saga.Gmd.WebApiServices.Api.Models.ParameterModels
{
    [ModelBinder(typeof(MoveCustomerKeyModelBinder))]
    [Validator(typeof(CustomerKeyMoveParameterValidator))]
    public class CustomerKeyMoveParameter
    {  
        public string OriginalSourceKey { get; set; }
        public string OriginalSourceId { get; set; }
        public string NewSourceKey { get; set; }
        public string NewSourceId { get; set; }
        public string ToMoveSourceKey { get; set; }
        public string ToMoveSourceId { get; set; }
    }
}
