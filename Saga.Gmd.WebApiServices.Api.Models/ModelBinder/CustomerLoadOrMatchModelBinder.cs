using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using Newtonsoft.Json;
using Saga.Gmd.WebApiServices.Api.Models.ParameterModels;
using Saga.Gmd.WebApiServices.Api.Models.Validators;
using Saga.Gmd.WebApiServices.Common;


namespace Saga.Gmd.WebApiServices.Api.Models.ModelBinder
{
    public class CustomerLoadOrMatchModelBinder : IModelBinder
    {
        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            string body = actionContext.Request.Content.ReadAsStringAsync().Result;
            CustomerLoadOrMatch model = new CustomerLoadOrMatch();
            model = JsonConvert.DeserializeObject<CustomerLoadOrMatch>(body); 
            
            //List<string> performingAction = new List<string> {"cload"};
            //model.AccessControl.ModuleAccess = performingAction.ToGroupCodeEnum();

           var validator = new CustomerLoadValidator();
            var result = validator.Validate(model.Customer);
    
            foreach (var e in result.Errors)
            {
                bindingContext.ModelState.AddModelError(e.PropertyName, e.ErrorMessage);
            }
            bindingContext.Model = model;
            return true;
        }
    }
}
