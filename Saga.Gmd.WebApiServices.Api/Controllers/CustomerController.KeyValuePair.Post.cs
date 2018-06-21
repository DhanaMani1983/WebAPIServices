using System.Web.Http;
using Saga.Gmd.WebApiServices.Models.Customer;

namespace Saga.Gmd.WebApiServices.Api.Controllers
{
    public partial class CustomerController
    { 
        [HttpPost]
        [ActionName("KeyValuePair")]
        public IHttpActionResult PostBykeyValue([FromBody]KeyValuePairParams keyValuePairParams)
        {
            return Ok();
        } 
        
    }
}