using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using System.Web.Http.ValueProviders;
using log4net;
using Saga.Gmd.WebApiServices.Api.Models.Parameters;
using Saga.Gmd.WebApiServices.Common;
using Saga.Gmd.WebApiServices.Models.Permissions;

namespace Saga.Gmd.WebApiServices.Api.Models
{
    public class GetParameterModelBinder : IModelBinder
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(GetParameterModelBinder));
        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        { 
            try
            {
                GetParameters parameters = new GetParameters();
                parameters.RequestIdentification =
                    bindingContext.GetValue("RequestIdentification").ToType<string>();
                parameters.IwillSend = bindingContext.GetValue("Iwillsend").ToType<string>();
                parameters.KeyValue.Key = bindingContext.GetValue("KeyValue.Key").ToType<string>();
                parameters.KeyValue.Value = bindingContext.GetValue("KeyValue.Value").ToType<string>();
                parameters.NameAndAddress.Title = bindingContext.GetValue("NameAndAdress.Title").ToType<string>();
                parameters.NameAndAddress.Surname = bindingContext.GetValue("NameAndAddress.Surname").ToType<string>();
                parameters.NameAndAddress.FirstName = bindingContext.GetValue("NameAndAddress.Firstname").ToType<string>();
                parameters.NameAndAddress.Dob = bindingContext.GetValue("NameAndAddress.Dob").ToType<DateTime>();
                parameters.NameAndAddress.Email = bindingContext.GetValue("NameAndAddress.Email").ToType<string>();
                parameters.NameAndAddress.Phone = bindingContext.GetValue("NameAndAddress.Phone").ToType<string>();
                parameters.NameAndAddress.Address.Address1 = bindingContext.GetValue("NameAndAddress.Address.Address1").ToType<string>();
                parameters.NameAndAddress.Address.Address2 = bindingContext.GetValue("NameAndAddress.Address.Address2").ToType<string>();
                parameters.NameAndAddress.Address.Address3 = bindingContext.GetValue("NameAndAddress.Address.Address3").ToType<string>();
                parameters.NameAndAddress.Address.Address4 = bindingContext.GetValue("NameAndAddress.Address.Address4").ToType<string>();
                parameters.NameAndAddress.Address.Postcode = bindingContext.GetValue("NameAndAddress.Address.Postcode").ToType<string>();
                parameters.ReturnMe.MailingHistory.Product = bindingContext.GetValue("Returnme.MailingHistory.Product").ToType<string>();
                parameters.ReturnMe.MailingHistory.FromDate = bindingContext.GetValue("ReturnMe.MailingHistory.FromDate").ToType<DateTime>();
                parameters.ReturnMe.MailingHistory.ToDate = bindingContext.GetValue("ReturnMe.MailingHistory.ToDate").ToType<DateTime>();
                parameters.ReturnMe.MailingHistory.FieldsTobeReturned =bindingContext.GetValue("ReturnMe.MailingHistory.FieldsTobeReturned").ToType<List<string>>();
                parameters.ReturnMe.CustomerKeys = bindingContext.GetValue("ReturnMe.CustomerKeys").ToType<List<string>>();

                parameters.Version = bindingContext.GetValue("Version").ToType<string>();
                if (string.IsNullOrEmpty(parameters.Version) || parameters.Version.ToLower() != "v1")
                {
                    bindingContext.ModelState.AddModelError("Version", "There is no version information found or has invalid version number in the URL");
                }

               // parameters.GroupCodes = parameters.ReturnMe.CustomerKeys.ToGroupCodeEnum();
                //Note, following logic to extract actual returnme items from the get parameter collection so that processing can be easier in later stage.
                // if you add new ReturnMe item that value should be added to where clause x.Contains("NewRequestMe")
                var returnMeItems = actionContext.Request.GetQueryNameValuePairs().Where(x => x.Key.ToLower().Contains("returnme")).SelectMany(s => s.Key.Split('.')).ToList();
                var distinctreturnMeItems = returnMeItems.Distinct().Where(x =>
                                x.ToLower().Contains("mailinghistory") || x.ToLower().Contains("customerkeys") ||
                                x.ToLower().Contains("permissions") || x.ToLower().Contains("membership"))
                        .ToList();
                parameters.ResponseRequestedItems = distinctreturnMeItems;
                parameters.ProcessStrategy = bindingContext.GetValue("Iwillsend").ToType<string>();


                bindingContext.Model = parameters;
            }
            catch (Exception ex)
            {
                log.Error("GetParameterModelBinder: " + ex.Message, ex);
            }
            return true;
        }



    }
}
