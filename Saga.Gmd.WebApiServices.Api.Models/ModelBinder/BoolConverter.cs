using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Saga.Gmd.WebApiServices.Api.Models.ParameterModels;

namespace Saga.Gmd.WebApiServices.Api.Models.ModelBinder
{
    public class NameAndAddressParameterConverter : CustomCreationConverter<NameAndAddressParameter>
    {
        public override NameAndAddressParameter Create(Type objectType)
        {
            return new NameAndAddressParameter();
        }
    }
}
