using Newtonsoft.Json;

namespace Saga.Gmd.WebApiServices.Models.Membership
{
    public class MembershipParam
    {
        [JsonIgnore]
        public string ActivationId { get; set; }
        [JsonIgnore]
        public string EncryptedActivationId { get; set; }
       
        [JsonIgnore]
        public string CustomerId { get; set; }
        public bool Required { get; set; }
    }


}