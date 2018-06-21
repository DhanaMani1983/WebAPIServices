using Newtonsoft.Json;
using Saga.Gmd.WebApiServices.Models.Customer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saga.Gmd.WebApiServices.Models.Gdpr
{

    public class PermissionsCustomerLoadModel
    {
        public PermissionsCustomerLoadModel()
        {
            PermissionCategory = new List<ChannelFlags>();
        }

        public KeyValuePair<string, string> CustomerKeyValue { get; set; }

        public NameAndAddress CustomerNameAndAddress { get; set; }

        public long PermissionsId { get; set; }

        public string Source { get; set; }
        public bool Hac { get; set; }
        public bool ReConsentRequiredCore { get; set; }

        public string JourneyType { get; set; }
        public string Journey { get; set; }

        public string QuestionId { get; set; }
      
        public DateTime? LastUpdatedDate { get; set; }
        public string LastUpdatedAgentName { get; set; }

        public List<ChannelFlags> PermissionCategory { get; set; }

        public CustomerAddress ChannelPostalAddress { get; set; }
        public string ChannelEmailAddress { get; set; }
        public string ChannelPhoneNo { get; set; }
        public string ChannelSmsNo { get; set; }

    }
}
