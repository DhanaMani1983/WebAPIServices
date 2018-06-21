using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using Newtonsoft.Json;
using Saga.Gmd.WebApiServices.Api.Models.ParameterModels;
using Saga.Gmd.WebApiServices.Api.Models.Validators;

namespace Saga.Gmd.WebApiServices.Api.Models.ModelBinder
{
    public class MembershipProductSoldParameterModelBinder : IModelBinder
    {

        public MembershipProductSoldParameterModelBinder()
        {
        }

        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            string body = actionContext.Request.Content.ReadAsStringAsync().Result;
            MembershipProductSoldParameterModel model = new MembershipProductSoldParameterModel();

            model = JsonConvert.DeserializeObject<MembershipProductSoldParameterModel>(body);


            var validator = new MembershipProductSoldParameterModelValidator();
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
