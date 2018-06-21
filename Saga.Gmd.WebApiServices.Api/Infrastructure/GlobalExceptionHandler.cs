using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Results;
using Saga.Gmd.WebApiServices.Api.Models;

namespace Saga.Gmd.WebApiServices.Api.Infrastructure
{
    public class GlobalExceptionHandler : ExceptionHandler
    {
        public override void Handle(ExceptionHandlerContext context)
        {
            List<string> errors = new List<string>();
            errors.Add(context.Exception?.Message);

            var d = context.Exception.GetType();

            var responsePackage = new ResponsePackage(null, HttpStatusCode.BadRequest, string.Empty,string.Empty,errors);
            

            
            var response = context.Request.CreateResponse(HttpStatusCode.BadRequest, new { Errors = errors, HttpStatusCode = (int)HttpStatusCode.BadRequest, Message =context.Exception.Message });


            context.Result = new ResponseMessageResult(response);
        }

    }
}