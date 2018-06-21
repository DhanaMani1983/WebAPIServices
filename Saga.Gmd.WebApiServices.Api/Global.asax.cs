using System.Net;
using System.Web.Http;
using System.Web.Routing;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Saga.Gmd.WebApiServices.Api.App_Start;
using Saga.Gmd.WebApiServices.Api.Infrastructure;

namespace Saga.Gmd.WebApiServices.Api
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            // Enforce the security protocol to TLS 1.2
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            GlobalConfiguration.Configure(WebApiConfig.Register);
            AutofacConfig.Configure();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            var config = new HttpConfiguration();
            config.Formatters.Remove(config.Formatters.XmlFormatter);
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.Formatting = Formatting.Indented; 
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            config.Formatters.JsonFormatter.SerializerSettings = settings; 

            config.MessageHandlers.Add(new ResponseWrappingHandler());
            log4net.Config.XmlConfigurator.Configure();
        }
    }
}