using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using System.Web.Security;
using log4net;
using log4net.Core;
using Saga.Gmd.WebApiServices.Models.Membership;
using Newtonsoft.Json;
using Saga.Gmd.WebApiServices.Api.Models.ParameterModels;
using Saga.Gmd.WebApiServices.Api.Models.Validators;
using Saga.Gmd.WebApiServices.BLL.Membership;
using Saga.Gmd.WebApiServices.Common;
using Saga.Gmd.WebApiServices.DAL.Membership;

namespace Saga.Gmd.WebApiServices.Api.Models.ModelBinder
{
    public class MembershipLoadModelBinder: IModelBinder
    {
        private IMembershipService _service;
        private static readonly ILog log = LogManager.GetLogger(typeof(MembershipLoadModelBinder));
        public MembershipLoadModelBinder()
        {
            _service = new MembershipService(new MembershipDataAccess(log, null), log);
        }

     
        public MembershipLoadModelBinder(IMembershipService service)
        {
            _service = service;
        }
        
        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            string body = actionContext.Request.Content.ReadAsStringAsync().Result;
            MembershipDataInputModel model = new MembershipDataInputModel();
            model = JsonConvert.DeserializeObject<MembershipDataInputModel>(body);

            // GITCS-9 : Support searching by membership no ...
            if (actionContext.ActionDescriptor.ActionName.ToLower().Contains("cancel"))
            {
                model.UpdatedStatus = ActionMembershipStatus.Cancel;
            }

            var activation = string.IsNullOrEmpty(model.ActivationId) ? model.EncryptedActivationId : model.ActivationId;
          
            MembershipHelper helper = new MembershipHelper(_service);
            MembershipDetails membershipDetails = helper.GetMembershipDetails(activation);

            if (membershipDetails.IsEligible != null) model.OverrideMust = membershipDetails.IsEligible.Value;
            var validator = new MembershipDataInputValidator();
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
