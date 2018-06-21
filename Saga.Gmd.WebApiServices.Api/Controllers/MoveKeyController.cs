using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using AutoMapper;
using log4net;
using Saga.Gmd.WebApiServices.Api.Models;
using Saga.Gmd.WebApiServices.Api.Models.ParameterModels;
using Saga.Gmd.WebApiServices.BLL.Customer;
using Saga.Gmd.WebApiServices.BLL.Membership;
using Saga.Gmd.WebApiServices.BLL.Permissions;
using Saga.Gmd.WebApiServices.BLL.Transaction;
using Saga.SSO.ClientCredentials.Endpoint.ApiFilters;
using Saga.Gmd.WebApiServices.Models.Customer;

namespace Saga.Gmd.WebApiServices.Api.Controllers
{
    public class MoveKeyController : ApiController
    {

        
        private readonly ILog _logger;
        private readonly ICustomerKeyMoveService _customerKeyMoveServiceService;
        

        public MoveKeyController(ILog logger, ICustomerKeyMoveService customerKeyMoveServiceService)
        {
            _logger = logger;
            _customerKeyMoveServiceService= customerKeyMoveServiceService;
        }

        [ResponseType(typeof(ResponsePackage))]
        [AuthenticateIdentity()]
        //[AuthorizeIdentity(Role = "CUSTOMER_LOAD")]
        [AuthorizeIdentity(Role = "CUSTOMER_MOVE_KEY")]
        [HttpGet]
        [ActionName("MoveCustomerKey")]
        public IHttpActionResult MoveCustomerKey(CustomerKeyMoveParameter transaction)
        {
            var mappedCustomerKeyMove = Mapper.Map<CustomerKeyMove>(transaction);

            return Ok(_customerKeyMoveServiceService.MoveCustomerKey(mappedCustomerKeyMove));
        }
    }
}
