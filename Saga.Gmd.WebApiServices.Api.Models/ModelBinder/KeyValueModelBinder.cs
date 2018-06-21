using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using Saga.Gmd.WebApiServices.Api.Models.Extensions;
using Saga.Gmd.WebApiServices.Common;
using Saga.Gmd.WebApiServices.Api.Models.ParameterModels;
using Saga.Gmd.WebApiServices.Api.Models.Validators;
using Saga.Gmd.WebApiServices.Models.MailingHistory;
using Saga.Gmd.WebApiServices.Models.Permissions;

namespace Saga.Gmd.WebApiServices.Api.Models.ModelBinder
{
    public class KeyValueModelBinder : IModelBinder
    {
        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            KeyValueParameter model = new KeyValueParameter();

            var a = actionContext.Request.Content.ReadAsStringAsync().Result;
            model.KeyValue.Key = bindingContext.GetValue("KeyValue.Key").ToType<string>();
            model.KeyValue.Value = bindingContext.GetValue("KeyValue.Value").ToType<string>();

            model.SuppressionOptions.IgnoreSuppression =
                string.IsNullOrEmpty( bindingContext.GetValue("SuppressionOptions.IgnoreSuppression") ) || !bindingContext.GetValue("SuppressionOptions.IgnoreSuppression").IsBooleanString() 
                ? true
                : bindingContext.GetValue("SuppressionOptions.IgnoreSuppression").ToType<bool>();

            var queryValuePairs = actionContext.Request.GetQueryNameValuePairs();
            var keyValuePairs = queryValuePairs as KeyValuePair<string, string>[] ?? queryValuePairs.ToArray();
            var returnMeItems = keyValuePairs.ToList();
            var items = (from item in returnMeItems where item.Key.ToLower().Contains("returnme")
                select item.Key.Split('.') 
                into splitItems
                select splitItems[1].ToLower()).ToList();
            // Get ReturnMe.XXX Items from the param list  - Get the XXX item names from the query params 

            model.ReturnMe.CustomerKeys = bindingContext.GetValue("ReturnMe.CustomerKeys") == null ? null : bindingContext.GetValue("ReturnMe.CustomerKeys").ToType<List<string>>();
            model.ReturnMe.Permissions.Required = bindingContext.GetValue("ReturnMe.Permissions.Required") != null && bindingContext.GetValue("ReturnMe.Permissions.Required").ToType<bool>();
            model.ReturnMe.TravelSummary.Required = bindingContext.GetValue("ReturnMe.TravelSummary.Required") != null &&
                                                    bindingContext.GetValue("ReturnMe.TravelSummary.Required")
                                                        .ToType<bool>();
            if (model.ReturnMe.Permissions != null)
            {
                model.ReturnMe.Permissions.ResponseParameter = bindingContext.GetValue("Returnme.Permissions.ResponseParameter").ToType<string>();
                model.ReturnMe.Permissions.PermissionParameter = bindingContext.GetValue("ReturnMe.Permissions.PermissionParameter").ToType<string>();
                model.ReturnMe.Permissions.Journey = bindingContext.GetValue("ReturnMe.Permissions.Journey").ToType<string>();
               
            }

            model.ReturnMe.MembershipFlags.Required = bindingContext.GetValue("ReturnMe.MembershipFlags.Required") != null && bindingContext.GetValue("ReturnMe.MembershipFlags.Required").ToType<bool>();
            model.ReturnMe.Membership.Required = bindingContext.GetValue("ReturnMe.Membership.Required") != null && bindingContext.GetValue("ReturnMe.Membership.Required").ToType<bool>();
            model.ReturnMe.CustomerDetails.Required = bindingContext.GetValue("Returnme.CustomerDetails.Required") != null && bindingContext.GetValue("Returnme.CustomerDetails.Required").ToType<bool>();
            model.ReturnMe.CustomerDetails.AddressType = bindingContext.GetValue("Returnme.CustomerDetails.AddressType").ToAddressType() ?? AddressType.Correspondence;

            if (items.Contains("mailinghistory"))
            {
                model.ReturnMe.MailingHistory = new MailingHistory();
            }
            if (model.ReturnMe.MailingHistory != null)
            {
                model.ReturnMe.MailingHistory.Product = bindingContext.GetValue("Returnme.MailingHistory.Product").ToType<string>();
                model.ReturnMe.MailingHistory.FromDate = bindingContext.GetValue("ReturnMe.MailingHistory.FromDate").ToType<DateTime>();
                model.ReturnMe.MailingHistory.ToDate = bindingContext.GetValue("ReturnMe.MailingHistory.ToDate").ToType<DateTime>();
                model.ReturnMe.MailingHistory.FieldsTobeReturned = bindingContext.GetValue("ReturnMe.MailingHistory.FieldsTobeReturned").ToType<List<string>>();
            }

            // Note, following logic to extract actual returnme items from the get parameter collection so that processing can be easier in later stage.
            // if you add new ReturnMe item that value should be added to where clause x.Contains("NewRequestMe")
            var distinctreturnMeItems = items.Distinct().Where(x =>
                   x.ToLower().Contains("mailinghistory") 
                || x.ToLower().Contains("customerkeys") 
                || x.ToLower().Contains("permissions") 
                || x.ToLower().Contains("membership") 
                || x.ToLower().Contains("customerdetails")
                || x.ToLower().Contains("travelsummary")
                || x.ToLower().Contains("membershipflags"))
                .ToList();

            model.ResponseRequestedItems = distinctreturnMeItems;
            model.AccessControl.CustomerKeyAccess = model.ReturnMe.CustomerKeys?.ToCustomerKeyGroupCodeEnum();
            model.AccessControl.ModuleAccess = model.ResponseRequestedItems.ToGroupCodeEnum();


            var validator = new KeyValueParameterValidator();
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
