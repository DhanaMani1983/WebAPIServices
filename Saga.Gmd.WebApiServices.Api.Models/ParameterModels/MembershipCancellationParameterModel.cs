using System.Collections.Generic;
using System.Web.Http.ModelBinding;
using FluentValidation.Attributes;
using Newtonsoft.Json;
using Saga.Gmd.WebApiServices.Api.Models.ModelBinder;
using Saga.Gmd.WebApiServices.Api.Models.Validators;
using Saga.Gmd.WebApiServices.Models.Membership;

namespace Saga.Gmd.WebApiServices.Api.Models.ParameterModels
{
    [ModelBinder(typeof(MembershipCancellationParameterModelBinder))]
    [Validator(typeof(MembershipCancellationParameterModelValidator))]
    public class MembershipCancellationParameterModel
    {
        public MembershipCancellationParameterModel()
        {
        }
        public long? MembershipNo { get; set; }
        public string EncryptedMembershipNo { get; set; }

        public string CancellationReason { get; set; }

        [JsonIgnore]
        public string OriginalStatus { get; set; }

        [JsonIgnore]
        public IEnumerable<MembershipOptions> ValidMembershipCancellationReasons { get; set; }

    }
}
