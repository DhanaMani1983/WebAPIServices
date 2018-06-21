using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Dispatcher;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Routing;
using Elmah.Contrib.WebApi;
using FluentValidation.WebApi;
using log4net.Core;
using Newtonsoft.Json;
using Saga.Gmd.WebApiServices.Api.Infrastructure;
using AutoMapper;
using Saga.Gmd.WebApiServices.Api.App_Start;
using Saga.Gmd.WebApiServices.Api.Controllers;

namespace Saga.Gmd.WebApiServices.Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            config.Filters.Add(new ValidateModelStateFilter());

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Services.Add(typeof(IExceptionLogger), new ElmahExceptionLogger());
            var json = config.Formatters.JsonFormatter;
            json.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.None;
            config.Formatters.Remove(config.Formatters.XmlFormatter);
           // config.Services.Replace(typeof(IExceptionHandler),new GlobalExceptionHandler());

            // Add ResponseWrappingHandler() for all requests (incl V2 routes)
            config.MessageHandlers.Add(new ResponseWrappingHandler() );

            config.Routes.MapHttpRoute(
              name: "DefaultApi2",
              routeTemplate: "api/v1/{controller}/{action}",
              defaults: new {controller = "CustomerController"},
              constraints: new { httpmethod = new HttpMethodConstraint(HttpMethod.Post) }
              // , handler: new ResponseWrappingHandler() { InnerHandler = new HttpControllerDispatcher(config) }
              );

            config.Routes.MapHttpRoute(
                name: "DefaultApiV1",
                routeTemplate: "api/v1/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional , controller = "CustomerController" },
                constraints: null
                // , handler: new ResponseWrappingHandler() { InnerHandler = new HttpControllerDispatcher(config) }
                );

            // V2 route setup - Not working : Using controller method Route() attributes instead...
            // This means that the ResponseWrappingHandler assigned here does not get invoked as the V2 
            // controller is only hit by virtue of the Controller Method's Route() attribute 
            //config.Routes.MapHttpRoute(
            //   name: "DefaultApiV2",
            //   routeTemplate: "api/v2/{controller}/{id}",
            //   defaults: new { id = RouteParameter.Optional, controller = "CustomerV2Controller" },
            //   constraints: null,//new { controller = GetControllerNames() },
            //    handler: new ResponseWrappingHandler() { InnerHandler = new HttpControllerDispatcher(config) }
            //   );

            var doRequestResponseLog = Convert.ToBoolean(ConfigurationManager.AppSettings["doRequestResponseLog"]);
            if (doRequestResponseLog)
            {
                config.MessageHandlers.Add(new ApiLogHandler());
            }
            FluentValidationModelValidatorProvider.Configure(config);
            AutoMapperConfig.Configure();

            //var corsAttr = new EnableCorsAttribute("http://localhost:63080/", "*", "*");
            //config.EnableCors(corsAttr);
        }
    }
}
