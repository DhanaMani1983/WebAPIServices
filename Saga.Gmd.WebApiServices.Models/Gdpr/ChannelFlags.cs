using Saga.Gmd.WebApiServices.Common;
using System;

namespace Saga.Gmd.WebApiServices.Models.Gdpr
{
    public class ChannelFlags
    {
        public string PermissionCategoryStatus { get; set; }
        public string PermissionCategoryDisplayValue { get; set; }
        public DateTime? LastUpdatedDate { get; set; }
        public string ChannelPostFlag { get; set; }
        public string ChannelEmailFlag { get; set; }
        public string ChannelPhoneNoFlag { get; set; }
        public string ChannelSmsFlag { get; set; }
        public bool? ReConsentRequired { get; set; }
    }
}