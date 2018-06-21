using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using Saga.SSO.ClientCredentials.Endpoint.ApiFilters;
using Swashbuckle.Swagger;

namespace Saga.Gmd.WebApiServices.Api.Infrastructure
{
    public class AssignOAuth2SecurityRequirements : IOperationFilter
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="schemaRegistry"></param>
        /// <param name="apiDescription"></param>
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {

            var actFilters = apiDescription.ActionDescriptor.GetFilterPipeline();
            var allowAnonymous = actFilters.Select(f => f.Instance).OfType<OverrideActionFiltersAttribute>().Any();
            if (allowAnonymous)
                return;
            Collection<AuthorizeIdentity> authorized = apiDescription.ActionDescriptor.GetCustomAttributes<AuthorizeIdentity>();
            if (!authorized.Any()) return;
            List<string> role = new List<string> { authorized[0].Role };

            List<IDictionary<string, IEnumerable<string>>> roles = new List<IDictionary<string, IEnumerable<string>>>
            {
                new Dictionary<string, IEnumerable<string>>
            {
                {"oauth2",role}
            }};
            operation.security = roles;

            Dictionary<string, IEnumerable<string>> oAuthRequirements = null;
            if (operation.security == null)
            {
                operation.security = new List<IDictionary<string, IEnumerable<string>>>();

                oAuthRequirements = new Dictionary<string, IEnumerable<string>>
                {
                    {"oauth2", Enumerable.Empty<string>()}
                };
            }

            operation.security.Add(oAuthRequirements);
        }


    }
}