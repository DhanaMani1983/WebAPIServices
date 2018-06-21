using System.Collections.Generic;
using System.Web.Http.Description;
using FluentValidation.Attributes;
using Saga.Gmd.WebApiServices.Api.Models.Validators;
using Saga.Gmd.WebApiServices.Models;
using Saga.Gmd.WebApiServices.Models.Customer;
using System;
using Saga.Gmd.WebApiServices.Common;

namespace Saga.Gmd.WebApiServices.Api.Models.ParameterModels
{

    [Validator(typeof(PermissionsCustomerLoadValidator))]
    public class PermissionsCustomerLoad
    {
        public PermissionsCustomerLoad()
        {
            PermissionCategory = new List<ChannelFlagsPost>();
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

        public List<ChannelFlagsPost> PermissionCategory { get; set; }
        public CustomerAddress ChannelPostalAddress { get; set; }
        public string ChannelEmailAddress { get; set; }
        public string ChannelPhoneNo { get; set; }
        public string ChannelSmsNo { get; set; }

    }


    public class ChannelFlagsPost
    {
        public string PermissionCategoryStatus { get; set; }
        public string PermissionCategoryDisplayValue { get; set; }
        public DateTime? LastUpdatedDate { get; set; }
        public ChannelFagStatus ChannelPostFlag { get; set; }
        public ChannelFagStatus ChannelEmailFlag { get; set; }
        public ChannelFagStatus ChannelPhoneNoFlag { get; set; }
        public ChannelFagStatus ChannelSmsFlag { get; set; }
        public bool? ReConsentRequired { get; set; }
    }
}
