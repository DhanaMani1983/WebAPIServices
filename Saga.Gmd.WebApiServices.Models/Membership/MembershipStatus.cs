using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Saga.Gmd.WebApiServices.Models.Membership
{
    public class MembershipStatus
    {
        [JsonProperty("MembershipStatus")]
        public string MembershipStatusOptions { get; set; }
    }
}
