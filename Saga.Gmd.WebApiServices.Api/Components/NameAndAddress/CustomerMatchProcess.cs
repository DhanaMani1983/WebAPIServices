using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using log4net;
using Newtonsoft.Json;
using Saga.Gmd.WebApiServices.Api.Models.ParameterModels;
using Saga.Gmd.WebApiServices.BLL.Customer;
using Saga.Gmd.WebApiServices.Common;
using Saga.Gmd.WebApiServices.Models;
using Saga.Gmd.WebApiServices.Models.Customer;

namespace Saga.Gmd.WebApiServices.Api.Components.NameAndAddress
{
    public class CustomerMatchProcess
    {
        private readonly NameAndAddressParameter _nameAndAddress;

        private readonly ICustomerMatchService _customerMatchService;
        private ITravelSummaryService _travelSummaryService;

        private readonly ILog _logger;
        private readonly bool _logParameterValue;

        public CustomerMatchProcess(NameAndAddressParameter nameAndAddress, ICustomerMatchService customerMatchService, ITravelSummaryService travelSummaryService, ILog logger)
        {
            _nameAndAddress = nameAndAddress;
            _travelSummaryService = travelSummaryService;
            _customerMatchService = customerMatchService;
            _logger = logger;
            _logParameterValue = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableObjectDump"]);
        }

        public CustomerMatchDetails Process()
        {
            CustomerMatchDetails details = new CustomerMatchDetails();
            if (_nameAndAddress.AccessControl.ModuleAccess.Find(x => x.HasAccess && x.Key == GroupCode.CMACH) == null)
            {

                //message.Message = "You dont' have access for CustomerMatch data.";
                //return message;
            }

            try
            {
                details.CustomerMatchInfoDetailes.AddRange(_customerMatchService.GetMatchingCustomers(_nameAndAddress.NameAndAddress, _nameAndAddress.SuppressionOptions, _nameAndAddress.ReturnMe.CustomerMatch.MatchType));
            }
            //catch (SqlException ex)
            //{
            //    _logger.Error("CustomerMatchProcess: " + "ErrorTag: " + ErrorTagProvider.ErrorTag + " -- " + ex.Message, ex);
            //    throw new Exception(ex.Message);
            //}
            catch (Exception ex)
            {
                _logger.Error("CustomerMatchProcess: " + "ErrorTag: " + ErrorTagProvider.ErrorTag + " -- " + ex.Message, ex);

                if (_logParameterValue)
                {
                    _logger.Error(
                        $"Parameters ProcessPermissionForNameAndAddress:- Name=FirstName={_nameAndAddress.NameAndAddress.FirstName}, LastName={_nameAndAddress.NameAndAddress.Surname}, Dob={_nameAndAddress.NameAndAddress.Dob}, " +
                        $"Address=AddresLine1={_nameAndAddress.NameAndAddress.Address.Address1}, AddressLine2={_nameAndAddress.NameAndAddress.Address.Address2}, AddressLine3={_nameAndAddress.NameAndAddress.Address.Address3}, " +
                        $"AddressLine4={_nameAndAddress.NameAndAddress.Address.Address4}, PostCode={_nameAndAddress.NameAndAddress.Address.Postcode}");
                }

                throw new Exception(ex.Message);
            }


            return details;
        }

        public TravelSummaryOutput TravelSummaryProcess(int numerId, WebApiServices.Models.NameAndAddress nameAndAddress = null)
        {
            TravelSummaryOutput  output = new TravelSummaryOutput();

            output.TravelSummary = _travelSummaryService.GetTravelSummary(numerId, nameAndAddress);
            return output;
        }
    }

    public class TravelSummaryOutput
    {
        [JsonProperty("TravelSummary")]
        public WebApiServices.Models.Customer.TravelSummary TravelSummary { get; set; }

    }
}