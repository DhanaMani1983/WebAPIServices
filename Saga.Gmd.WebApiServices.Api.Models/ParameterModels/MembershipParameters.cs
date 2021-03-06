﻿using System;
using Newtonsoft.Json;
using Saga.Gmd.WebApiServices.Models;
using Saga.Gmd.WebApiServices.Models.Security;
using FluentValidation.Attributes;
using Saga.Gmd.WebApiServices.Api.Models.Validators;

namespace Saga.Gmd.WebApiServices.Api.Models.ParameterModels
{
    [Validator(typeof(MembershipDataInputValidator))]
    public class MembershipParameters
    {
        public MembershipParameters()
        {
            CustNameAndAddress = new NameAndAddress();
            AccessControl = new AccessControl();
        }
        public NameAndAddress CustNameAndAddress { get; set; }
       [JsonIgnore]
        public int CustomerId { get; set; }
        [JsonIgnore]
        public bool? IsEligible { get; set; }
        public string Division { get; set; }
        public string ProductCode { get; set; }
        public string ActivationId { get; set; }
        public string EncryptedActivationId { get; set; }
        public long? MembershipNo { get; set; }
        public string EncryptedMembershipNo { get; set; }
        public string UpdatedStatus { get; set; }
        public string OriginalStatus { get; set; }
        public string UpdatedStatusReason { get; set; }
        public string AgentName { get; set; }
        public string SourceSystem { get; set; }
        //public DateTime?  CurrentDateTime { get; set; }
        public bool? OverrideFlag { get; set; }
        public string OverrideReason { get; set; }
        public string DeclineReason { get; set; }
        public string FulfilmentOverride { get; set; }
        public string ActivationSource { get; set; }
        public decimal? Premium { get; set; }
        public decimal? Revenue { get; set; }
        public DateTime? TransactionDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime EndDate { get; set; }
        [JsonIgnore]
        public AccessControl AccessControl { get; set; }
    }
}