using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using Saga.Gmd.WebApiServices.Api.Models.ParameterModels;
using Saga.Gmd.WebApiServices.Api.Models.Validators;
using Saga.Gmd.WebApiServices.Common;

namespace Saga.Gmd.WebApiServices.Api.Models.ModelBinder
{
    public class MoveCustomerKeyModelBinder : IModelBinder
    {
        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            CustomerKeyMoveParameter model = new CustomerKeyMoveParameter();

            var a = actionContext.Request.Content.ReadAsStringAsync().Result;
            model.OriginalSourceKey = bindingContext.GetValue("OriginalSourceKey").ToType<string>();
            model.OriginalSourceId = bindingContext.GetValue("OriginalSourceId").ToType<string>();
            model.NewSourceKey = bindingContext.GetValue("NewSourceKey").ToType<string>();
            model.NewSourceId = bindingContext.GetValue("NewSourceId").ToType<string>();
            model.ToMoveSourceId = bindingContext.GetValue("ToMoveSourceId").ToType<string>();
            model.ToMoveSourceKey = bindingContext.GetValue("ToMoveSourceKey").ToType<string>();

            var validator = new CustomerKeyMoveParameterValidator();
            var result = validator.Validate(model);

            foreach (var e in result.Errors)
            {
                bindingContext.ModelState.AddModelError(e.PropertyName, e.ErrorMessage);
            }

            bindingContext.Model = model;
            return true;
        }
    }
}