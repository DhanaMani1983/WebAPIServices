using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Saga.Gmd.WebApiServices.Models.Customer;

namespace Saga.Gmd.WebApiServices.Models.Gdpr
{
    public class PermissionFull
    {
        public PermissionFull()
        {
            ChannelPostalAddressList = new List<CustomerAddress>();
            ChannelEmailList = new List<string>();
            ChannelSmsNoList = new List<string>();
            ChannelPhoneNoList = new List<string>();
            PermissionCategory = new List<ChannelFlags>();
        }

        public int PermissionId { get; set; }
        public string Source { get; set; }
        public bool Hac { get; set; }
        public bool ReConsentRequiredCore { get; set; }
        public string JourneyType { get; set; }
        public string Journey { get; set; }
        public DateTime? LastUpdatedDate { get; set; }
        public string LastUpdatedAgentName { get; set; }

        public List<ChannelFlags> PermissionCategory { get; set; }
        public CustomerAddress ChannelPostalAddress { get; set; }
        public string ChannelEmailAddress { get; set; }
        public string ChannelPhoneNo { get; set; }
        public string ChannelSmsNo { get; set; }

        [JsonProperty("ChannelPostalAddressList")]
        public List<CustomerAddress> ChannelPostalAddressList { get; set; }
        [JsonProperty("ChannelEmailAddressList")]
        public List<string> ChannelEmailList { get; set; }
        [JsonProperty("ChannelSMSNoList")]
        public List<string> ChannelSmsNoList { get; set; }
        [JsonProperty("ChannelPhoneNoList")]
        public List<string> ChannelPhoneNoList { get; set; }
        public DateTime CurrentDatetime => DateTime.UtcNow;
    }
}