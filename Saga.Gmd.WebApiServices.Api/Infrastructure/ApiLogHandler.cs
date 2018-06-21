using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Routing;
using Newtonsoft.Json;
using Saga.Gmd.WebApiServices.Api.Models;


namespace Saga.Gmd.WebApiServices.Api.Infrastructure
{
    public class ApiLogHandler : DelegatingHandler
    {
        private string _error = string.Empty;
        LogfileWriter writer = new LogfileWriter();
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
                CancellationToken cancellationToken)
        {
            if (request.RequestUri.PathAndQuery.Contains("swagger"))
                return await base.SendAsync(request, cancellationToken);

            var apiLogEntry = CreateApiLogEntryWithRequestData(request);


            if (request.Content != null)
            {
                await request.Content.ReadAsStringAsync()
                    .ContinueWith(task =>
                    {
                        apiLogEntry.RequestContentBody = task.Result;
                    }, cancellationToken);
            }

            return await base.SendAsync(request, cancellationToken)
                .ContinueWith(task =>
                { 
                    var response = task.Result;

                    // Update the API log entry with response info
                    apiLogEntry.ResponseStatusCode = (int)response.StatusCode;
                    apiLogEntry.ResponseTimestamp = DateTime.Now;

                    if (response.Content != null)
                    {
                        var byteResponse = response.Content.ReadAsByteArrayAsync().Result;
                        if (byteResponse != null && byteResponse.Length > 0)
                        {
                            var data = System.Text.Encoding.UTF8.GetString(byteResponse);

                            if (data != "null" && data != "Access denied, No Principal")
                            {
                                apiLogEntry.ResponseContentBody =
                                    JsonConvert.DeserializeObject(data)
                                        .ToString();
                            }
                            else
                            {
                                apiLogEntry.Error = data;

                            }
                        }

                        apiLogEntry.ResponseContentType = response.Content.Headers.ContentType.MediaType;
                        apiLogEntry.ResponseHeaders = SerializeHeaders(response.Content.Headers);
                    }
                     
                    writer.Write(apiLogEntry); 

                    return response;
                }, cancellationToken);
           //  return base.SendAsync(request, cancellationToken);
        }


        private ApiLogEntry CreateApiLogEntryWithRequestData(HttpRequestMessage request)
        {
            var context = ((HttpContextBase)request.Properties["MS_HttpContext"]);
            var routeData = request.GetRouteData();
            Guid id = Guid.NewGuid();
            return new ApiLogEntry
            {
                ApiLogEntryId = id,
                Application = "Customer API",
                User = context.User.Identity.Name,
                Machine = Environment.MachineName,
                RequestContentType = context.Request.ContentType,
                RequestRouteTemplate = routeData.Route.RouteTemplate,
                //RequestRouteData = SerializeRouteData(routeData),
                RequestIpAddress = context.Request.UserHostAddress,
                RequestMethod = request.Method.Method,
                RequestHeaders = SerializeHeaders(request.Headers),
                RequestTimestamp = DateTime.Now,
                RequestUri = request.RequestUri.ToString()
            };
        }

        private string SerializeRouteData(IHttpRouteData routeData)
        {
            return JsonConvert.SerializeObject(routeData, Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects, ReferenceLoopHandling = ReferenceLoopHandling.Serialize });
        }

        private string SerializeHeaders(HttpHeaders headers)
        {
            var dict = new Dictionary<string, string>();

            foreach (var item in headers.ToList())
            {
                if (item.Value != null)
                {
                    var header = String.Empty;
                    foreach (var value in item.Value)
                    {
                        header += value + " ";
                    }

                    // Trim the trailing space and add item to the dictionary
                    header = header.TrimEnd(" ".ToCharArray());
                    dict.Add(item.Key, header);
                }
            }

            return JsonConvert.SerializeObject(dict, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects, ReferenceLoopHandling = ReferenceLoopHandling.Serialize, Formatting = Formatting.Indented });

        }
    }
}