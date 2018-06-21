namespace Saga.Gmd.WebApiServices.Api.WebClients.Dtos
{
    public class AFECustomerChangeNotificationRequest
    {
        public AFECustomerChangeNotification notification { get; set; }
    }

    public class AFECustomerChangeNotification
    {
        public string Id { get; set; }
        public string Source { get; set; }
        public string OriginatingSource { get; set; }
        public string OriginatingSourceId { get; set;  }
        public string OriginatingChangeTimestamp { get; set;  }
        public string Code { get; set; }
        public string Value { get; set; }
        public string Timestamp { get; set; }

    }
}