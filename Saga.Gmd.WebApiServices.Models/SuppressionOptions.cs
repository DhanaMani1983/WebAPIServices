using Newtonsoft.Json;
using Saga.Gmd.WebApiServices.Common.JsonConverters;

namespace Saga.Gmd.WebApiServices.Models
{

    public class SuppressionOptions
    {
        [JsonConverter(typeof(JsonBooleanDefaultTrueConverter))]
        public bool IgnoreSuppression { get; set; }

    }
}
