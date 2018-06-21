using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.ModelBinding;
using AutoMapper;
using Saga.Gmd.WebApiServices.Api.Models;
using Saga.Gmd.WebApiServices.Api.Models.ModelBinder;
using Saga.Gmd.WebApiServices.Api.Models.ParameterModels;
using Saga.SSO.ClientCredentials.Endpoint.ApiFilters;

namespace Saga.Gmd.WebApiServices.Api.Controllers
{
	public partial class CustomerController
	{
        
        [ResponseType(typeof(ResponsePackage))]
        [AuthenticateIdentity()]
        [AuthorizeIdentity(Role = "CUSTOMER_LOAD")]
        [HttpPost]
        [ActionName("CustomerTransactionalLoad")]
        public IHttpActionResult Transaction(TransactionParamter transaction)
        {
            var mappedTransaction = Mapper.Map<WebApiServices.Models.Transaction.TransactionParamter>(transaction);

            int output = _transactionServices.Save(mappedTransaction);

            return Ok($"Transaction loaded successfully. [Id={output}]");
        }
    }
}