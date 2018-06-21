using Saga.Gmd.WebApiServices.Api.WebClients.Dtos;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace Saga.Gmd.WebApiServices.Api.WebClients
{
    public class AfeWebApiClient : IAfeWebApiClient
    {

        private readonly HttpClient _client;
        private readonly string _targetActionName;

        public AfeWebApiClient(
             Uri baseUri, string targetActionName 
            )
        {
            if (baseUri == null) throw new ArgumentNullException(nameof(baseUri));

            _client = new HttpClient();

            _client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            _client.BaseAddress = baseUri;
            _targetActionName = targetActionName;

        }

        public Action<string, string, string> OnCompleted { get; set; }
        public Action<string, Exception, string> OnError { get; set; }


        public AFECustomerChangeNotificationResponse CustomerChange(AFECustomerChangeNotificationRequest request)
        {
            string postdataJson = null;
            var retval = new AFECustomerChangeNotificationResponse();
            try
            {
                postdataJson = JsonConvert.SerializeObject(request);
                //var postdataString = new StringContent(postdataJson, new UTF8Encoding(), "application/json");

                var response = _client.PostAsJsonAsync(_targetActionName, request).Result;
                // This extension handles json serialization of the payload etc...

                retval.Success = response.IsSuccessStatusCode;

                OnCompleted?.Invoke($"{_client.BaseAddress.AbsoluteUri}{_targetActionName}",
                    response.ToString(), postdataJson);

            }
            catch (Exception ex)
            {
                retval.Success = false;
                OnError?.Invoke($"{_client.BaseAddress.AbsoluteUri}{_targetActionName}", ex, postdataJson);
            }

            return retval;
        }

    }
}