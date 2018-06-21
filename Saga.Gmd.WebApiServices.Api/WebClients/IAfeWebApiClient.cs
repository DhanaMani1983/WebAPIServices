using Saga.Gmd.WebApiServices.Api.WebClients.Dtos;
using System;
using System.Net;

namespace Saga.Gmd.WebApiServices.Api.WebClients
{
    public interface IAfeWebApiClient
    {
        AFECustomerChangeNotificationResponse CustomerChange(AFECustomerChangeNotificationRequest request);

        Action<string, string, string> OnCompleted { get; set; }
        Action<string, Exception, string> OnError { get; set; }


    }
}
