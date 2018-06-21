using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net;
using Saga.Gmd.WebApiServices.Api.Models.JsonModels;
using Saga.Gmd.WebApiServices.Api.Models.ParameterModels;
using Saga.Gmd.WebApiServices.BLL.Customer;
using Saga.Gmd.WebApiServices.Common;
using Saga.Gmd.WebApiServices.Models;
using Saga.Gmd.WebApiServices.Models.Customer;

namespace Saga.Gmd.WebApiServices.Api.Components.KeyValuePair
{
    public interface ICustomerDetailsProcess
    {
        object Process(KeyValueParameter keyValueParameter );
    }

    public class CustomerDetailsProcess : ICustomerDetailsProcess
    {

        private readonly ICustomerDetailsService _customerDetailsService;
        private readonly ILog _logger;

        public CustomerDetailsProcess(
            ICustomerDetailsService customerDetailsService,
            ILog logger)
        {
            _customerDetailsService = customerDetailsService;
            _logger = logger;
        }

        public object Process( KeyValueParameter keyValueParameter )
        {
            KeyValueParameter _parameter = keyValueParameter;

            NoAccessToCustomerDetails message = new NoAccessToCustomerDetails();
            if (_parameter.AccessControl.ModuleAccess.Find(x => x.HasAccess && x.Key == GroupCode.GCUST) == null)
            {
                message.Message = "You dont' have access for CustomerDetails data.";
                return message;
            }

            CustomerInfoDetails customerInfoDetails =
                 _customerDetailsService.GetCustomerDetail(
                     new KeyValuePair<string, string>(_parameter.KeyValue.Key, _parameter.KeyValue.Value),
                     _parameter.ReturnMe.CustomerDetails.AddressType.Value,
                     _parameter.SuppressionOptions
                     );
            return customerInfoDetails;
        }
    }
}