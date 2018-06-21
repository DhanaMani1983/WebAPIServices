using Newtonsoft.Json;

namespace Saga.Gmd.WebApiServices.Models.ReturnMe
{
    public class ReturnMePermissions
    {
        public bool Required { get; set; }
        public string ResponseParameter { get; set; }  // Summary, Full, Specific
        public string PermissionParameter { get; set; } //All,
        public string Journey { get; set; }

        [JsonIgnore]
        public string MatchType { get; set; }
    }
}
