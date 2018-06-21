using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Saga.Gmd.WebApiServices.Models.Membership;

namespace Saga.Gmd.WebApiServices.Api.Models.JsonModels
{
    public class MembershipOutput
    {

        public MembershipOutput()
        {
            MembershipData = new MembershipDetails();
        }
        [JsonProperty("Membership")]
        public MembershipDetails MembershipData { get; set; }
    }
}
