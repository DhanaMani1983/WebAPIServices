using System.Collections.Generic;
using System.Linq;
using System.Net.Configuration;
using System.Text;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using Newtonsoft.Json;
using Saga.Gmd.WebApiServices.Api.Models.Extensions;
using Saga.Gmd.WebApiServices.Api.Models.ParameterModels;
using Saga.Gmd.WebApiServices.Api.Models.Validators;
using Saga.Gmd.WebApiServices.Common;
using Saga.Gmd.WebApiServices.Models;
using IModelBinder = System.Web.Http.ModelBinding.IModelBinder;
using ModelBindingContext = System.Web.Http.ModelBinding.ModelBindingContext;
using Saga.Gmd.WebApiServices.Models.MailingHistory;

namespace Saga.Gmd.WebApiServices.Api.Models.ModelBinder
{
    public class NameAndAddressModelBinder : IModelBinder
    {
        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            string body = actionContext.Request.Content.ReadAsStringAsync().Result;
            var model = new NameAndAddressParameter();
            var a = JsonConvert.DeserializeObject<NameAndAddressParameter>(body );
            JsonConvert.PopulateObject(body, model);

            if ((model.ReturnMe.Permissions != null && model.ReturnMe.Permissions.Required))
            {
                model.ResponseRequestedItems.Add("permissions");
                model.NameAndAddress.Address.Address1 = BuildAddress1(model.NameAndAddress);
                model.NameAndAddress.Address.Address2 = model.NameAndAddress.Address.Address2 ?? (model.NameAndAddress.CustomerAddress.City ?? "");
                model.NameAndAddress.Address.Address3 = model.NameAndAddress.Address.Address3 ?? (model.NameAndAddress.CustomerAddress.County ?? "");
                model.NameAndAddress.Address.Address4 = model.NameAndAddress.Address.Address4 ?? (model.NameAndAddress.CustomerAddress.Country ?? "");
                model.NameAndAddress.Address.Postcode = model.NameAndAddress.Address.Postcode ?? model.NameAndAddress.CustomerAddress.Postcode;
                model.NameAndAddress.Address.MatchType = MatchType.NAME_AND_ADDRESS.ToLowerString();
                model.NameAndAddress.MatchType = MatchType.NAME_AND_ADDRESS.ToLowerString();
                model.ReturnMe.Permissions.MatchType = MatchType.NAME_AND_ADDRESS.ToLowerString();
            }
            if ((model.ReturnMe.Membership != null && model.ReturnMe.Membership.Required))
            {
                model.ResponseRequestedItems.Add("membership");
            }
            if (model.ReturnMe.MailingHistory != null)
            {
                model.ResponseRequestedItems.Add("mailinghistory");
                model.NameAndAddress.MatchType = MatchType.NAME_AND_ADDRESS.ToLowerString();
                model.ReturnMe.MailingHistory.MatchType = MatchType.NAME_AND_ADDRESS.ToLowerString();
            }
            List<string> emptyList = new List<string>();
            if (model.ReturnMe.CustomerKeys != null && model.ReturnMe.CustomerKeys.SequenceEqual(emptyList) || model.ReturnMe.CustomerKeys?.Count > 0)
            {
                model.ResponseRequestedItems.Add("customerkeys");
                model.AccessControl.CustomerKeyAccess = model.ReturnMe.CustomerKeys.ToCustomerKeyGroupCodeEnum();
            }
            if (model.ReturnMe.CustomerDetail != null && model.ReturnMe.CustomerDetail.Required)
            {
                model.ResponseRequestedItems.Add("customerdetail");
            }
            if (model.ReturnMe.TravelSummary != null && model.ReturnMe.TravelSummary.Required)
            {

                model.ResponseRequestedItems.Add("travelsummary");
            }
            // if (model.ReturnMe.CustomerMatch?.MatchType != null)
            if (model.ReturnMe.CustomerMatch != null)
            {
                //matchtype we need in all levels models to validate purpose, hence we hae MatchPrperty in child object of NameAndAddressParameter model
                model.ResponseRequestedItems.Add("customermatch");
                //
                // GITCS-3 : We now accept "null" match type value - must map to DEFAULT MatchType enum value for new default matching rules
                model.ReturnMe.CustomerMatch.MatchType =
                    model.ReturnMe.CustomerMatch.MatchType?.ToLower().Trim() == "null" 
                        ? MatchType.DEFAULT.ToString().ToLower()
                        : model.ReturnMe.CustomerMatch.MatchType?.ToLower().Trim()
                    ;
                // Synch the other 2 match types 
                model.NameAndAddress.MatchType = model.ReturnMe.CustomerMatch.MatchType;
                model.NameAndAddress.Address.MatchType = model.ReturnMe.CustomerMatch.MatchType;
                //
                // model.NameAndAddress.MatchType = model.ReturnMe.CustomerMatch.MatchType.ToLower().Trim();
                // model.NameAndAddress.Address.MatchType = model.ReturnMe.CustomerMatch.MatchType.ToLower().Trim();
            }


            model.AccessControl.ModuleAccess = model.ResponseRequestedItems.ToGroupCodeEnum();


            var validator = new NameAndAddressParameterValidator();
            var result = validator.Validate(model);

            foreach (var e in result.Errors)
            {
                bindingContext.ModelState.AddModelError(e.PropertyName, e.ErrorMessage);
            }

            bindingContext.Model = model;

            return true;
        }

        private string BuildAddress1(NameAndAddress nameAndAddress)
        {
            if (!string.IsNullOrWhiteSpace(nameAndAddress.Address.Address1))
            {
                return nameAndAddress.Address.Address1;
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                if (!string.IsNullOrWhiteSpace(nameAndAddress.CustomerAddress.HouseName))
                {
                    sb.Append(nameAndAddress.CustomerAddress.HouseName);
                    sb.Append(", ");
                }
                if (!string.IsNullOrWhiteSpace(nameAndAddress.CustomerAddress.HouseNumber))
                {
                    sb.Append(nameAndAddress.CustomerAddress.HouseNumber);
                    sb.Append(", ");
                }
                if (!string.IsNullOrWhiteSpace(nameAndAddress.CustomerAddress.Street))
                {
                    sb.Append(nameAndAddress.CustomerAddress.Street);
                    sb.Append(", ");
                }
                if (!string.IsNullOrWhiteSpace(nameAndAddress.CustomerAddress.Street1))
                {
                    sb.Append(nameAndAddress.CustomerAddress.Street1);
                }
                return sb.ToString().Trim().TrimEnd(','); ;
            }
        }
    }
}
