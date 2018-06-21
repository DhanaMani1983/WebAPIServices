using System;
using System.Web.Http;
using System.Web.Http.Description;
using AutoMapper;
using Saga.Gmd.WebApiServices.Api.Infrastructure;
using Saga.Gmd.WebApiServices.Api.Models;
using Saga.SSO.ClientCredentials.Endpoint.ApiFilters;
using System.Threading;
using Saga.Gmd.WebApiServices.Api.WebClients.Dtos;

namespace Saga.Gmd.WebApiServices.Api.Controllers
{
    public partial class CustomerController
    {
        private LogfileWriter _logfileWriter = new LogfileWriter();
        private const string _logFileTimeFormat = "dd/MM/yyyy HH:mm:ss.fff";

        [ActionName("CustomerLoad")]
        [ResponseType(typeof(ResponsePackage))]
        [AuthenticateIdentity()]
        [AuthorizeIdentity(Role = "CUSTOMER_LOAD")]
        [HttpPost]
        public IHttpActionResult PostCustomer(Models.ParameterModels.CustomerLoadOrMatch customer
            )
        {


            var mappedCustomer = Mapper.Map<WebApiServices.Models.Customer.CustomerLoadOrMatch>(customer);
            // customer.AccessControl = _clientScopeService.VerifyUserHasAccessToGroupCode(customer.AccessControl, Scope.CUSTOMER_LOAD);

            int customerId;
            DateTime processedDatetime;
            var id = _mciRequestService.SaveCustomer(mappedCustomer, out customerId, out processedDatetime);
            // This is the [RecordID] PKey of the [MCI_CUSTOMER_API_LANDING] table

            string message = (id > 0) 
                ? $"Customer loaded successfully. [Id={id}]"
                : $"Customer load failed. [Id={id}]"; 

            // Callback to Afe WebApi notify of customer change : No async call currently 
            //
            //Task.Factory.StartNew(() =>
            //{
            AFECustomerChangeNotificationResponse afeNotificationResponse = NotifyChangeToAfe(customer, processedDatetime, id);
            //};

            // Update local Afe log table to reflect the call above ...
            CreateAfeCallbackDbLog(customerId, afeNotificationResponse);

            return Ok(message);
        }

        private void CreateAfeCallbackDbLog(int customerId, AFECustomerChangeNotificationResponse afeNotificationResponse)
            {
            
            LogDbAction("Finally updating the database");
            //Fire and Forget call to AFE Web Api
            //NOTE: read the comments below and take the necessary steps
            //Task.Factory.StartNew(() =>
            //{
                try
                {
                // wether API Success or Fail need to update the database, so windows service will pick up and process(Windows service need to be build)
                _gmdToAfeService.Save(customerId, afeNotificationResponse.Success);
                                                                         //Build the request and make a call based on AFE API Requirement
                                                                         //use the model 'MciCustomerInfoModel' to build request
                                                                         //you may wish to use 'AfeNotificationService' to extract the code from here, (this implimentation not completed)
            }
                // turn our request string into a byte stream
            catch (Exception ex)
                // now send it
                // grab te response and print it out to the console along with the status code
                {
                LogDbAction("Error while executing _gmdToAfeService.Save method--> " + ex.Message);
                }

            LogDbAction("Updated the database");
            }

        private void LogDbAction( string message )
            {
            var threadId = Thread.CurrentThread.ManagedThreadId.ToString();
            _logfileWriter.Write(string.Format("{0} [{1}] - {2}", DateTime.Now.ToString( _logFileTimeFormat ), threadId, message + "\r\n"));
            }

        private AFECustomerChangeNotificationResponse NotifyChangeToAfe(Models.ParameterModels.CustomerLoadOrMatch customer, DateTime processedDatetime, long id)
            {
            
            //
            var afeNotificationRequest = new AFECustomerChangeNotificationRequest
                {
                notification =
                new AFECustomerChangeNotification
                    {
                    Id = id.ToString(), // customerId.ToString(),
                    Source = "CWS",
                    Code = customer.Customer.SystemSource,
                    Value = customer.Customer.SystemId,
                    OriginatingSource = customer.Customer.SystemSource.Substring(0, 3),
                    // Timestamp = processedDatetime.ToString()
                    // AFE Endpoint supported Timestamp format 
                    Timestamp = processedDatetime.ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'")
                    //"2018-01-24T12:41:30.937Z"
                    // wether API Success or Fail need to update the database, so windows service will pick up and process(Windows service need to be build)
                }
            };
            // Assign local delegates to log outcome ...
            _afeWebClient.OnCompleted = LogAfeCallSuccess;
            _afeWebClient.OnError = LogAfeCallException;

            // Call AfeWebClient to invoke the callback
            var afeNotificationResponse = _afeWebClient.CustomerChange(afeNotificationRequest);
            
            return afeNotificationResponse;
                }

        private void LogAfeCallSuccess(string requestUrl, string result, string jsonRequest )
                {
            var threadId = Thread.CurrentThread.ManagedThreadId.ToString();
            _logfileWriter.Write(
                $"{DateTime.Now.ToString(_logFileTimeFormat)} [{threadId}] - Target Url: {requestUrl + Environment.NewLine}"
            );
            _logfileWriter.Write(
                $"{DateTime.Now.ToString(_logFileTimeFormat)} [{threadId}] - Request: {jsonRequest + Environment.NewLine}"    
                );
            _logfileWriter.Write(
                $"{DateTime.Now.ToString(_logFileTimeFormat)} [{threadId}] - Response: {result + Environment.NewLine}"
            );

            // response.Close();
                }

        private void LogAfeCallException(string requestUrl, Exception ex, string jsonRequest)
        {
            var threadId = Thread.CurrentThread.ManagedThreadId.ToString();
            _logfileWriter.Write(
                $"{DateTime.Now.ToString(_logFileTimeFormat)} [{threadId}] - Target Url: {requestUrl + Environment.NewLine}"
            );
            _logfileWriter.Write(
                $"{DateTime.Now.ToString(_logFileTimeFormat)} [{threadId}] - Request: {jsonRequest + Environment.NewLine}"
            );
            _logfileWriter.Write(
                $"{DateTime.Now.ToString(_logFileTimeFormat)} [{threadId}] - {"Error Message: " + ex.Message + "\r\n"}");
            _logfileWriter.Write(
                $"{DateTime.Now.ToString(_logFileTimeFormat)} [{threadId}] - {"Inner Exception: " + ex.InnerException?.Message}");
        }

    }
}