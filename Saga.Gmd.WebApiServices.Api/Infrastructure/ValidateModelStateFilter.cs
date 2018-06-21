using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using log4net;
using log4net.Core;

namespace Saga.Gmd.WebApiServices.Api.Infrastructure
{
    public class ValidateModelStateFilter : ActionFilterAttribute
    { 
        public override void OnActionExecuting(HttpActionContext actionContext)
        { 
            if (!actionContext.ModelState.IsValid)
            {
                actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, actionContext.ModelState);
            }
        }
    }
}