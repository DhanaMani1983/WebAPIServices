using System.Linq;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using Autofac.Integration.WebApi;
using Newtonsoft.Json;
using Saga.Gmd.WebApiServices.Api.Models.ParameterModels;
using Saga.Gmd.WebApiServices.Api.Models.Validators;
using Saga.Gmd.WebApiServices.DAL.Membership;
using Saga.Gmd.WebApiServices.Models.Membership;

namespace Saga.Gmd.WebApiServices.Api.Models.ModelBinder
{
    public class MembershipCancellationParameterModelBinder : IModelBinder
    {

        private IMembershipDataAccess _membershipDataAccess;

        public MembershipCancellationParameterModelBinder()
        {
        }


        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            string body = actionContext.Request.Content.ReadAsStringAsync().Result;
            MembershipCancellationParameterModel model = new MembershipCancellationParameterModel();
            model = JsonConvert.DeserializeObject<MembershipCancellationParameterModel>(body);

            // Get current membership to calidate existing status ..
            _membershipDataAccess =
                (IMembershipDataAccess)
                (actionContext.ControllerContext.Configuration.DependencyResolver as AutofacWebApiDependencyResolver)
                .GetService(typeof(IMembershipDataAccess));

            MembershipDetails membershipDetails =
                _membershipDataAccess.GetMembershipDetails<long, MembershipDetails>( model.MembershipNo??default(long)  );
            // Call new Generic DA method 
            if (membershipDetails.MembershipNo.HasValue)
            {
                model.OriginalStatus = membershipDetails.MembershipStatus;
            }
            // GITCS-9 - cancellation reason
            model.ValidMembershipCancellationReasons =
                _membershipDataAccess.GetMembershipOptions("MEMBERSHIP_CANCEL_REASON").Where(mo => mo.CodeList == "MEMBERSHIP_CANCEL_REASON");

            var validator = new MembershipCancellationParameterModelValidator();
            var result = validator.Validate(model);

            foreach (var e in result.Errors)
            {
                bindingContext.ModelState.AddModelError(e.PropertyName, e.ErrorMessage);
            }
            bindingContext.Model = model;
            return true;

        }

    }

}
