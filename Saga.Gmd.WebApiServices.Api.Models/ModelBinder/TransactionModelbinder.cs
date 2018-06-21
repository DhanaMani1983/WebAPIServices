using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using Newtonsoft.Json;
using Saga.Gmd.WebApiServices.Api.Models.Validators;
using Saga.Gmd.WebApiServices.Models.Transaction;

namespace Saga.Gmd.WebApiServices.Api.Models.ModelBinder
{
    public class TransactionModelbinder : IModelBinder
    {
        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            string body = actionContext.Request.Content.ReadAsStringAsync().Result; 
            var model = JsonConvert.DeserializeObject<ParameterModels.TransactionParamter>(body);
            model.RequestType = "Quantum";
            var finalTransactionParamter = model;
            finalTransactionParamter.Customer.TransactionBrand = model.TransactionBrand;
            finalTransactionParamter.Customer.SystemSource = model.SourceSystem;
            finalTransactionParamter.Customer.MarketingSource = model.MarketingSource;
            finalTransactionParamter.Customer.TransactionType = model.MarketingSource;

            var transactionValidator = new TransactionParamterValidator();
            var validateResult =  transactionValidator.Validate(finalTransactionParamter); 

            foreach (var e in validateResult.Errors)
            {
                bindingContext.ModelState.AddModelError(e.PropertyName, e.ErrorMessage);
            }
            bindingContext.Model = finalTransactionParamter;
            return true;

        }
    }
}
