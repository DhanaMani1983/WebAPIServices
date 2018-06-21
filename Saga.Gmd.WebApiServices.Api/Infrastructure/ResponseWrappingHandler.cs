using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using log4net;
using Newtonsoft.Json;
using Saga.Gmd.WebApiServices.Api.Models;
using Saga.Gmd.WebApiServices.Common;
using Saga.Gmd.WebApiServices.Models;

namespace Saga.Gmd.WebApiServices.Api.Infrastructure
{
    public class ResponseWrappingHandler : DelegatingHandler
    {


        private static readonly ILog log = LogManager.GetLogger(typeof(ResponseWrappingHandler));
        private string _requestIdentification = "";
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            ErrorTagProvider.ErrorTag = Guid.NewGuid().ToString("D");
            ErrorTagProvider.ErrorTagDatabase = Guid.NewGuid().ToString("N");
            var values = request.GetQueryNameValuePairs();
            _requestIdentification = values.GetEnumerator().ToIEnumerable().FirstOrDefault(x => x.Key == "RequestIdentification").Value;
            log.Info("RequestIdentification :" + _requestIdentification + " process started.");

            var response = await base.SendAsync(request, cancellationToken);

            return BuildApiResponse(request, response);

        }

        private HttpResponseMessage BuildApiResponse(HttpRequestMessage request, HttpResponseMessage response)
        {
            object content;
            List<string> modelStateErrors = new List<string>();

            if (response.TryGetContentValue(out content) && !response.IsSuccessStatusCode)
            {
                HttpError httpError = content as HttpError;
                if (httpError != null)
                {
                    content = null;

                    if (httpError.ModelState != null)
                    {
                        var httpErrorObject = response.Content.ReadAsStringAsync().Result;

                        var anonymousErrorObject = new { message = "", ModelState = new Dictionary<string, string[]>() };

                        var deserializedErrorObject = JsonConvert.DeserializeAnonymousType(httpErrorObject, anonymousErrorObject);

                        var modelStateValues = deserializedErrorObject.ModelState.Select(kvp => string.Join(". ", kvp.Value));

                        var stateValues = modelStateValues as string[] ?? modelStateValues.ToArray();
                        for (var i = 0; i < stateValues.Count(); i++)
                        {
                            modelStateErrors.Add(stateValues.ElementAt(i));
                        }
                    }
                    else if (httpError.ContainsKey("ExceptionType"))
                    {
                        if (httpError.ContainsValue("Saga.Gmd.WebApiServices.Common.DatabaseException"))
                        {
                            modelStateErrors.Add((httpError["ExceptionMessage"]?.ToString()));
                        }
                    }
                    else if (httpError.Message.Length > 0 && string.IsNullOrEmpty(httpError.ExceptionMessage))
                    {
                        modelStateErrors.Add(httpError.Message);
                    }
                    else if (!string.IsNullOrEmpty(httpError.Message))
                    {
                        modelStateErrors.Add(httpError.ExceptionMessage);
                    }
                }

            }
            string info = string.Empty;
            bool? IsAnyItemNull = null;
            //if (request.Method == HttpMethod.Get )
            //{
            switch (response.StatusCode)
            {

                case HttpStatusCode.OK:
                    {
                        object[] contents = content as object[];
                        if (contents != null)
                            foreach (var i in contents)
                            {
                                if (i != null)
                                {
                                    if (i.GetType().Name == "NoAccessToMailingHistory")
                                    {
                                        var hasMessage = ((Models.JsonModels.NoAccessToMailingHistory)i).Message;
                                        if (hasMessage == null)
                                        {
                                            IsAnyItemNull = true;
                                        }
                                    }
                                    if (i.GetType().Name == "CustomerKeys")
                                    {
                                        var hasPartialResult =
                                            ((Models.JsonModels.CustomerKeys)i).Keys?.HasPartialResult;
                                        if (hasPartialResult == null)
                                        {
                                            IsAnyItemNull = true;
                                        }
                                        if (hasPartialResult != null && !hasPartialResult.Value)
                                        {
                                            var message = ((Models.JsonModels.CustomerKeys)i).Keys.Message;
                                            if (!string.IsNullOrEmpty(message))
                                            {
                                                info = ErrorCodeInfo.RequestOkButResultNotFound;
                                            }
                                        }
                                    }
                                    else if (i.GetType().Name == "NoAccessToCusotmerKeys")
                                    {
                                        IsAnyItemNull = true;
                                        info = "No CustomerKeys found in the request";
                                    }
                                    else if ( (i.GetType().Name.ToString() == "HttpStatusCode") )
                                    {
                                        response.StatusCode = HttpStatusCode.NotFound;
                                        response.ReasonPhrase = "No Customer Found.";
                                        content = null;
                                    }
                                    else if (i is DatabaseException)
                                    {
                                        response.StatusCode = HttpStatusCode.NotFound;
                                        response.ReasonPhrase = "Invalid Data";
                                        content = ((DatabaseException)i).Message;
                                        break;
                                    }
                                }
                            }

                        if (content == null)
                        {
                            info = ErrorCodeInfo.NoDataFound;
                        }
                        else if (content is string)
                        {
                            
                                info = content.ToString();
                                content = null; 
                        }
                       
                        if (IsAnyItemNull.GetValueOrDefault())
                        {
                            info = "One or more object contains " +
                                   "'null'" +
                                   ", because  provided input didn't fetch any data.";
                        }
                    }
                    break;
                case HttpStatusCode.BadRequest:
                    info = ErrorCodeInfo.BadRequestInfo;
                    break;
                case HttpStatusCode.Unauthorized:
                    info = ErrorCodeInfo.UnAuthorizedInfo;
                    modelStateErrors.Add(response.ReasonPhrase);
                    break;
                case HttpStatusCode.InternalServerError:
                    //info = ErrorCodeInfo.InternalServerErrorInfo;
                    break;
                case HttpStatusCode.Forbidden:
                    info = ErrorCodeInfo.ForbiddenInfo;
                    break;
                default:
                    if (!modelStateErrors.Any())
                        modelStateErrors.Add(response.ReasonPhrase);
                    break;
            }
            //}





            var newResponse = request.CreateResponse(response.StatusCode, new ResponsePackage(content, response.StatusCode, response.ReasonPhrase, info, modelStateErrors));



            foreach (var header in response.Headers)
            {
                newResponse.Headers.Add(header.Key, header.Value);
            }
            log.Info("RequestIdentification: " + _requestIdentification + " process ended.");
            return newResponse;
        }
    }
}