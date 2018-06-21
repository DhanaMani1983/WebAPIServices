using Newtonsoft.Json;
using Saga.Gmd.WebApiServices.Models.Membership;

namespace Saga.Gmd.WebApiServices.Api.Models.JsonModels
{
    public class MembershipOptionsDataOutput
    {
        public MembershipOptionsDataOutput()
        {
            MembershipOptionsData = new MembershipOptionsData();
           
        }

        [JsonProperty("MembershipOptions")]
        public MembershipOptionsData MembershipOptionsData { get; set; }
        

    }

    public class MembershipOptionsDataOutputV2
    {
        public MembershipOptionsDataOutputV2()
        {
            MembershipOptionsData = new MembershipOptionsDataV2();

        }

        [JsonProperty("MembershipOptions")]
        public MembershipOptionsDataV2 MembershipOptionsData { get; set; }


    }


}
