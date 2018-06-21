using Newtonsoft.Json;
using System;

namespace Saga.Gmd.WebApiServices.Common.JsonConverters
{
    public class JsonBooleanDefaultTrueConverter : JsonConverter
    {
        public override bool CanWrite { get { return false; } }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var value = reader.Value.ToString().ToLower().Trim();
            switch (value)
            {
                case "false":
                case "no":
                case "n":
                case "0":
                    return false;
            }
            return true;
        }

        public override bool CanConvert(Type objectType)
        {
            if (objectType == typeof(Boolean))
            {
                return true;
            }
            return false;
        }
    }
}
