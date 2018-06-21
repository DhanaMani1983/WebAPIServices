using FluentValidation.Attributes;
using Saga.Gmd.WebApiServices.Api.Models.ModelBinder;
using Saga.Gmd.WebApiServices.Api.Models.Validators;
using System.Web.Http.ModelBinding;

namespace Saga.Gmd.WebApiServices.Api.Models.ParameterModels
{
    [ModelBinder(typeof(MembershipProductSoldParameterModelBinder))]
    [Validator(typeof(MembershipProductSoldParameterModelValidator))]
    public class MembershipProductSoldParameterModel
    {
        public string ActivationId { get; set; }
        public string EncryptedActivationId { get; set; }
        public long? MembershipNo { get; set; }
        public string EncryptedMembershipNo { get; set; }

    }
}
