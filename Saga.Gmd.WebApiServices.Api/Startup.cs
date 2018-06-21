using System;
using System.Configuration;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using Saga.SSO.ClientCredentials.Endpoint.Extensions;
using Saga.SSO.ClientCredentials.Endpoint.Options;

[assembly: OwinStartup(typeof(Saga.Gmd.WebApiServices.Api.Startup))]

namespace Saga.Gmd.WebApiServices.Api
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var ccoptions = new ApiIdentityOptions
            {
                IdpMetaDataUri = ConfigurationManager.AppSettings.Get("SSO_IdpMetaDataUri"),
                BlockAnonymous = false,//(!string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings.Get("SSO_BlockAnonymous"))),
                IgnoreTLSWarnings = (!string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings.Get("SSO_IgnoreTLSWarnings"))),
                RequireTLS = (!string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings.Get("SSO_RequireTLS"))),
                Audience = ConfigurationManager.AppSettings.Get("SSO_Audience"),
                Logging = new ApiLoggingOptions
                {
                    LogLevel = Saga.SSO.Common.Helpers.LogDetailHelper.SetLogLevel(ConfigurationManager.AppSettings.Get("SSO_LogLevel")),
                    SaveToPath = ConfigurationManager.AppSettings.Get("SSO_LogFolder")
                },
                //  UseWithHybridLibraries = true// (!string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings.Get("OAUTH2.Identity.UseWithHybridLibraries")))
            };

            app.UseSagaClientCredentialsFlowMiddleWare(ccoptions);

        }

    }
}
